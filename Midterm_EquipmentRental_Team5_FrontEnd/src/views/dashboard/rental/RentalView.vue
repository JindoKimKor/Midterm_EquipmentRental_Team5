<template>
  <v-container fluid>
    <RentalTable />

    <ActiveRentalHistoryTable />

    <GetCompletedRentalTable class="mt-6" />
    <GetOverDueRentalTable />

    <OverdueRentalsAlertTable class="mt-6" />
  </v-container>
</template>

<script setup>
import ActiveRentalHistoryTable from '@/components/tables/rentals/ActiveRentalHistoryTable.vue'
import GetCompletedRentalTable from '@/components/tables/rentals/GetCompletedRentalTable.vue'
import RentalTable from '@/components/tables/rentals/RentalTable.vue'
import GetOverDueRentalTable from '@/components/tables/rentals/OverdueRentalTable.vue'
</script>

<style scoped>
.bg-red-lighten-5 {
  background-color: #ffebee !important;
}
</style>
