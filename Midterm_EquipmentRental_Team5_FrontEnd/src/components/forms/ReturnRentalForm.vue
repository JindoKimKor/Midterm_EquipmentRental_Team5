<template>
  <v-card class="pa-4" max-width="600">
    <v-card-title class="text-h6">Return Equipment</v-card-title>

    <v-form @submit.prevent="submitForm" v-model="isFormValid">
      <v-row dense>
        <!-- Rental Info Dropdown -->
        <v-col cols="12">
          <v-select
            v-model="form.rentalId"
            :items="rentalOptions"
            item-title="label"
            item-value="id"
            label="Rental"
            :rules="[required]"
          />
        </v-col>

        <!-- Condition Dropdown -->
        <v-col cols="12">
          <v-select
            v-model="form.condition"
            :items="conditionOptions"
            label="Condition Upon Return"
            :rules="[required]"
          />
        </v-col>

        <!-- Notes Textarea -->
        <v-col cols="12">
          <v-textarea v-model="form.notes" label="Notes" rows="3" auto-grow clearable />
        </v-col>

        <!-- Submit Button -->
        <v-col cols="12" class="d-flex justify-end">
          <v-btn type="submit" color="primary" :disabled="!isFormValid"> Return </v-btn>
        </v-col>
      </v-row>
    </v-form>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

// Form state
const form = ref({
  rentalId: null,
  condition: null,
  notes: '',
})
const isFormValid = ref(false)

// Required rule
const required = (value) => !!value || 'Required'

// Options
const rentalOptions = ref([])
const conditionOptions = ['Good', 'Damaged', 'Broken', 'Needs Maintenance']

// Fetch current active rentals
onMounted(async () => {
  try {
    const response = await axios.get('/api/rentals/active') // or your actual endpoint
    rentalOptions.value = response.data.map((rental) => ({
      id: rental.id,
      label: `${rental.equipmentName} (Rented by ${rental.customerName})`,
    }))
  } catch (error) {
    console.error('Failed to load rentals', error)
  }
})

// Submit handler
async function submitForm() {
  try {
    const payload = {
      rentalId: form.value.rentalId,
      condition: form.value.condition,
      notes: form.value.notes,
    }

    await axios.post('/api/rentals/return', payload)
    alert('Return submitted successfully!')

    // Reset form
    form.value.rentalId = null
    form.value.condition = null
    form.value.notes = ''
  } catch (error) {
    console.error('Failed to submit return', error)
    alert('Failed to submit return')
  }
}
</script>
