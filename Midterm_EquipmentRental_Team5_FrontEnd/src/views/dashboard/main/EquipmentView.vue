<template>
  <v-container fluid>
    <!-- Welcome and Role -->
    <v-row class="mb-6" align="center" justify="space-between">
      <v-col cols="12" md="6">
        <h1>Welcome back, {{ userName }}!</h1>
        <p>
          You are logged in as <strong>{{ userRole }}</strong>
        </p>
      </v-col>
      <v-col cols="12" md="4" class="text-right" v-if="userRole === 'Admin'">
        <v-btn color="primary" @click="goToCreate">+ Add New Equipment</v-btn>
      </v-col>
    </v-row>

    <!-- Dashboard Cards -->
    <v-row dense>
      <v-col cols="12" sm="6" md="3">
        <v-card class="pa-4" elevation="6" color="blue lighten-2" dark>
          <v-card-title class="headline">
            {{ totalEquipment }}
          </v-card-title>
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

    <!-- Rented Equipment Table (toggle) -->
    <v-row v-if="showRented" class="mt-6">
      <v-col cols="12">
        <v-data-table
          :headers="tableHeaders"
          :items="rentedEquipment"
          class="elevation-1"
          :items-per-page="5"
        >
          <template #item.condition="{ item }">
            {{ item.condition }}
          </template>
          <template #item.status="{ item }">
            <v-chip :color="item.isAvailable ? 'green' : 'red'" dark small>
              {{ item.isAvailable ? 'Available' : 'Rented' }}
            </v-chip>
          </template>
          <template #item.actions="{ item }" v-if="userRole === 'Admin'">
            <v-btn icon color="blue" @click="editEquipment(item.id)">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon color="red" @click="confirmDelete(item.id)">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
          <template #item.actions="{ item }" v-else>
            <v-btn small color="primary" @click="viewDetails(item.id)">Details</v-btn>
          </template>
        </v-data-table>
      </v-col>
    </v-row>

    <!-- Equipment Table (always visible for Admin and User) -->
    <v-row class="mt-8">
      <v-col cols="12">
        <EquipmentTable />
      </v-col>
    </v-row>

    <!-- Delete Confirmation Dialog -->
    <v-dialog v-model="showDeleteDialog" max-width="400">
      <v-card>
        <v-card-title class="headline">Confirm Delete</v-card-title>
        <v-card-text>Are you sure you want to delete this equipment?</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn text @click="showDeleteDialog = false">Cancel</v-btn>
          <v-btn color="red" text @click="deleteEquipment">Yes, Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Toast Snackbar -->
    <v-snackbar v-model="snackbar.show" :color="snackbar.color" timeout="3000">
      {{ snackbar.message }}
    </v-snackbar>
  </v-container>
</template>

<script setup>
import EquipmentTable from '@/components/tables/equipment/EquipmentTable.vue'
import { ref, onMounted, computed } from 'vue'
// import axios from 'axios'
import { useRouter } from 'vue-router'

const router = useRouter()

// // Simulated auth info
// const userName = ref(localStorage.getItem('name') || 'User')
// const userRole = ref(localStorage.getItem('role') || 'User')

const equipment = ref([])
const rentedEquipment = ref([])
const availableEquipment = ref([])
const activeRental = ref(null)

const showRented = ref(false)
const showDeleteDialog = ref(false)
const deletingId = ref(null)

const snackbar = ref({
  show: false,
  message: '',
  color: 'success',
})

const tableHeaders = [
  { text: 'Name', value: 'name' },
  { text: 'Category', value: 'category' },
  { text: 'Condition', value: 'condition' },
  { text: 'Status', value: 'status' },
  { text: 'Actions', value: 'actions', sortable: false },
]

// const fetchEquipment = async () => {
//   try {
//     const res = await axios.get('/api/equipment')
//     equipment.value = res.data
//   } catch (error) {
//     console.error(error)
//   }
// }

// const fetchRented = async () => {
//   if (userRole.value !== 'Admin') return
//   try {
//     const res = await axios.get('/api/equipment/rented')
//     rentedEquipment.value = res.data
//   } catch (error) {
//     console.error(error)
//   }
// }

// const fetchAvailable = async () => {
//   try {
//     const res = await axios.get('/api/equipment/available')
//     availableEquipment.value = res.data
//   } catch (error) {
//     console.error(error)
//   }
// }

// Simulate fetching user's active rental (user only)
const fetchActiveRental = async () => {
  if (userRole.value !== 'User') return
  // Replace with your API or logic to fetch current rental for logged user
  // Here, just simulating none active rental
  activeRental.value = null
}

const totalEquipment = computed(() => equipment.value.length)

onMounted(() => {
  fetchEquipment()
  fetchAvailable()
  fetchRented()
  fetchActiveRental()
})

const goToCreate = () => {
  router.push('/admin/equipment/create')
}

const editEquipment = (id) => {
  router.push(`/admin/equipment/edit/${id}`)
}

const viewDetails = (id) => {
  router.push(`/equipment/${id}`)
}

const confirmDelete = (id) => {
  deletingId.value = id
  showDeleteDialog.value = true
}

// const deleteEquipment = async () => {
//   try {
//     await axios.delete(`/api/equipment/${deletingId.value}`)
//     snackbar.value = { show: true, message: 'Deleted successfully', color: 'success' }
//     showDeleteDialog.value = false
//     // Refresh list
//     fetchEquipment()
//     fetchRented()
//     fetchAvailable()
//   } catch (error) {
//     snackbar.value = { show: true, message: 'Delete failed', color: 'error' }
//   }
// }
</script>
