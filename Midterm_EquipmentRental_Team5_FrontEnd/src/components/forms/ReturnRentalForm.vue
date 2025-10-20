<template>
  <v-container class="pa-4" max-width="600">
    <v-form @submit.prevent="submitForm" v-model="isFormValid">
      <v-row dense>
        <!-- Rental Info Dropdown -->
        <v-col cols="12">
          <v-select
            v-model="form.rentalId"
            :items="rentalOptions"
            item-title="title"
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
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getActiveRentals, returnEquipment } from '@/api/RentalController'

const form = ref({
  rentalId: null,
  condition: null,
  notes: '',
})
const isFormValid = ref(false)

const required = (value) => !!value || 'Required'

const rentalOptions = ref([])
const conditionOptions = ['New', 'Excellent', 'Good', 'Fair', 'Poor']

onMounted(async () => {
  try {
    const response = await getActiveRentals()
    if (response)
      rentalOptions.value = response.map((r) => {
        return {
          id: r.id,
          title: r.customer.name + ' - ' + r.equipment.name,
        }
      })
  } catch (error) {
    console.error('Failed to load rentals', error)
  }
})

async function submitForm() {
  try {
    const payload = {
      RentalId: form.value.rentalId,
      Condition: form.value.condition,
      Notes: form.value.notes,
    }
    await returnEquipment(payload)
    resetForm()
    alert('Successfully  to submit return')
  } catch (error) {
    console.error('Failed to submit return', error)
    alert('Failed to submit return')
  }
}

function resetForm() {
  form.value.rentalId = null
  form.value.condition = null
  form.value.notes = ''
}
</script>
