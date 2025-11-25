<template>
  <v-card class="pa-6" elevation="3" rounded>
    <v-card-title class="d-flex justify-space-between align-center">
      <b class="text-h5 font-weight-bold">Rental</b>
    </v-card-title>

    <v-data-table
      :headers="headers"
      :items="rentals"
      :items-per-page="10"
      class="elevation-1"
      density="comfortable"
      hover
      item-value="id"
      fixed-header
      height="600"
    >
      <!-- Equipment Name -->
      <template #item.equipmentName="{ item }">
        <router-link
          :to="`/equipments/${item.equipmentId}`"
          class="text-decoration-none font-weight-medium"
        >
          {{ item.equipmentName }}
        </router-link>
      </template>

      <!-- Category -->
      <template #item.equipmentCategory="{ item }">
        <v-chip color="blue-lighten-4" text-color="blue-darken-3" label small class="ma-0">
          {{ item.equipmentCategory ?? '—' }}
        </v-chip>
      </template>

      <!-- Customer Name -->
      <template #item.customerName="{ item }">
        {{ item.customerName }}
      </template>

      <!-- Issued At -->
      <template #item.issuedAt="{ item }">
        {{ formatDate(item.issuedAt) }}
      </template>

      <!-- Due Date -->
      <template #item.dueDate="{ item }">
        {{ formatDate(item.dueDate) }}
      </template>

      <!-- Returned At -->
      <template #item.returnedAt="{ item }">
        {{ item.returnedAt ? formatDate(item.returnedAt) : 'Not Returned' }}
      </template>

      <!-- Active Status -->
      <template #item.isActive="{ item }">
        <v-chip
          :color="item.isActive ? 'green-lighten-4' : 'red-lighten-4'"
          :text-color="item.isActive ? 'green-darken-2' : 'red-darken-2'"
          label
          small
        >
          {{ item.isActive ? 'Active' : 'Completed' }}
        </v-chip>
      </template>

      <!-- Overdue Fee -->
      <template #item.overdueFee="{ item }">
        <span class="font-mono font-weight-medium">
          {{ item.overdueFee ? formatCurrency(item.overdueFee) : '$0.00' }}
        </span>
      </template>

      <!-- Extension Reason -->
      <template #item.extensionReason="{ item }">
        <span>{{ item.extensionReason || '—' }}</span>
      </template>

      <!-- Actions -->
      <template #item.actions="{ item }">
        <div class="d-flex align-center ga-2">
          <!-- Details Button -->
          <v-tooltip text="View rental details" location="top">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                :to="`rentals/${item.id}`"
                color="primary"
                variant="flat"
                size="small"
                class="text-capitalize"
                aria-label="View rental details"
              >
                <v-icon start size="18">mdi-eye</v-icon>
                Details
              </v-btn>
            </template>
          </v-tooltip>

          <!-- Mark as Returned -->
          <v-tooltip text="Cancel Rental" location="top">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                icon
                color="red-darken-2"
                size="small"
                :disabled="!item.isActive"
                @click="markReturned(item.id)"
              >
                <v-icon>mdi-logout</v-icon>
              </v-btn>
            </template>
          </v-tooltip>
        </div>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onBeforeMount } from 'vue'
import { getAllRentals, cancelRental } from '@/api/RentalController'

const headers = [
  { title: 'Equipment', value: 'equipmentName' },
  { title: 'Customer', value: 'customerName' },
  { title: 'Issued At', value: 'issuedAt' },
  { title: 'Due Date', value: 'dueDate' },
  { title: 'Returned At', value: 'returnedAt' },
  { title: 'Status', value: 'isActive' },
  { title: 'Overdue Fee', value: 'overdueFee' },
  { title: 'Extension Reason', value: 'extensionReason' },
  { title: 'Actions', value: 'actions', sortable: false },
]

const rentals = ref([])

const loadRentals = async () => {
  try {
    const response = await getAllRentals()
    console.log(response)
    rentals.value = response || []
  } catch (error) {
    console.error('Failed to load rentals:', error)
  }
}

onBeforeMount(loadRentals)

/* ---------------- Utility Functions ---------------- */

function formatCurrency(value) {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  }).format(value)
}

function formatDate(date) {
  if (!date) return '—'
  return new Date(date).toLocaleDateString()
}

/* --------------- Mark As Returned --------------- */

async function markReturned(id) {
  try {
    await cancelRental(id)
    window.location.reload() // 🔥 Full page refresh
    await loadRentals()
  } catch (err) {
    console.error('Failed to mark returned:', err)
  }
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
