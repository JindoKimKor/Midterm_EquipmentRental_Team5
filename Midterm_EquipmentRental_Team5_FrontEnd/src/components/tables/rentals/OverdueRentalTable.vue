<template>
  <v-card class="pa-6 red-alert" elevation="3" rounded>
    <v-card-title class="d-flex align-center text-h5 font-weight-bold text-red-darken-3 mb-4">
      <v-icon left color="red darken-3" class="mr-2" size="32">mdi-alert-circle</v-icon>
      Overdue Rentals
    </v-card-title>

    <v-data-table
      :headers="headers"
      :items="overdueRentals"
      class="elevation-2"
      density="comfortable"
      fixed-header
      height="480"
      hover
      item-key="id"
      :loading="loading"
      loading-text="Loading overdue rentals..."
      no-data-text="No overdue rentals found"
    >
      <!-- Due Date -->
      <template #item.dueDate="{ item }">
        <span>{{ formatDate(item.dueDate) }}</span>
      </template>

      <template #item.daysOverdue="{ item }">
        <v-chip
          color="red darken-3"
          text-color="white"
          small
          label
          class="font-weight-semibold"
          aria-label="Days overdue"
        >
          {{ calculateDaysOverdue(item.dueDate) }} day<span
            v-if="calculateDaysOverdue(item.dueDate) !== 1"
            >s</span
          >
        </v-chip>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getOverdueRentals, returnEquipment } from '@/api/RentalController'

const headers = [
  { title: 'Equipment', value: 'equipment.name' },
  { title: 'Customer', value: 'customer.name' },
  { title: 'Due Date', value: 'dueDate' },
  { title: 'Days Overdue', value: 'daysOverdue' },
]

const overdueRentals = ref([])
const loading = ref(false)
const returningId = ref(null)

onMounted(fetchOverdueRentals)

async function fetchOverdueRentals() {
  loading.value = true
  try {
    overdueRentals.value = await getOverdueRentals()
  } catch (error) {
    console.error('Failed to fetch overdue rentals:', error)
  } finally {
    loading.value = false
  }
}

function formatDate(date) {
  return new Date(date).toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })
}

function calculateDaysOverdue(dueDate) {
  const due = new Date(dueDate)
  const now = new Date()
  const diff = now - due
  return Math.max(Math.floor(diff / (1000 * 60 * 60 * 24)), 0)
}

async function handleReturnNow(item) {
  if (returningId.value) return // prevent multiple clicks

  if (confirm(`Mark rental of "${item.equipment.name}" as returned?`)) {
    returningId.value = item.id
    try {
      await returnEquipment(item.id)
      overdueRentals.value = overdueRentals.value.filter((rental) => rental.id !== item.id)
      alert('Rental marked as returned.')
    } catch (error) {
      alert('Failed to mark as returned. Please try again.')
      console.error(error)
    } finally {
      returningId.value = null
    }
  }
}
</script>

<style scoped>
.red-alert {
  background-color: #ffebee !important;
  border: 1px solid #f44336;
}

.font-weight-semibold {
  font-weight: 600;
}
</style>
