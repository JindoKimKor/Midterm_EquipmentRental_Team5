<template>
  <UserActiveRental @rental-loaded="handleRentalLoaded" />

  <v-form v-if="form.rentalId" v-model="isFormValid" @submit.prevent="submitForm">
    <v-row dense>
      <v-col cols="12">
        <v-select
          v-model="form.condition"
          :items="conditionOptions"
          label="Condition Upon Return"
          :rules="[required]"
          clearable
          persistent-hint
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
        <v-btn
          type="submit"
          color="primary"
          :disabled="!isFormValid"
          variant="elevated"
          class="text-none"
        >
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

const isFormValid = ref(false)

const form = ref({
  rentalId: null,
  condition: null,
  notes: '',
})

const conditionOptions = ['Good', 'Damaged', 'Broken', 'Needs Maintenance']
const required = (value) => !!value || 'This field is required.'

function handleRentalLoaded(rental) {
  if (rental?.id) {
    form.value.rentalId = rental.id
  }
}

async function submitForm() {
  try {
    await returnEquipment({ ...form.value })
    alert('Equipment return submitted successfully!')
    resetForm()
  } catch (error) {
    console.error('Return submission failed:', error)
    alert('Error submitting return. Please try again.')
  }
}

function resetForm() {
  form.value = {
    rentalId: null,
    condition: null,
    notes: '',
  }
}
</script>
