<template>
  <v-container fluid>
    <CustomerTable />
    <v-dialog v-model="showDeleteDialog" max-width="400">
      <v-card>
        <v-card-title class="text-h6">Delete this customer?</v-card-title>
        <v-card-text>
          Are you sure you want to delete <strong>{{ deletingCustomer?.name }}</strong> and their
          rental history?
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="showDeleteDialog = false">Cancel</v-btn>
          <v-btn color="red" @click="deleteCustomer">Yes, Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Snackbar -->
    <v-snackbar v-model="snackbar.show" :color="snackbar.color" timeout="3000">
      {{ snackbar.message }}
    </v-snackbar>
  </v-container>
</template>

<script setup>
import CustomerTable from '@/components/tables/CustomerTable.vue'
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const customers = ref([])
const showDeleteDialog = ref(false)
const deletingCustomer = ref(null)
const snackbar = ref({
  show: false,
  message: '',
  color: '',
})

const router = useRouter()

// Table headers
const headers = [
  { title: 'Name', value: 'name' },
  { title: 'Username', value: 'username' },
  { title: 'Role', value: 'role' },
  { title: 'Active Rental', value: 'activeRental', sortable: false },
  { title: 'Actions', value: 'actions', sortable: false },
]

// Navigate to pages
const goToCreate = () => {
  router.push('/dashboard/customer/create')
}

const viewCustomer = (id) => {
  router.push(`/dashboard/customers/${id}`)
}

const editCustomer = (id) => {
  router.push(`/dashboard/customer/edit/${id}`)
}

// const confirmDelete = (customer) => {
//   deletingCustomer.value = customer
//   showDeleteDialog.value = true
// }

// const deleteCustomer = async () => {
//   try {
//     const response = await fetch(`/api/customers/${deletingCustomer.value.id}`, {
//       method: 'DELETE',
//     })
//     if (!response.ok) throw new Error('Failed to delete')

//     snackbar.value = { show: true, message: 'Customer deleted.', color: 'success' }
//     showDeleteDialog.value = false
//     await fetchCustomers()
//   } catch (err) {
//     snackbar.value = { show: true, message: 'Error deleting customer.', color: 'error' }
//   }
// }

// // Fetch customers
// const fetchCustomers = async () => {
//   try {
//     const response = await fetch('/api/customers')
//     if (!response.ok) throw new Error('Failed to fetch')
//     const data = await response.json()
//     customers.value = data
//   } catch (err) {
//     snackbar.value = { show: true, message: 'Failed to load customers.', color: 'error' }
//   }
// }

// onMounted(() => {
//   fetchCustomers()
// })
</script>
