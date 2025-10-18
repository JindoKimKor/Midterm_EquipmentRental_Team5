<template>
  <v-card class="pa-6" elevation="3" rounded>
    <v-card-title class="text-h5 font-weight-bold mb-4"> Active Rentals </v-card-title>

    <v-data-table
      :headers="headers"
      :items="rentals"
      class="elevation-2"
      density="comfortable"
      fixed-header
      height="480"
      hover
      item-key="id"
    >
      <!-- Equipment Image -->
      <template #item.equipmentImage="{ item }">
        <v-avatar size="64" tile>
          <v-img :src="item.equipment.imageUrl" alt="Equipment Image" />
        </v-avatar>
      </template>

      <!-- Days Rented -->
      <template #item.daysRented="{ item }">
        <span class="font-weight-medium">{{ calculateDaysRented(item.issuedAt) }}</span>
        <small class="grey--text text--darken-1 ml-1">days</small>
      </template>

      <!-- Actions -->
      <template #item.actions="{ item }">
        <v-btn
          color="primary"
          rounded
          small
          elevation="2"
          @click="viewRentalDetails(item)"
          aria-label="View rental details"
        >
          View Details
        </v-btn>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getActiveRentals } from '@/api/RentalController'

const headers = [
  { title: 'Image', value: 'equipmentImage', sortable: false, width: '80' },
  { title: 'Equipment Name', value: 'equipment.name' },
  { title: 'Customer', value: 'customer.fullName' },
  { title: 'Days Rented', value: 'daysRented', sortable: false, width: '120' },
  { title: 'Actions', value: 'actions', sortable: false, width: '140' },
]

const rentals = ref([])

onMounted(async () => {
  try {
    rentals.value = await getActiveRentals()
  } catch (error) {
    console.error('Failed to fetch rentals:', error)
  }
})

function calculateDaysRented(issuedAt) {
  const issuedDate = new Date(issuedAt)
  const now = new Date()
  const diffTime = now - issuedDate
  return Math.floor(diffTime / (1000 * 60 * 60 * 24))
}

function viewRentalDetails(rental) {
  // Implement your logic here or navigate to rental details page
  console.log('Viewing rental details:', rental)
}
</script>

<style scoped>
.font-weight-medium {
  font-weight: 500;
}
</style>
