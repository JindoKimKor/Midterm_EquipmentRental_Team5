<template>
  <v-container>
    <v-data-table :headers="headers" :items="equipments" class="elevation-1">
      <template #item.rentalPrice="{ item }"> ${{ item.rentalPrice.toFixed(2) }} </template>

      <template #item.isAvailable="{ item }">
        <v-chip :color="item.isAvailable ? 'green' : 'red'" dark>
          {{ item.isAvailable ? 'Available' : 'Unavailable' }}
        </v-chip>
      </template>

      <template #item.createdAt="{ item }">
        {{ new Date(item.createdAt).toLocaleDateString() }}
      </template>
    </v-data-table>
  </v-container>
</template>

<script setup>
import { getAvailableEquipment } from '@/api/EquipmentController'
import { onBeforeMount, ref } from 'vue'

const headers = [
  { title: 'ID', key: 'id' },
  { title: 'Name', key: 'name' },
  { title: 'Description', key: 'description' },
  { title: 'Category', key: 'category' },
  { title: 'Condition', key: 'condition' },
  { title: 'Rental Price', key: 'rentalPrice' },
  { title: 'Available', key: 'isAvailable' },
  { title: 'Created At', key: 'createdAt' },
]

const equipments = ref([])

onBeforeMount(async () => {
  equipments = await getAvailableEquipment()
})
</script>
