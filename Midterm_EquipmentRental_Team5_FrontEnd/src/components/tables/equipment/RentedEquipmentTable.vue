<template>
  <v-card class="pa-4">
    <v-card-title class="d-flex justify-space-between align-center">
      <span class="text-h6">Rented equipment</span>
    </v-card-title>

    <v-data-table
      :headers="headers"
      :items="equipment"
      :items-per-page="10"
      class="elevation-1"
      density="comfortable"
      hover
    >
      <!-- Name column with router-link -->
      <template #item.name="{ item }">
        <router-link :to="`/equipments/${item.id}`" class="text-decoration-none font-weight-medium">
          {{ item.name }}
        </router-link>
      </template>

      <!-- Category Chip -->
      <template #item.category="{ item }">
        <v-chip color="blue-lighten-4" text-color="blue-darken-3" label>
          {{ item.category }}
        </v-chip>
      </template>

      <!-- Condition Chip -->
      <template #item.condition="{ item }">
        <v-chip color="grey-lighten-3" text-color="black" label>
          {{ item.condition }}
        </v-chip>
      </template>

      <!-- Rental Price -->
      <template #item.rentalPrice="{ item }">
        {{ formatCurrency(item.rentalPrice) }}
      </template>

      <!-- Availability -->
      <template #item.isAvailable="{ item }">
        <v-chip :color="item.isAvailable ? 'green' : 'red'" variant="flat" label small>
          {{ item.isAvailable ? 'Available' : 'Unavailable' }}
        </v-chip>
      </template>

      <!-- Created Date -->
      <template #item.createdAt="{ item }">
        {{ formatDate(item.createdAt) }}
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { getRentedEquipmentSummary } from '@/api/EquipmentController'
import { onBeforeMount, ref } from 'vue'

// ✅ Data headers
const headers = [
  { title: 'Equipment Name', value: 'name' },
  { title: 'Category', value: 'category' },
  { title: 'Condition', value: 'condition' },
  { title: 'Rental Price ($)', value: 'rentalPrice' },
  { title: 'Availability', value: 'isAvailable' },
  { title: 'Created Date', value: 'createdAt' },
  { title: 'Actions', value: 'actions', sortable: false },
]

const equipment = ref([])

onBeforeMount(async () => {
  const response = await getRentedEquipmentSummary()
  equipment.value = response.equipment || []
})

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
  console.log('Viewing equipment:', item)
}

function editEquipment(item) {
  console.log('Editing equipment:', item)
}
</script>
