<template>
  <v-card class="pa-4">
    <v-card-title class="text-h6">Active Rentals</v-card-title>

    <v-data-table :headers="headers" :items="rentals" class="elevation-1" density="comfortable">
      <!-- Equipment Image -->
      <template #item.equipmentImage="{ item }">
        <v-avatar size="60">
          <v-img :src="item.equipment.imageUrl" alt="Equipment Image" />
        </v-avatar>
      </template>

      <!-- Days Rented -->
      <template #item.daysRented="{ item }">
        {{ calculateDaysRented(item.issuedAt) }} days
      </template>

      <!-- View Details Button -->
      <template #item.actions="{ item }">
        <v-btn color="primary" size="small" @click="viewRentalDetails(item)"> View Details </v-btn>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

// Table headers
const headers = [
  { title: 'Image', value: 'equipmentImage', sortable: false },
  { title: 'Equipment Name', value: 'equipment.name' },
  { title: 'Customer', value: 'customer.fullName' },
  { title: 'Days Rented', value: 'daysRented', sortable: false },
  { title: 'Actions', value: 'actions', sortable: false },
]

// Data
const rentals = ref([])

// Fetch active rentals on mount
onMounted(async () => {
  try {
    const response = await axios.get('/api/rentals/active')
    rentals.value = response.data
  } catch (error) {
    console.error('Failed to fetch rentals:', error)
  }
})

// Compute days since IssuedAt
function calculateDaysRented(issuedAt) {
  const issuedDate = new Date(issuedAt)
  const now = new Date()
  const diffTime = Math.abs(now - issuedDate)
  return Math.floor(diffTime / (1000 * 60 * 60 * 24))
}

// Handle "View Details"
function viewRentalDetails(rental) {
  // This could navigate or open a modal
  console.log('View rental:', rental)
}
</script>
