<template>
  <v-card class="pa-6" elevation="3" rounded>
    <v-card-title class="text-h5 font-weight-bold mb-4">Active Rentals</v-card-title>

    <v-data-table
      :headers="tableHeaders"
      :items="rentals"
      class="elevation-2"
      density="comfortable"
      fixed-header
      height="480"
      hover
      item-key="id"
    >
      <template #item.equipmentImage="{ item }">
        <v-avatar size="64" tile>
          <v-img
            :src="item.equipment.imageUrl || 'https://via.placeholder.com/64'"
            alt="Equipment Image"
          />
        </v-avatar>
      </template>

      <template #item.equipment.name="{ item }">
        <router-link
          :to="`equipments/${item.equipment.id}`"
          class="text-decoration-none font-weight-medium"
        >
          {{ item.equipment.name }}
        </router-link>
      </template>

      <template #item.daysRented="{ item }">
        <span class="font-weight-medium">{{ calculateDaysRented(item.issuedAt) }}</span>
        <small class="grey--text text--darken-1 ml-1">days</small>
      </template>

      <template v-if="isAdmin" #item.issuedAt="{ item }">
        {{ formatDate(item.issuedAt) }}
      </template>

      <template v-if="isAdmin" #item.dueDate="{ item }">
        {{ formatDate(item.dueDate) }}
      </template>

      <template v-if="isAdmin" #item.returnedAt="{ item }">
        {{ item.returnedAt ? formatDate(item.returnedAt) : 'Not returned' }}
      </template>

      <template #item.actions="{ item }">
        <div class="d-flex align-center ga-2">
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
                <v-icon start size="18" class="mr-1">mdi-eye</v-icon>
                Details
              </v-btn>
            </template>
          </v-tooltip>

          <v-tooltip text="Mark as returned" location="top">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                icon
                color="red darken-2"
                size="small"
                :disabled="!item.isActive"
                aria-label="Mark as returned"
                :to="`rentals/return`"
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
import { ref, onMounted, computed } from 'vue'
import { getActiveRentals } from '@/api/RentalController'
import { useAuthenticationStore } from '@/stores/Authentication'

const useAuthStore = useAuthenticationStore()

const isAdmin = ref(useAuthStore.authRole)

const tableHeaders = computed(() => {
  const baseHeaders = [
    { title: 'Equipment Name', value: 'equipment.name' },
    { title: 'Customer', value: 'customer.name' },
    { title: 'Days Rented', value: 'daysRented', sortable: false },
  ]

  const adminHeaders = [
    { title: 'Issue Date', value: 'issuedAt' },
    { title: 'Due Date', value: 'dueDate' },
    { title: 'Return Date', value: 'returnedAt' },
    { title: 'Actions', value: 'actions', sortable: false },
  ]

  return isAdmin.value ? [...baseHeaders, ...adminHeaders] : baseHeaders
})

const rentals = ref([])

onMounted(async () => {
  try {
    const res = await getActiveRentals()
    if (res) rentals.value = res
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

function formatDate(date) {
  if (!date) return 'N/A'
  return new Date(date).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })
}
</script>

<style scoped>
.font-weight-medium {
  font-weight: 500;
}
</style>
