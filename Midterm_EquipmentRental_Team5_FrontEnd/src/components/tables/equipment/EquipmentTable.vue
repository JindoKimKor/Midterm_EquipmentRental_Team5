<template>
  <v-card>
    <v-card-title>
      Equipment List
      <v-spacer />
      <div class="d-flex">
        <div>
          <AddEquipmentDialog v-model="isAddEquipmentDialogOpen" />
        </div>
        <div class="pr-4 pl-4">
          <ViewRentedEquipmentRentals v-model="isViewRentedEquipment" />
        </div>
        <!-- <div>
          <add-equipment-dialog v-model="isViewAvailableEquipment" />
        </div> -->
      </div>
    </v-card-title>

    <v-data-table :headers="headers" :items="equipment" :items-per-page="10" class="elevation-1">
      <template #item.name="{ item }">
        <router-link :to="`/equipments/${item.id}`" class="text-decoration-none">
          {{ item.name }}x
        </router-link>
      </template>

      <template #item.actions="{ item }">
        <v-btn v-if="isAdmin" icon size="small" color="blue" @click="editEquipment(item.id)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>

        <v-btn v-if="isAdmin" icon size="small" color="red" @click="deleteEquipment(item.id)">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref } from 'vue'
import AddEquipmentDialog from '@/components/dialog/equipment/AddEquipmentDialog.vue'
import ViewRentedEquipmentRentals from '@/components/dialog/equipment/ViewRentedEquipments.vue'

const isAdmin = true
const isUser = !isAdmin
let isAddEquipmentDialogOpen = ref(false)
let isViewRentedEquipment = ref(false)
let isViewAvailableEquipment = ref(false)

const headers = [
  { title: 'Name', value: 'name' },
  { title: 'Category', value: 'category' },
  { title: 'Condition', value: 'condition' },
  { title: 'Status', value: 'status' },
  isAdmin ? { title: 'Actions', value: 'actions', sortable: false } : {},
]

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

const goToCreateEquipment = () => {
  console.log('Navigate to create equipment form')
}

const viewDetails = (id) => {
  console.log('Viewing details for', id)
}

const editEquipment = (id) => {
  isAddEquipmentDialogOpen.value = true
}

const deleteEquipment = (id) => {
  if (confirm('Delete this equipment?')) {
    console.log('Deleting equipment', id)
  }
}
</script>
