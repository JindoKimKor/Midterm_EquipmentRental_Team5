<template>
  <v-card>
    <v-card-title>
      Equipment List
      <v-spacer />
      <v-btn v-if="isAdmin" color="primary" @click="goToCreateEquipment">
        <v-icon start>mdi-plus</v-icon>
        Add Equipment
      </v-btn>
    </v-card-title>

    <v-data-table :headers="headers" :items="equipment" :items-per-page="10" class="elevation-1">
      <template #item.name="{ item }">
        <router-link :to="`/equipments/${item.id}`" class="text-decoration-none">
          {{ item.name }}
        </router-link>
      </template>

      <template #item.actions="{ item }">
        <v-btn icon size="small" color="blue" @click="editEquipment(item.id)">
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
import { ref } from 'vue'

const isAdmin = true
const isUser = !isAdmin

// Headers for table
const headers = [
  { title: 'Name', value: 'name' },
  { title: 'Category', value: 'category' },
  { title: 'Condition', value: 'condition' },
  { title: 'Status', value: 'status' },
  isAdmin ? { title: 'Actions', value: 'actions', sortable: false } : {},
]

// Example equipment data (replace with API call)
const equipment = ref([
  {
    id: 1,
    name: 'Canon DSLR',
    category: 'Camera',
    condition: 'Good',
    status: 'Available',
  },
  {
    id: 2,
    name: 'Laptop - Dell XPS',
    category: 'Computer',
    condition: 'Fair',
    status: 'In Use',
  },
])

// Action handlers (stubbed for now)
const goToCreateEquipment = () => {
  console.log('Navigate to create equipment form')
}

const viewDetails = (id) => {
  console.log('Viewing details for', id)
}

const editEquipment = (id) => {
  console.log('Editing equipment', id)
}

const deleteEquipment = (id) => {
  if (confirm('Delete this equipment?')) {
    console.log('Deleting equipment', id)
  }
}
</script>
