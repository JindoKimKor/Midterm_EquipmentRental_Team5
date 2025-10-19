<template>
  <v-card class="pa-4" max-width="600">
    <v-card-title class="text-h6">Issue Equipment</v-card-title>

    <v-form @submit.prevent="submitForm" v-model="isFormValid">
      <v-row dense>
        <!-- Equipment Dropdown -->
        <v-col cols="12">
          <v-select
            v-model="form.equipmentId"
            :items="equipmentOptions"
            item-title="name"
            item-value="id"
            label="Select Equipment"
            :rules="[required]"
            :loading="loadingEquipment"
            :return-object="false"
            variant="outlined"
            density="comfortable"
          />
        </v-col>

        <!-- Customer Dropdown -->
        <v-col cols="12">
          <v-select
            v-model="form.customerId"
            :items="customerOptions"
            item-title="name"
            item-value="id"
            label="Select Customer"
            :rules="[required]"
            :loading="loadingCustomers"
            :return-object="false"
            variant="outlined"
            density="comfortable"
          />
        </v-col>

        <!-- Submit Button -->
        <v-col cols="12" class="d-flex justify-end">
          <v-btn type="submit" color="primary" :disabled="!isFormValid" :loading="submitting">
            ISSUE
          </v-btn>
        </v-col>
      </v-row>
    </v-form>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getAvailableEquipment } from '@/api/EquipmentController'
import { getAllCustomers } from '@/api/CustomerController'
import { issueEquipment } from '@/api/RentalController'

// Form state
const form = ref({
  equipmentId: null,
  customerId: null,
})
const isFormValid = ref(false)

// Dropdown options
const equipmentOptions = ref([])
const customerOptions = ref([])

// Loading states
const loadingEquipment = ref(false)
const loadingCustomers = ref(false)
const submitting = ref(false)

// Validation rule
const required = (value) => !!value || 'Required'

// Load equipment and customers
onMounted(async () => {
  await loadOptions()
})

async function loadOptions() {
  loadingEquipment.value = true
  loadingCustomers.value = true

  try {
    const [equipRes, customerRes] = await Promise.all([getAvailableEquipment(), getAllCustomers()])

    equipmentOptions.value = equipRes?.data || equipRes || []
    customerOptions.value = customerRes?.data || customerRes || []

    console.log('Equipment loaded:', equipmentOptions.value)
    console.log('Customers loaded:', customerOptions.value)
  } catch (error) {
    console.error('Failed to load options', error)
  } finally {
    loadingEquipment.value = false
    loadingCustomers.value = false
  }
}

// Submit handler
async function submitForm() {
  if (!isFormValid.value) return

  submitting.value = true

  try {
    const payload = {
      equipmentId: form.value.equipmentId,
      customerId: form.value.customerId,
    }

    const response = await issueEquipment(payload)

    console.log('Rental issued successfully:', response)
    alert('Equipment issued successfully!')

    // Reset form
    form.value.equipmentId = null
    form.value.customerId = null

    // Reload equipment to update available items
    await loadOptions()
  } catch (error) {
    console.error('Issue failed', error)
    alert('Failed to issue equipment. Please try again.')
  } finally {
    submitting.value = false
  }
}
</script>
