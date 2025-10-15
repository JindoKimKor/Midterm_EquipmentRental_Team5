<template>
  <v-container>
    <!-- Profile Card -->
    <v-card class="mb-4 pa-4">
      <v-card-title>
        <div class="text-h6">Customer Profile</div>
        <v-spacer />
        <v-chip :color="roleColor(customer.role)" class="text-uppercase" dark>
          {{ customer.role }}
        </v-chip>
      </v-card-title>

      <v-card-text>
        <p><strong>Name:</strong> {{ customer.name }}</p>
        <p><strong>Username:</strong> {{ customer.username }}</p>
      </v-card-text>
    </v-card>

    <!-- Rental History -->
    <v-card>
      <v-card-title>Rental History</v-card-title>
      <v-data-table :headers="rentalHeaders" :items="rentalHistory" class="elevation-1">
        <template #item.status="{ item }">
          <v-chip :color="statusColor(item.status)" dark>{{ item.status }}</v-chip>
        </template>
      </v-data-table>
    </v-card>
  </v-container>
</template>

<script setup>
import { onBeforeMount, onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const customerId = route.params.id

// Simulated role check
const isAdmin = true // ⬅️ Replace with real auth logic

// Data
const customer = ref({})
const rentalHistory = ref([])

// Table headers for rental history
const rentalHeaders = [
  { title: 'Equipment', key: 'equipment' },
  { title: 'Issue Date', key: 'issueDate' },
  { title: 'Return Date', key: 'returnDate' },
  { title: 'Status', key: 'status' },
]

// Load data (replace with real API)
onBeforeMount(async () => {
  // Fetch customer info
  customer.value = {
    id: customerId,
    name: 'Alex Johnson',
    username: 'alexj',
    role: isAdmin ? 'Admin' : 'User',
  }

  // Fetch rental history
  rentalHistory.value = [
    {
      equipment: 'MacBook Pro',
      issueDate: '2025-08-01',
      returnDate: '2025-08-15',
      status: 'Returned',
    },
    {
      equipment: 'DSLR Camera',
      issueDate: '2025-09-10',
      returnDate: null,
      status: 'Active',
    },
  ]
})

// Chip colors
const roleColor = (role) => (role.toLowerCase() === 'admin' ? 'blue' : 'green')

const statusColor = (status) => {
  switch (status.toLowerCase()) {
    case 'active':
      return 'blue'
    case 'returned':
      return 'green'
    case 'overdue':
      return 'red'
    default:
      return 'grey'
  }
}
</script>
