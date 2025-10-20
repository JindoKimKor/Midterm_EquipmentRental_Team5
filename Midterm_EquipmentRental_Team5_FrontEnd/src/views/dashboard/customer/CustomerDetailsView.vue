<template>
  <v-container>
    <v-card class="mb-6" elevation="2">
      <v-card-title class="d-flex align-center justify-space-between">
        <div class="text-h6 font-weight-medium">
          {{ customer?.name || 'Customer Information' }}
          <v-chip color="primary" class="text-uppercase" variant="elevated" size="small">
            {{ customer?.role || 'N/A' }}
          </v-chip>
        </div>

        <v-btn
          color="primary"
          variant="elevated"
          prepend-icon="mdi-pencil"
          @click="openEditDialog"
        >
          Edit Profile
        </v-btn>
      </v-card-title>

      <v-divider />

      <v-card-text v-if="loading" class="d-flex justify-center py-6">
        <v-progress-circular indeterminate color="primary" />
      </v-card-text>

      <v-card-text v-else-if="error" class="text-error text-center py-6">
        Failed to load customer information.
      </v-card-text>

      <v-card-text v-else>
        <v-row dense>
          <v-col cols="12" sm="6">
            <strong>Email:</strong> {{ customer?.email || 'Not provided' }}
          </v-col>
          <v-col cols="12" sm="6">
            <strong>Username:</strong> {{ customer?.userName || 'N/A' }}
          </v-col>
        </v-row>
      </v-card-text>

      <v-divider class="my-2" />

      <v-card-text>
        <CustomerRentedHistory :customer-id="customerId" />
      </v-card-text>
    </v-card>

    <!-- Edit Profile Dialog -->
    <AddCustomerDialog
      v-model="isAddCustomerDialogOpen"
      :customer="customer"
      :show-activator="false"
      @saved="refreshCustomers"
    />
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import CustomerRentedHistory from '@/components/tables/customer/CustomerRentedHistory.vue'
import { getCustomer } from '@/api/CustomerController'
import AddCustomerDialog from '@/components/dialog/AddCustomerDialog.vue'

const route = useRoute()
const customerId = route.params.id
const isAddCustomerDialogOpen = ref(false)

const customer = ref(null)
const loading = ref(true)
const error = ref(false)

const openEditDialog = () => {
  isAddCustomerDialogOpen.value = true
}

onMounted(async () => await refreshCustomers())

async function refreshCustomers() {
  loading.value = true
  error.value = false

  try {
    const res = await getCustomer(customerId)
    customer.value = res?.data ?? res
    console.log('Customer loaded:', customer.value)
  } catch (err) {
    console.error('Error loading customer:', err)
    error.value = true
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.text-error {
  color: #d32f2f;
}
</style>
