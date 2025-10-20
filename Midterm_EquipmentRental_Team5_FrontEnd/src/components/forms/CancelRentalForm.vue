<template>
  <v-container class="pa-4" max-width="600">
    <v-form @submit.prevent="submitForm" v-model="isFormValid">
      <v-select
        v-model="form.rentalId"
        :items="rentalOptions"
        item-title="title"
        item-value="id"
        label="Select Rental"
        :rules="[required]"
        class="mb-4"
      />

      <v-btn type="submit" color="error" :disabled="!isFormValid" :loading="submitting" block>
        Cancel Rental
      </v-btn>
    </v-form>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { cancelRental, getActiveRentals } from '@/api/RentalController'

const form = ref({ rentalId: null })
const isFormValid = ref(false)
const submitting = ref(false)
const rentalOptions = ref([])

const required = (value) => !!value || 'Required'

onMounted(loadOptions)

async function loadOptions() {
  try {
    const rentals = await getActiveRentals()
    rentalOptions.value = rentals.map((r) => ({
      id: r.id,
      title: `${r.customer.name} - ${r.equipment.name}`,
    }))
  } catch (err) {
    console.error(err)
  }
}

function removeFromOptions(id) {
  rentalOptions = rentalOptions.map((m) => {
    if (m.id != id) return m
  })
}

async function submitForm() {
  if (!form.value.rentalId) return
  submitting.value = true

  try {
    await cancelRental(form.value.rentalId)
    alert('Rental cancelled.')
    removeFromOptions(form.value.rentalId)
    form.value.rentalId = null
  } catch (err) {
    alert('Cancellation failed.')
  } finally {
    submitting.value = false
  }
}
</script>
