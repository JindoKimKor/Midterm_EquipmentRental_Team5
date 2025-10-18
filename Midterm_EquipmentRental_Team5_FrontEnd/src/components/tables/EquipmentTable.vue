<template>
  <v-card>
    <v-card-title>
      Equipment List
      <v-spacer />
      AddEquipmentDialog
    </v-card-title>

    <v-data-table :headers="headers" :items="equipment" :items-per-page="10" class="elevation-1">
      <template #item.name="{ item }">
        <router-link :to="`/equipments/${item.id}`" class="text-decoration-none">
          {{ item.name }}
        </router-link>
      </template>

      <template #item.actions="{ item }">
        <v-btn icon size="small" color="blue" @click="editEquipment(item)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>
        <v-btn icon size="small" color="red" @click="deleteEquipment(item.id)">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { getAllEquipment } from '@/api/EquipmentController'
import { onBeforeMount, ref } from 'vue'

const isAdmin = true

const headers = [
  { title: 'Name', value: 'name' },
  { title: 'Category', value: 'category' },
  { title: 'Condition', value: 'condition' },
  { title: 'Status', value: 'status' },
  isAdmin ? { title: 'Actions', value: 'actions', sortable: false } : {},
]

const equipment = ref([])

onBeforeMount(async () => {
  equipment.value = await getAllEquipment()
})

const goToCreateEquipment = () => {
  console.log('Navigate to create equipment form')
}

const editEquipment = (id) => {
  console.log('Editing equipment', id)
}

const deleteEquipment = (id) => {
  if (confirm('Delete this equipment?')) {
    deleteEquipment(id)
  }
}
</script>
