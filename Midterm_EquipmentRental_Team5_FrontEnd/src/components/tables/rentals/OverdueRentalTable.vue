<template>
  <v-card class="pa-4 red-alert" border flat>
    <v-card-title class="text-h6 text-red-darken-2"> ⚠️ Overdue Rentals </v-card-title>

    <v-data-table
      :headers="headers"
      :items="overdueRentals"
      class="elevation-1"
      density="comfortable"
    >
      <template #item.dueDate="{ item }">
        {{ formatDate(item.dueDate) }}
      </template>

      <template #item.daysOverdue="{ item }">
        <v-chip color="red-darken-2" text-color="white" small label>
          {{ calculateDaysOverdue(item.dueDate) }} days
        </v-chip>
      </template>

      <template #item.actions="{ item }">
        <v-tooltip text="Mark as returned">
          <template #activator="{ props }">
            <v-btn
              size="small"
              color="red-darken-2"
              icon
              v-bind="props"
              @click="handleReturnNow(item)"
            >
              <v-icon>mdi-logout</v-icon>
            </v-btn>
          </template>
        </v-tooltip>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getOverdueRentals } from '@/api/RentalController'

const headers = [
  { title: 'Equipment', value: 'equipment.name' },
  { title: 'Customer', value: 'customer.fullName' },
  { title: 'Due Date', value: 'dueDate' },
  { title: 'Days Overdue', value: 'daysOverdue' },
  { title: 'Actions', value: 'actions', sortable: false },
]

const overdueRentals = ref([])

onMounted(async () => {
  try {
    const response = await getOverdueRentals()
    overdueRentals.value = response
  } catch (error) {
    console.error('Failed to fetch overdue rentals:', error)
  }
})

function formatDate(date) {
  return new Date(date).toLocaleDateString()
}

function calculateDaysOverdue(dueDate) {
  const due = new Date(dueDate)
  const now = new Date()
  const diff = now - due
  return Math.max(Math.floor(diff / (1000 * 60 * 60 * 24)), 0)
}

function handleReturnNow(item) {
  console.log('Returning overdue rental:', item)
  // You can trigger a modal or API call here
}
</script>

<style scoped>
.red-alert {
  background-color: #ffebee !important;
}
</style>
