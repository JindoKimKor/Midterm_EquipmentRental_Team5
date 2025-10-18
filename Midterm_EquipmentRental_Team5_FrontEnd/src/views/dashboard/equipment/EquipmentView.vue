<template>
  <v-container fluid>
    <!-- Dashboard Cards -->
    <v-row dense>
      <v-col cols="12" sm="6" md="3">
        <v-card class="pa-4" elevation="6" color="blue lighten-2" dark>
          <v-card-title class="headline"> {{ totalEquipment }}0 </v-card-title>
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
            {{ rentedEquipment.length }}
          </v-card-title>
          <v-card-subtitle>Rented Equipment</v-card-subtitle>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" md="3">
        <v-card class="pa-4" elevation="6" color="green lighten-1" dark>
          <v-card-title class="headline">{{ availableEquipment.length }}</v-card-title>
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
import EquipmentTable from '@/components/tables/equipment/EquipmentTable.vue'
import RentedEquipmentTable from '@/components/tables/equipment/RentedEquipmentTable.vue'
import { ref } from 'vue'

const rentedEquipment = ref([])
const availableEquipment = ref([])
const activeRental = ref(null)

const showRented = ref(false)
</script>
