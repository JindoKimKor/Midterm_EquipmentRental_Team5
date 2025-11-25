<template>
  <v-container fluid>
    <!-- Dashboard Cards -->
    <v-row dense>
      <v-col cols="12" sm="6" md="3">
        <v-card class="pa-4" elevation="6" color="blue lighten-2" dark>
          <v-card-title class="headline"> {{ totalEquipment }} </v-card-title>
          <v-card-subtitle>Total Equipment</v-card-subtitle>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" md="3">
        <v-card
          class="pa-4"
          elevation="6"
          color="orange lighten-1"
          dark
          @click="showRented = !showRented"
          style="cursor: pointer"
        >
          <v-card-title class="headline">
            {{ totalRentedEquipment }}
          </v-card-title>
          <v-card-subtitle>Rented Equipment</v-card-subtitle>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" md="3">
        <v-card class="pa-4" elevation="6" color="green lighten-1" dark>
          <v-card-title class="headline">{{ totalAvailableEquipment }}</v-card-title>
          <v-card-subtitle>Available Equipment</v-card-subtitle>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" md="3" v-if="userRole === 'User'">
        <v-card class="pa-4" elevation="6" color="purple lighten-1" dark>
          <v-card-title class="headline">
            {{ activeRental ? activeRental.name : 'None' }}
          </v-card-title>
          <v-card-subtitle>My Active Rental</v-card-subtitle>
        </v-card>
      </v-col>
    </v-row>
    <v-row class="mt-8">
      <v-col cols="12">
        <EquipmentTable />
      </v-col>
    </v-row>
    <v-row class="mt-8">
      <v-col cols="12">
        <RentedEquipmentTable />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import {
  getAllEquipment,
  getAvailableEquipment,
  getRentedEquipmentSummary,
} from '@/api/EquipmentController'
import EquipmentTable from '@/components/tables/equipment/EquipmentTable.vue'
import RentedEquipmentTable from '@/components/tables/equipment/RentedEquipmentTable.vue'
import { onBeforeMount, ref } from 'vue'

const showRented = ref(false)

const totalEquipment = ref(0)
const totalRentedEquipment = ref(0)
const totalAvailableEquipment = ref(0)

onBeforeMount(async () => {
  const res = await Promise.all([
    getAllEquipment(),
    getRentedEquipmentSummary(),
    getAvailableEquipment(),
  ])

  totalEquipment.value = res[0].length
  totalRentedEquipment.value = res[1].length
  totalAvailableEquipment.value = res[2].length
})
</script>
