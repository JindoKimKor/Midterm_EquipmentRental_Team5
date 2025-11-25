<template>
  <v-card elevation="3" rounded>
    <v-card-title class="d-flex align-center">
      <span class="text-h5 font-weight-semibold">Customer List</span>
      <v-spacer />
      <add-customer-dialog
        v-model="isAddCustomerDialogOpen"
        :customer="selectedCustomer"
        @saved="refreshCustomers"
        @closed="cleanup"
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
        <span class="font-weight-medium">{{ item.userName }}</span>
      </template>

      <template #item.details="{ item }">
        <v-tooltip text="View customer details" location="top">
          <template #activator="{ props }">
            <v-btn
              v-bind="props"
              :to="`customers/${item.id}`"
              color="primary"
              variant="flat"
              size="small"
              class="text-capitalize"
              aria-label="View customer details"
            >
              <v-icon start size="18" class="mr-1">mdi-eye</v-icon>
              Details
            </v-btn>
          </template>
        </v-tooltip>
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

const showPasswords = ref({})

const headers = [
  { title: 'Username', value: 'userName' },
  { title: 'Email', value: 'email' },
  { title: 'Details', value: 'details', sortable: false },
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

const cleanup = () => {
  selectedCustomer.value = null
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
</script>

<style scoped>
.text-decoration-none {
  text-decoration: none;
}

.font-weight-medium {
  font-weight: 500;
}
</style>
