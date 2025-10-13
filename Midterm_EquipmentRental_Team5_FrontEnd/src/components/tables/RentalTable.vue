<template>
  <v-card>
    <v-card-title> Rental Records </v-card-title>

    <v-data-table
      :headers="headers"
      :items="filteredRentals"
      :items-per-page="10"
      class="elevation-1"
    >
      <template #item.issueDate="{ item }">
        {{ formatDate(item.issueDate) }}
      </template>

      <template #item.status="{ item }">
        <v-chip :color="getStatusColor(item.status)" text-color="white" small>
          {{ item.status }}
        </v-chip>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, computed } from 'vue'

// Simulated role check and user
const isAdmin = true // Replace with actual check
const userId = 2 // Logged-in user's ID

// Table headers
const headers = [
  { title: 'Equipment', value: 'equipment' },
  { title: 'Customer', value: 'customer' },
  { title: 'Issue Date', value: 'issueDate' },
  { title: 'Status', value: 'status' },
]

// Simulated rental data (replace with API fetch)
const rentals = ref([
  {
    id: 1,
    equipment: 'Canon DSLR',
    customer: 'Alice Johnson',
    customerId: 1,
    issueDate: '2025-10-01',
    status: 'Active',
  },
  {
    id: 2,
    equipment: 'Dell XPS 15',
    customer: 'Bob Smith',
    customerId: 2,
    issueDate: '2025-09-20',
    status: 'Completed',
  },
])

// Role-based filter
const filteredRentals = computed(() => {
  return isAdmin ? rentals.value : rentals.value.filter((r) => r.customerId === userId)
})

// Format date
const formatDate = (dateStr) => {
  return new Date(dateStr).toLocaleDateString()
}

// Status color chip
const getStatusColor = (status) => {
  switch (status.toLowerCase()) {
    case 'active':
      return 'blue'
    case 'completed':
      return 'green'
    case 'overdue':
      return 'red'
    default:
      return 'grey'
  }
}
</script>
