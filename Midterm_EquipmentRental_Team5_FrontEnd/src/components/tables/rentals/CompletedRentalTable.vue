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

      <!-- Status -->
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
import { getCompletedRentals } from '@/api/RentalController'

const headers = [
  { title: 'Equipment', value: 'equipment.name' },
  { title: 'Customer', value: 'customer.fullName' },
  { title: 'Return Date', value: 'returnedAt' },
  { title: 'Duration', value: 'duration' },
  { title: 'Status', value: 'status', sortable: false },
]

const completedRentals = ref([])

onMounted(async () => {
  try {
    const response = await getCompletedRentals()
    completedRentals.value = response
  } catch (error) {
    console.error('Failed to fetch completed rentals:', error)
  }
})

function formatDate(date) {
  return new Date(date).toLocaleDateString()
}

function calculateDuration(issuedAt, returnedAt) {
  const start = new Date(issuedAt)
  const end = new Date(returnedAt)
  const ms = end - start
  return Math.floor(ms / (1000 * 60 * 60 * 24))
}

function getStatus(rental) {
  const due = new Date(rental.dueDate)
  const returned = new Date(rental.returnedAt)
  return returned <= due ? 'On Time' : 'Late'
}

function getStatusColor(rental) {
  return getStatus(rental) === 'On Time' ? 'green' : 'red'
}
</script>
