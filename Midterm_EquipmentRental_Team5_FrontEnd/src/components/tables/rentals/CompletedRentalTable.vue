<template>
  <v-card class="pa-6" elevation="3" rounded>
    <v-card-title class="text-h5 font-weight-bold mb-4"> Completed Rentals </v-card-title>

    <v-data-table
      :headers="headers"
      :items="completedRentals"
      class="elevation-2"
      density="comfortable"
      fixed-header
      height="480"
      hover
      item-key="id"
    >
      <!-- Return Date -->
      <template #item.returnedAt="{ item }">
        <span>{{ formatDate(item.returnedAt) }}</span>
      </template>

      <!-- Duration -->
      <template #item.duration="{ item }">
        <span class="font-weight-medium">
          {{ calculateDuration(item.issuedAt, item.returnedAt) }}
        </span>
        <small class="grey--text text--darken-1 ml-1">days</small>
      </template>

      <!-- Status -->
      <template #item.status="{ item }">
        <v-chip
          :color="getStatusColor(item)"
          text-color="white"
          small
          label
          class="text-capitalize"
        >
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
  { title: 'Customer', value: 'customer.name' },
  { title: 'Return Date', value: 'returnedAt' },
  { title: 'Duration', value: 'duration', sortable: false, width: '110' },
  { title: 'Status', value: 'status', sortable: false, width: '100' },
]

const completedRentals = ref([])

onMounted(async () => {
  try {
    completedRentals.value = await getCompletedRentals()
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

<style scoped>
.font-weight-medium {
  font-weight: 500;
}
.text-capitalize {
  text-transform: capitalize;
}
</style>
