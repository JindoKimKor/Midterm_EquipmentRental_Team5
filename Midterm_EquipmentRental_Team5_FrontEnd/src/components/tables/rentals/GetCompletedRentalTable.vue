<template>
  <v-card class="pa-4" color="red-lighten-5" border flat>
    <v-card-title class="text-h6 text-red-darken-2"> ⚠️ Overdue Rentals </v-card-title>

    <v-data-table
      :headers="headers"
      :items="overdueRentals"
      class="elevation-1 text-red-darken-3"
      density="comfortable"
    >
      <!-- Due Date -->
      <template #item.dueDate="{ item }">
        {{ formatDate(item.dueDate) }}
      </template>

      <!-- Days Overdue -->
      <template #item.daysOverdue="{ item }">
        {{ calculateDaysOverdue(item.dueDate) }} days
      </template>

      <!-- Optional Action -->
      <template #item.actions="{ item }">
        <v-btn size="small" color="red-darken-2" variant="tonal" @click="handleReturnNow(item)">
          Return Now
        </v-btn>
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
  { title: 'Due Date', value: 'dueDate' },
  { title: 'Days Overdue', value: 'daysOverdue' },
  { title: 'Actions', value: 'actions', sortable: false },
]

// Data
const overdueRentals = ref([])

onMounted(async () => {
  try {
    const response = await axios.get('/api/rentals/overdue')
    overdueRentals.value = response.data
  } catch (error) {
    console.error('Failed to fetch overdue rentals:', error)
  }
})

// Format date
function formatDate(date) {
  return new Date(date).toLocaleDateString()
}

// Calculate how many days it's overdue
function calculateDaysOverdue(dueDate) {
  const due = new Date(dueDate)
  const now = new Date()
  const diff = now - due
  return Math.max(Math.floor(diff / (1000 * 60 * 60 * 24)), 0)
}

// Optional: handle return now
function handleReturnNow(item) {
  console.log('Returning rental:', item)
  // You can open a return form or send an API request
}
</script>
