<template>
  <v-container fluid>
    <RentedEquipmentTable />
    <!-- Snackbar -->
    <v-snackbar v-model="snackbar.show" :color="snackbar.color" timeout="3000">
      {{ snackbar.message }}
    </v-snackbar>
  </v-container>
</template>

<script setup>
import RentedEquipmentTable from '@/components/tables/equipment/RentedEquipmentTable.vue'
import { ref } from 'vue'

const snackbar = ref({
  show: false,
  message: '',
  color: '',
})
</script>

<style scoped>
.bg-red-lighten-5 {
  background-color: #ffebee !important;
}
</style>
