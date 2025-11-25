<template>
  <v-container>
    <v-row justify="center">
      <v-col cols="12" md="8">
        <v-card class="pa-4" elevation="3">
          <v-card-title class="text-h5 font-weight-bold">Your Rental</v-card-title>

          <v-divider class="my-2" />

          <v-card-text>
            <div v-if="loading">
              <v-alert type="info" variant="tonal"> Loading your rental... </v-alert>
            </div>

            <div v-else-if="!rental">
              <v-alert type="warning" variant="tonal"> You have no active rental. </v-alert>
            </div>

            <div v-else>
              <h3 class="text-h6 font-weight-medium mb-3">{{ rental.equipmentName }}</h3>
              <p><strong>🛠 Category:</strong> {{ rental.equipmentCategory }}</p>
              <p><strong>💵 Rental Price:</strong> {{ formatCurrency(rental.rentalPrice) }}</p>

              <v-row>
                <v-col cols="12" md="6">
                  <p><strong>📅 Issued At:</strong> {{ formatDate(rental.issuedAt) }}</p>
                  <p><strong>📆 Due Date:</strong> {{ formatDate(rental.dueDate) }}</p>
                </v-col>

                <v-col cols="12" md="6">
                  <p>
                    <strong>✅ Returned At:</strong>
                    {{ rental.returnedAt ? formatDate(rental.returnedAt) : 'Not returned yet' }}
                  </p>
                  <p>
                    <strong>Status:</strong>
                    <v-chip
                      :color="rental.isActive ? 'green lighten-4' : 'grey lighten-2'"
                      :text-color="rental.isActive ? 'green darken-2' : 'grey darken-2'"
                      label
                      small
                    >
                      {{ rental.isActive ? 'Active' : 'Completed' }}
                    </v-chip>
                  </p>
                </v-col>
              </v-row>

              <v-row v-if="rental.overdueFee || rental.extensionReason" class="mt-4">
                <v-col cols="12" md="6" v-if="rental.overdueFee !== null">
                  <p>
                    <strong>💸 Overdue Fee:</strong>
                    {{ formatCurrency(rental.overdueFee) }}
                  </p>
                </v-col>
                <v-col cols="12" md="6" v-if="rental.extensionReason">
                  <p><strong>📝 Extension Reason:</strong> {{ rental.extensionReason }}</p>
                </v-col>
              </v-row>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getCustomerActiveRental } from '@/api/CustomerController'
import { useAuthenticationStore } from '@/stores/Authentication'

const emit = defineEmits(['rental-loaded'])

const rental = ref(null)
const loading = ref(true)
const authStore = useAuthenticationStore()

function formatDate(date) {
  if (!date) return '—'
  return new Date(date).toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })
}

function formatCurrency(value) {
  if (value === null || value === undefined) return '$0.00'
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  }).format(value)
}

async function fetchRental() {
  try {
    const response = await getCustomerActiveRental(authStore.authUserId)
    // Handle API returning an object directly or nested in data
    rental.value = response[0] || response

    if (rental.value) emit('rental-loaded', rental.value)
  } catch (error) {
    console.error('Failed to load rental:', error)
  } finally {
    loading.value = false
  }
}

onMounted(fetchRental)
</script>

<style scoped>
.rental-list {
  max-width: 600px;
  margin: auto;
}
</style>
