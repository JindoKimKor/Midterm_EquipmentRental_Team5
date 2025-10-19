<template>
  <div class="rental-list">
    <h2>Your Rental</h2>

    <div v-if="loading">Loading your rental...</div>
    <div v-else-if="!rental">You have no active rental.</div>
    <div v-else class="rental-card">
      <h3>{{ rental.equipment.name }}</h3>
      <p><strong>Issued At:</strong> {{ formatDate(rental.issuedAt) }}</p>
      <p><strong>Due Date:</strong> {{ formatDate(rental.dueDate) }}</p>
      <p>
        <strong>Returned At:</strong>
        {{ rental.returnedAt ? formatDate(rental.returnedAt) : 'Not returned yet' }}
      </p>
      <p><strong>Status:</strong> {{ rental.isActive ? 'Active' : 'Completed' }}</p>
      <p v-if="rental.overdueFee"><strong>Overdue Fee:</strong> ${{ rental.overdueFee }}</p>
      <p v-if="rental.extensionReason">
        <strong>Extension Reason:</strong> {{ rental.extensionReason }}
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getCustomerActiveRental } from '@/api/CustomerController'
import { useUserInformationStore } from '@/stores/UserInformation'

const rental = ref(null) // ✅ single rental
const loading = ref(true)

const userInfoStore = useUserInformationStore()

function formatDate(date) {
  return new Date(date).toLocaleDateString()
}

async function fetchRental() {
  try {
    const response = await getCustomerActiveRental(userInfoStore.id)
    rental.value = response
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

.rental-card {
  border: 1px solid #ccc;
  padding: 16px;
  margin-top: 16px;
  border-radius: 6px;
  background: #f9f9f9;
}
</style>
