<template>
  <v-card class="pa-4">
    <v-card-title class="text-h6">Completed Rentals</v-card-title>

    <v-data-table
      :headers="headers"
      :items="completedRentals"
      class="elevation-1"
      density="comfortable"
    >
      <!-- Return Date -->
      <template #item.returnedAt="{ item }">
        {{ formatDate(item.returnedAt) }}
      </template>

      <!-- Duration -->
      <template #item.duration="{ item }">
        {{ calculateDuration(item.issuedAt, item.returnedAt) }} days
      </template>

      <!-- Status Badge -->
      <template #item.status="{ item }">
        <v-chip :color="getStatusColor(item)" text-color="white" small label>
          {{ getStatus(item) }}
        </v-chip>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

// Table headers
const headers = [
  { title: 'Equipment', value: 'equipment.name' },
  { title: 'Customer', value: 'customer.fullName' },
  { title: 'Return Date', value: 'returnedAt' },
  { title: 'Duration', value: 'duration' },
  { title: 'Status', value: 'status', sortable: false },
]

// Data
const completedRentals = ref([])

// Fetch completed rentals
onMounted(async () => {
  try {
    const response = await axios.get('/api/rentals/completed')
    completedRentals.value = response.data
  } catch (error) {
    console.error('Error fetching completed rentals', error)
  }
})

// Format return date
function formatDate(date) {
  return new Date(date).toLocaleDateString()
}

// Calculate rental duration in days
function calculateDuration(issuedAt, returnedAt) {
  const start = new Date(issuedAt)
  const end = new Date(returnedAt)
  const diffTime = end - start
  return Math.floor(diffTime / (1000 * 60 * 60 * 24))
}

// Determine status
function getStatus(rental) {
  const due = new Date(rental.dueDate)
  const returned = new Date(rental.returnedAt)
  return returned <= due ? 'On Time' : 'Late'
}

// Status chip color
function getStatusColor(rental) {
  return getStatus(rental) === 'On Time' ? 'green' : 'red'
}
</script>
