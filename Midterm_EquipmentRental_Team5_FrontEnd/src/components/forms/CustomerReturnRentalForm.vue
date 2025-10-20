<template>
  <UserActiveRental @rental-loaded="handleRentalLoaded" />

  <v-form v-if="form.rentalId" v-model="isFormValid" @submit.prevent="submitForm" class="mt-4">
    <v-row dense>
      <v-col cols="12">
        <v-select
          v-model="form.condition"
          :items="conditionOptions"
          label="Condition Upon Return"
          :rules="[required]"
          clearable
          variant="outlined"
          density="comfortable"
        />
      </v-col>

      <v-col cols="12">
        <v-textarea
          v-model="form.notes"
          label="Additional Notes"
          rows="3"
          auto-grow
          clearable
          variant="outlined"
          density="comfortable"
        />
      </v-col>

      <v-col cols="12" class="d-flex justify-end">
        <v-btn type="submit" color="primary" :disabled="!isFormValid" variant="elevated">
          Submit Return
        </v-btn>
      </v-col>
    </v-row>
  </v-form>

  <v-alert v-else type="info" class="mt-4" variant="tonal" border="start" border-color="primary">
    You currently have no active rental to return.
  </v-alert>
</template>

<script setup>
import { ref } from 'vue'
import UserActiveRental from '../UserActiveRental.vue'
import { returnEquipment } from '@/api/RentalController'

// Form state
const form = ref({
  rentalId: null,
  condition: null,
  notes: '',
})

const isFormValid = ref(false)

// Dropdown options
const conditionOptions = ['Good', 'Damaged', 'Broken', 'Needs Maintenance']

// Validation rule
const required = (value) => !!value || 'This field is required.'

// Handle loaded rental from child component
function handleRentalLoaded(rental) {
  form.value.rentalId = rental?.id || null
}

// Submit handler
async function submitForm() {
  try {
    await returnEquipment({ ...form.value })
    alert('Equipment return submitted successfully!')
    resetForm()
  } catch (error) {
    console.error('Return submission failed:', error)
    alert('Failed to submit return. Please try again.')
  }
}

// Reset form
function resetForm() {
  form.value.condition = null
  form.value.notes = ''
  form.value.rentalId = null
}
</script>
