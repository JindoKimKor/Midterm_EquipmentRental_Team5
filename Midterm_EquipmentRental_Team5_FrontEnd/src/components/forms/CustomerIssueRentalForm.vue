<template>
  <v-container class="pa-4" max-width="600">
    <div v-if="loadingRental" class="text-center pa-4">
      <v-progress-circular indeterminate color="primary" />
    </div>

    <div v-else-if="hasActiveRental" class="text-subtitle-1 pa-4 text-error">
      You already have an active rental. You must return it before issuing new equipment.
    </div>

    <v-form v-else @submit.prevent="submitForm" v-model="isFormValid">
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

        <!-- Submit Button -->
        <v-col cols="12" class="d-flex justify-end">
          <v-btn
            type="submit"
            color="primary"
            :disabled="!isFormValid || submitting"
            :loading="submitting"
          >
            ISSUE
          </v-btn>
        </v-col>
      </v-row>
    </v-form>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getAvailableEquipment } from '@/api/EquipmentController'
import { issueEquipment } from '@/api/RentalController'
import { getCustomerActiveRental } from '@/api/CustomerController'
import { useAuthenticationStore } from '@/stores/Authentication'
import router from '@/router'

const useAuthStore = useAuthenticationStore()

const form = ref({
  equipmentId: null,
  customerId: useAuthStore.authUserId,
})
const isFormValid = ref(false)
const equipmentOptions = ref([])
const loadingEquipment = ref(false)
const submitting = ref(false)

const loadingRental = ref(true)
const hasActiveRental = ref(false)

const required = (value) => !!value || 'Required'

// Check for existing rental and load equipment
onMounted(async () => {
  await checkCustomerRental()
  if (!hasActiveRental.value) {
    await loadEquipmentOptions()
  }
})

// 🔍 Check if the customer already has an active rental
async function checkCustomerRental() {
  loadingRental.value = true
  try {
    const rental = await getCustomerActiveRental(useAuthStore.authUserId)
    hasActiveRental.value = !!rental
  } catch (error) {
    hasActiveRental.value = false
  } finally {
    loadingRental.value = false
  }
}

// 🎒 Load available equipment
async function loadEquipmentOptions() {
  loadingEquipment.value = true
  try {
    const response = await getAvailableEquipment()
    equipmentOptions.value = response?.data || response || []
  } catch (error) {
    console.error('Failed to load equipment', error)
  } finally {
    loadingEquipment.value = false
  }
}

// 📤 Submit form to issue equipment
async function submitForm() {
  if (!isFormValid.value) return
  submitting.value = true

  try {
    const payload = {
      EquipmentId: form.value.equipmentId,
      CustomerId: form.value.customerId,
    }

    await issueEquipment(payload)
    alert('Equipment issued successfully!')
    resetForm()
  } catch (error) {
    console.error('Issue failed', error)
    alert('Failed to issue equipment. Please try again.')
  } finally {
    submitting.value = false
  }
}

function resetForm() {
  router.go(0)
}
</script>
