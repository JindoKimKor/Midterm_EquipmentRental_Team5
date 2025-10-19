<template>
  <v-card elevation="3" rounded>
    <v-card-title class="d-flex align-center">
      <span class="text-h5 font-weight-semibold">Customer List</span>
      <v-spacer />
      <add-customer-dialog
        v-model="isAddCustomerDialogOpen"
        :customer="selectedCustomer"
        @saved="refreshCustomers"
      />
    </v-card-title>

    <v-data-table
      :headers="headers"
      :items="customersArr"
      :items-per-page="10"
      class="elevation-1"
      dense
      hover
      fixed-header
      height="600"
    >
      <template #item.userName="{ item }">
        <router-link :to="`customers/${item.id}`" class="text-decoration-none font-weight-medium">
          {{ item.userName }}
        </router-link>
      </template>

      <template #item.password="{ item }">
        <v-icon
          class="mr-2"
          small
          style="cursor: pointer"
          @click="togglePasswordVisibility(item.id)"
          :title="showPasswords[item.id] ? 'Hide Password' : 'Show Password'"
        >
          {{ showPasswords[item.id] ? 'mdi-eye-off' : 'mdi-eye' }}
        </v-icon>
        <span>{{ showPasswords[item.id] ? item.password : maskedPassword(item.password) }}</span>
      </template>

      <template #item.actions="{ item }">
        <v-tooltip text="Edit" location="top">
          <template #activator="{ props }">
            <v-btn
              icon
              size="small"
              color="blue darken-2"
              @click="editCustomerHandler(item)"
              v-bind="props"
              aria-label="Edit customer"
            >
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
          </template>
        </v-tooltip>

        <v-tooltip text="Delete" location="top">
          <template #activator="{ props }">
            <v-btn
              icon
              size="small"
              color="red darken-2"
              @click="deleteCustomerHandler(item.id)"
              v-bind="props"
              aria-label="Delete customer"
            >
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-tooltip>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import AddCustomerDialog from '@/components/dialog/AddCustomerDialog.vue'
import { getAllCustomers, deleteCustomer } from '@/api/CustomerController'

const isAddCustomerDialogOpen = ref(false)
const selectedCustomer = ref(null)
const customersArr = ref([])

// Track password visibility per customer id
const showPasswords = ref({})

const headers = [
  { title: 'Username', value: 'userName' },
  { title: 'Password', value: 'password' },
  { title: 'Name', value: 'name' },
  { title: 'Email', value: 'email' },
  { title: 'Role', value: 'role' },
  { title: 'Actions', value: 'actions', sortable: false },
]

const loadCustomers = async () => {
  try {
    customersArr.value = await getAllCustomers()
    customersArr.value.forEach((customer) => {
      showPasswords.value[customer.id] = false
    })
  } catch (error) {
    console.error('Failed to load customers:', error)
  }
}

onMounted(loadCustomers)

const editCustomerHandler = (item) => {
  selectedCustomer.value = item
  isAddCustomerDialogOpen.value = true
}

const deleteCustomerHandler = async (id) => {
  if (confirm('Delete customer and rental history? This action cannot be undone.')) {
    try {
      await deleteCustomer(id)
      await loadCustomers()
    } catch (error) {
      console.error('Failed to delete customer:', error)
    }
  }
}

const refreshCustomers = () => {
  loadCustomers()
}

const togglePasswordVisibility = (id) => {
  showPasswords.value[id] = !showPasswords.value[id]
}

const maskedPassword = (password) => {
  if (!password) return ''
  return '•'.repeat(password.length)
}
</script>

<style scoped>
.text-decoration-none {
  text-decoration: none;
}

.font-weight-medium {
  font-weight: 500;
}
</style>
