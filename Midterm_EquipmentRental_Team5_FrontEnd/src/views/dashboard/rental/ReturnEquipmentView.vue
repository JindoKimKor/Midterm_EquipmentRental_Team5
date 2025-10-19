<template>
  <v-container fluid class="pa-4" style="max-width: 800px">
    <v-card elevation="3" rounded>
      <v-toolbar color="deep-purple accent-4" dark flat>
        <v-toolbar-title>Return Rental</v-toolbar-title>
      </v-toolbar>

      <v-card-text>
        <ReturnRentalForm v-if="role === 'Admin'" />
        <CustomerReturnRentalForm v-if="role === 'User'" />
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import CustomerReturnRentalForm from '@/components/forms/CustomerReturnRentalForm.vue'
import ReturnRentalForm from '@/components/forms/ReturnRentalForm.vue'
import { useUserInformationStore } from '@/stores/UserInformation'
import { ref } from 'vue'

const userInfoStore = useUserInformationStore()

const role = ref(userInfoStore.role)
</script>
