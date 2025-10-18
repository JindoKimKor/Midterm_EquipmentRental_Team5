<template>
  <v-card class="pa-4">
    <v-card-title class="text-h6">Active Rentals</v-card-title>

    <v-data-table :headers="headers" :items="rentals" class="elevation-1" density="comfortable">
      <template #item.equipmentImage="{ item }">
        <v-avatar size="60">
          <v-img :src="item.equipment.imageUrl" alt="Equipment Image" />
        </v-avatar>
      </template>
      <template #item.daysRented="{ item }">
        {{ calculateDaysRented(item.issuedAt) }} days
      </template>
      <template #item.actions="{ item }">
        <v-btn color="primary" size="small" @click="viewRentalDetails(item)"> View Details </v-btn>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getActiveRentals } from '@/api/RentalController'

const headers = [
  { title: 'Image', value: 'equipmentImage', sortable: false },
  { title: 'Equipment Name', value: 'equipment.name' },
  { title: 'Customer', value: 'customer.fullName' },
  { title: 'Days Rented', value: 'daysRented', sortable: false },
  { title: 'Actions', value: 'actions', sortable: false },
]

const rentals = ref([])

onMounted(async () => {
  try {
    const response = await getActiveRentals()
    rentals.value = response
  } catch (error) {
    console.error('Failed to fetch rentals:', error)
  }
})

function calculateDaysRented(issuedAt) {
  const issuedDate = new Date(issuedAt)
  const now = new Date()
  const diffTime = Math.abs(now - issuedDate)
  return Math.floor(diffTime / (1000 * 60 * 60 * 24))
}

function viewRentalDetails(rental) {
  console.log('View rental:', rental)
}
</script>
