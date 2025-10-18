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
            return-object="{false}"
          />
        </v-col>

        <!-- Customer Dropdown -->
        <v-col cols="12">
          <v-select
            v-model="form.customerId"
            :items="customerOptions"
            item-title="fullName"
            item-value="id"
            label="Select Customer"
            :rules="[required]"
            return-object="{false}"
          />
        </v-col>

        <!-- Submit Button -->
        <v-col cols="12" class="d-flex justify-end">
          <v-btn type="submit" color="primary" :disabled="!isFormValid"> Issue </v-btn>
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
  equipmentId: null,
  customerId: null,
})
const isFormValid = ref(false)

// Dropdown options
const equipmentOptions = ref([])
const customerOptions = ref([])

// Validation rule
const required = (value) => !!value || 'Required'

// Load equipment and customers
onMounted(async () => {
  try {
    const [equipRes, customerRes] = await Promise.all([
      axios.get('/api/equipment'),
      axios.get('/api/customers'),
    ])

    equipmentOptions.value = equipRes.data || []
    customerOptions.value = customerRes.data || []
  } catch (error) {
    console.error('Failed to load options', error)
  }
})

// Submit handler
async function submitForm() {
  try {
    const payload = {
      equipmentId: form.value.equipmentId,
      customerId: form.value.customerId,
    }

    await axios.post('/api/rentals/issue', payload)

    alert('Rental issued successfully!')
    // Reset form
    form.value.equipmentId = null
    form.value.customerId = null
  } catch (error) {
    console.error('Issue failed', error)
    alert('Failed to issue rental')
  }
}
</script>
