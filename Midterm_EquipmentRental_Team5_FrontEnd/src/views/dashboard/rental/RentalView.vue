<template>
  <v-container fluid>
    <div v-if="role === 'Admin'">
      <RentalTable />
      <ActiveRentalHistoryTable class="mt-6" />
      <CompletedRentalTable class="mt-6" />
      <OverdueRentalTable class="mt-6" />
    </div>
    <div v-if="role === 'User'">
      <UserActiveRental />
    </div>
  </v-container>
</template>

<script setup>
import ActiveRentalHistoryTable from '@/components/tables/rentals/ActiveRentalHistoryTable.vue'
import CompletedRentalTable from '@/components/tables/rentals/CompletedRentalTable.vue'
import RentalTable from '@/components/tables/rentals/RentalTable.vue'
import OverdueRentalTable from '@/components/tables/rentals/OverdueRentalTable.vue'
import useAuthenticationStore from '@/stores/Authentication'
import { ref } from 'vue'
import UserActiveRental from '@/components/UserActiveRental.vue'

const userInfoStore = useAuthenticationStore()

const role = ref(userInfoStore.authRole)
</script>

<style scoped>
.bg-red-lighten-5 {
  background-color: #ffebee !important;
}
</style>
