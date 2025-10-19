<template>
  <v-card class="pa-6" elevation="3" rounded>
    <v-card-title class="d-flex justify-space-between align-center">
      <span class="text-h5 font-weight-semibold">Rented Equipment</span>
    </v-card-title>

    <v-data-table
      :headers="headers"
      :items="equipment"
      :items-per-page="10"
      class="elevation-1"
      density="comfortable"
      hover
      item-value="id"
      fixed-header
      height="600"
    >
      <!-- Name column with router-link -->
      <template #item.name="{ item }">
        <router-link :to="`equipments/${item.id}`" class="text-decoration-none font-weight-medium">
          {{ item.name }}
        </router-link>
      </template>

      <!-- Category Chip -->
      <template #item.category="{ item }">
        <v-chip color="blue lighten-4" text-color="blue darken-3" label small class="ma-0">
          {{ item.category }}
        </v-chip>
      </template>

      <!-- Condition Chip -->
      <template #item.condition="{ item }">
        <v-chip color="grey lighten-3" text-color="black" label small class="ma-0">
          {{ item.condition }}
        </v-chip>
      </template>

      <!-- Rental Price -->
      <template #item.rentalPrice="{ item }">
        <span class="font-mono font-weight-medium">
          {{ formatCurrency(item.rentalPrice) }}
        </span>
      </template>

      <template #item.isAvailable="{ item }">
        <v-chip
          :color="item.isAvailable ? 'green lighten-4' : 'red lighten-4'"
          :text-color="item.isAvailable ? 'green darken-2' : 'red darken-2'"
          label
          small
          class="ma-0"
        >
          {{ item.isAvailable ? 'Available' : 'Unavailable' }}
        </v-chip>
      </template>

      <template #item.createdAt="{ item }">
        {{ formatDate(item.createdAt) }}
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onBeforeMount } from 'vue'
import { getRentedEquipmentSummary } from '@/api/EquipmentController'

const headers = [
  { title: 'Equipment Name', value: 'name' },
  { title: 'Category', value: 'category' },
  { title: 'Condition', value: 'condition' },
  { title: 'Rental Price ($)', value: 'rentalPrice' },
  { title: 'Availability', value: 'isAvailable' },
  { title: 'Created Date', value: 'createdAt' },
]

const equipment = ref([])

const loadEquipment = async () => {
  try {
    const response = await getRentedEquipmentSummary()
    equipment.value = response || []
  } catch (error) {
    console.error('Failed to load equipment:', error)
  }
}

onBeforeMount(loadEquipment)

function formatCurrency(value) {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  }).format(value)
}

function formatDate(date) {
  return new Date(date).toLocaleDateString()
}

function viewEquipment(item) {
  console.log('View equipment:', item)
}

function editEquipment(item) {
  console.log('Edit equipment:', item)
}

function refreshData() {
  loadEquipment()
}
</script>

<style scoped>
.text-decoration-none {
  text-decoration: none;
}
.font-weight-medium {
  font-weight: 500;
}
</style>
