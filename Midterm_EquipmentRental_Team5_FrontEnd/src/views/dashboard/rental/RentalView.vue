<template>
  <v-container fluid>
    <RentalTable />
    <ActiveRentalHistoryTable class="mt-6" />
    <CompletedRentalTable class="mt-6" />
    <OverdueRentalTable class="mt-6" />
  </v-container>
</template>

<script setup>
import ActiveRentalHistoryTable from '@/components/tables/rentals/ActiveRentalHistoryTable.vue'
import CompletedRentalTable from '@/components/tables/rentals/CompletedRentalTable.vue'
import RentalTable from '@/components/tables/rentals/RentalTable.vue'
import OverdueRentalTable from '@/components/tables/rentals/OverdueRentalTable.vue'
</script>

<style scoped>
.bg-red-lighten-5 {
  background-color: #ffebee !important;
}
</style>
