<template>
  <v-container>
    <v-card class="mb-4 pa-4">
      <v-card-title>
        <div class="text-h6">{{ customer.name }}</div>
        <v-spacer />
        <v-chip class="text-uppercase" dark>
          {{ customer.role }}
        </v-chip>
      </v-card-title>

      <v-card-text>
        <div><strong>Email:</strong> {{ customer.email }}</div>
        <div><strong>Username:</strong> {{ customer.userName }}</div>
      </v-card-text>
    </v-card>

    <v-card>
      <v-card-title>Rental History</v-card-title>
      <CustomerRentedHistory :customer-id="customerId" />
    </v-card>
  </v-container>
</template>

<script setup>
import CustomerRentedHistory from '@/components/tables/customer/CustomerRentedHistory.vue'
import { onBeforeMount, ref } from 'vue'
import { useRoute } from 'vue-router'
import { getCustomer } from '@/api/CustomerController'

const route = useRoute()
const customerId = route.params.id

const customer = ref({})

onBeforeMount(async () => {
  customer.value = await getCustomer(customerId)
})
</script>
