<template>
  <v-container>
    <v-card class="pa-4">
      <!-- Title + Status -->
      <v-card-title>
        <div class="text-h6 font-weight-bold">Rental Details</div>
        <v-spacer />
        <v-chip :color="statusColor(rentalStatus)" class="text-uppercase" dark>
          {{ rentalStatus }}
        </v-chip>
      </v-card-title>

      <v-divider class="my-2" />

      <!-- Rental Content -->
      <v-card-text>
        <!-- Equipment and Customer Info -->
        <v-row>
          <!-- Equipment Info -->
          <v-col cols="12" md="6">
            <h4 class="mb-2">🔧 Equipment</h4>
            <p><strong>Name:</strong> {{ rental.equipment?.name }}</p>
            <p><strong>ID:</strong> {{ rental.equipment?.id }}</p>
            <p><strong>Category:</strong> {{ rental.equipment?.category }}</p>
            <p><strong>Condition:</strong> {{ rental.equipment?.condition }}</p>
            <p>
              <strong>Rental Price:</strong> {{ formatCurrency(rental.equipment?.rentalPrice) }}
            </p>
          </v-col>

          <!-- Customer Info -->
          <v-col cols="12" md="6">
            <h4 class="mb-2">👤 Customer</h4>
            <p><strong>Name:</strong> {{ rental.customer?.name }}</p>
            <p><strong>Email:</strong> {{ rental.customer?.email }}</p>
            <p><strong>Username:</strong> {{ rental.customer?.userName }}</p>
            <p><strong>Role:</strong> {{ rental.customer?.role }}</p>
          </v-col>
        </v-row>

        <v-divider class="my-4" />

        <!-- Dates -->
        <v-row>
          <v-col cols="12" md="4">
            <h4>📅 Issued At</h4>
            <p>{{ formatDate(rental.issuedAt) }}</p>
          </v-col>
          <v-col cols="12" md="4">
            <h4>📆 Due Date</h4>
            <p>{{ formatDate(rental.dueDate) }}</p>
          </v-col>
          <v-col cols="12" md="4">
            <h4>✅ Returned At</h4>
            <p>
              {{ rental.returnedAt ? formatDate(rental.returnedAt) : 'Not yet returned' }}
            </p>
          </v-col>
        </v-row>

        <!-- Extra Info -->
        <v-row class="mt-4">
          <v-col cols="12" md="6">
            <h4>💸 Overdue Fee</h4>
            <p>{{ rental.overdueFee ? formatCurrency(rental.overdueFee) : '$0.00' }}</p>
          </v-col>
          <v-col cols="12" md="6">
            <h4>📝 Extension Reason</h4>
            <p>{{ rental.extensionReason || '—' }}</p>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref, onBeforeMount, computed } from 'vue'
import { useRoute } from 'vue-router'
import { getRentalDetails } from '@/api/RentalController'

const route = useRoute()
const rentalId = route.params.id

const rental = ref({})

// Load rental details
onBeforeMount(async () => {
  const res = await getRentalDetails(rentalId)
  rental.value = res
})

// Rental status based on returnedAt and dueDate
const rentalStatus = computed(() => {
  if (!rental.value.isActive) return 'Returned'

  const now = new Date()
  const dueDate = new Date(rental.value.dueDate)

  if (now > dueDate && !rental.value.returnedAt) {
    return 'Overdue'
  }

  return 'Active'
})

// Format status chip color
const statusColor = (status) => {
  switch (status.toLowerCase()) {
    case 'active':
      return 'blue darken-2'
    case 'returned':
      return 'green'
    case 'overdue':
      return 'red'
    default:
      return 'grey'
  }
}

const formatDate = (date) => (date ? new Date(date).toLocaleString() : 'N/A')

const formatCurrency = (value) =>
  new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  }).format(value || 0)
</script>

<style scoped>
h4 {
  font-weight: 600;
}
</style>
