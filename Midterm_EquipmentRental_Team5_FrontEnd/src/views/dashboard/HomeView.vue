<template>
  <v-container class="d-flex flex-column">
    <EquipmentTable />
    <div class="pt-5" />
    <CustomerTable />
    <div class="pt-5" />
    <RentalTable />
  </v-container>
</template>

<script setup>
import EquipmentTable from '@/components/tables/EquipmentTable.vue'
import CustomerTable from '@/components/tables/CustomerTable.vue'
import RentalTable from '@/components/tables/RentalTable.vue'
</script>
