<template>
  <v-card>
    <v-card-title>
      Equipment List
      <v-spacer />
      <AddEquipmentDialog v-model="isAddEquipmentDialogOpen" />
    </v-card-title>

    <v-data-table
      :headers="headers"
      :items="equipment"
      :items-per-page="10"
      class="elevation-1"
      density="comfortable"
      hover
    >
      <!-- Name as link -->
      <template #item.name="{ item }">
        <router-link :to="`/equipments/${item.id}`" class="text-decoration-none font-weight-medium">
          {{ item.name }}
        </router-link>
      </template>

      <!-- Rental Price formatted as currency -->
      <template #item.rentalPrice="{ item }">
        {{
          new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(
            item.rentalPrice,
          )
        }}
      </template>

      <!-- Availability as colored chip -->
      <template #item.isAvailable="{ item }">
        <v-chip :color="item.isAvailable ? 'green' : 'red'" variant="flat" label small>
          {{ item.isAvailable ? 'Available' : 'Unavailable' }}
        </v-chip>
      </template>

      <!-- Created At formatted -->
      <template #item.createdAt="{ item }">
        {{ new Date(item.createdAt).toLocaleDateString() }}
      </template>

      <!-- Actions with tooltips -->
      <template #item.actions="{ item }">
        <v-tooltip text="Edit" location="top">
          <template #activator="{ props }">
            <v-btn icon size="small" color="blue" @click="editEquipment(item)" v-bind="props">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
          </template>
        </v-tooltip>

        <v-tooltip text="Delete" location="top">
          <template #activator="{ props }">
            <v-btn
              icon
              size="small"
              color="red"
              @click="deleteEquipmentHandler(item.id)"
              v-bind="props"
            >
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-tooltip>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { deleteEquipment, getAllEquipment } from '@/api/EquipmentController'
import AddEquipmentDialog from '@/components/dialog/equipment/AddEquipmentDialog.vue'
import { onBeforeMount, ref } from 'vue'

const isAdmin = true
let isAddEquipmentDialogOpen = ref(false)

const headers = [
  { title: 'Name', value: 'name' },
  { title: 'Category', value: 'category' },
  { title: 'Condition', value: 'condition' },
  { title: 'Rental Price', value: 'rentalPrice' },
  { title: 'Available', value: 'isAvailable' },
  { title: 'Created At', value: 'createdAt' },
  ...(isAdmin ? [{ title: 'Actions', value: 'actions', sortable: false }] : []),
]

const equipment = ref([])

onBeforeMount(async () => {
  equipment.value = await getAllEquipment()
})

const goToCreateEquipment = () => {
  console.log('Navigate to create equipment form')
}

const editEquipment = (id) => {
  isAddEquipmentDialogOpen.value = true
  console.log('Editing equipment', id)
}

const deleteEquipmentHandler = (id) => {
  if (confirm('Delete this equipment?')) {
    deleteEquipment(id)
  }
}
</script>
