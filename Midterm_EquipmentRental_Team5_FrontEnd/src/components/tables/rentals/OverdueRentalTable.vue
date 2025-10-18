<template>
  <v-card class="pa-4">
    <v-card-title class="text-h6">Rental History</v-card-title>

    <v-timeline side="end" align="start" density="comfortable">
      <v-timeline-item
        v-for="rental in rentals"
        :key="rental.id"
        :color="getColor(rental)"
        size="small"
      >
        <template #opposite>
          <div class="text-caption text-grey-darken-1">
            {{ formatDate(rental.issuedAt) }} →
            {{ rental.returnedAt ? formatDate(rental.returnedAt) : 'Ongoing' }}
          </div>
        </template>

        <template #default>
          <div class="font-weight-medium">Rented by {{ rental.customer.fullName }}</div>
          <div class="text-caption">Duration: {{ getDuration(rental) }} days</div>
          <v-chip class="mt-2" :color="getColor(rental)" text-color="white" small label>
            {{ getStatus(rental) }}
          </v-chip>
        </template>
      </v-timeline-item>
    </v-timeline>
  </v-card>
</template>

<script setup>
import { getCompletedRentals } from '@/api/RentalController'
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'

const rentals = ref([])
const route = useRoute()

onMounted(async () => {
  try {
    const response = await getCompletedRentals()
    rentals.value = response
  } catch (err) {
    console.error('Error fetching rental history:', err)
  }
})

// Helpers
function formatDate(date) {
  return new Date(date).toLocaleDateString()
}

function getDuration(rental) {
  const start = new Date(rental.issuedAt)
  const end = rental.returnedAt ? new Date(rental.returnedAt) : new Date()
  const ms = end - start
  return Math.max(Math.floor(ms / (1000 * 60 * 60 * 24)), 0)
}

function getStatus(rental) {
  if (rental.isActive) {
    const now = new Date()
    const due = new Date(rental.dueDate)
    return now > due ? 'Overdue' : 'Active'
  }
  return 'Completed'
}

function getColor(rental) {
  const status = getStatus(rental)
  return status === 'Completed' ? 'green' : status === 'Active' ? 'blue' : 'red'
}
</script>
