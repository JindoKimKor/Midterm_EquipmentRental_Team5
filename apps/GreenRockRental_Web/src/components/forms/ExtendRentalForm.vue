<template>
  <v-container class="pa-4" max-width="600">
    <v-form @submit.prevent="submitForm" v-model="isFormValid">
      <v-row dense>
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

        <v-col cols="12">
          <v-text-field
            v-model="form.newReturnDate"
            label="New Return Date"
            type="date"
            :rules="[required]"
          />
        </v-col>

        <!-- Notes Textarea -->
        <v-col cols="12">
          <v-textarea v-model="form.notes" label="Notes" rows="3" auto-grow clearable />
        </v-col>

        <v-col cols="12" class="d-flex justify-end">
          <v-btn type="submit" color="primary" :disabled="!isFormValid"> Extend Rental </v-btn>
        </v-col>
      </v-row>
    </v-form>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { extendRental, getActiveRentals } from '@/api/RentalController'

const form = ref({
  rentalId: null,
  newReturnDate: null,
  notes: '',
})

const isFormValid = ref(false)

const rentalOptions = ref([])
const required = (value) => !!value || 'Required'

onMounted(loadOptions)

async function loadOptions() {
  try {
    const rentals = await getActiveRentals()
    if (rentals)
      rentalOptions.value = rentals.map((r) => ({
        id: r.id,
        title: `${r.customerName} - ${r.equipmentName}`,
      }))
  } catch (err) {
    console.error(err)
  }
}

async function submitForm() {
  try {
    await extendRental(form.value.rentalId, {
      NewDueDate: form.value.newReturnDate,
      Reason: form.value.notes,
    })
    alert('Rental extended successfully!')
    resetForm()
  } catch (error) {
    console.error('Extension failed:', error)
    alert('Failed to extend rental.')
  }
}

function resetForm() {
  form.value.rentalId = null
  form.value.newReturnDate = null
}
</script>
