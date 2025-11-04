<template>
  <v-card elevation="3" rounded>
    <v-card-title class="d-flex align-center">
      <span class="text-h5 font-weight-semibold">Equipment List</span>
      <v-spacer />
      <div v-if="isAdmin">
        <AddEquipmentDialog
          v-model="isAddEquipmentDialogOpen"
          :equipment="selectedEquipment"
          @saved="refreshEquipments"
          @closed="cleanup"
        />
      </div>
    </v-card-title>

    <v-data-table
      :headers="headers"
      :items="equipment"
      :items-per-page="10"
      class="elevation-1"
      density="comfortable"
      hover
      fixed-header
      height="600"
    >
      <!-- Name as link -->
      <template v-slot:[`item.name`]="{ item }">
        <router-link
          :to="`/dashboard/equipments/${item.id}`"
          class="text-decoration-none font-weight-medium"
        >
          {{ item.name }}
        </router-link>
      </template>
      <!-- Category -->
      <template #item.category="{ item }">
        <v-chip class="ma-0" color="indigo lighten-4" text-color="indigo darken-2" small label>
          {{ item.category }}
        </v-chip>
      </template>

      <!-- Condition -->
      <template #item.condition="{ item }">
        <v-chip class="ma-0" color="grey lighten-3" text-color="grey darken-4" small label>
          {{ item.condition }}
        </v-chip>
      </template>

      <!-- Rental Price formatted as currency -->
      <template #item.rentalPrice="{ item }">
        <span class="font-mono font-weight-medium">
          {{
            new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(
              item.rentalPrice,
            )
          }}
        </span>
      </template>

      <!-- Availability as colored chip -->
      <template #item.isAvailable="{ item }">
        <v-chip
          :color="item.isAvailable ? 'green lighten-4' : 'red lighten-4'"
          :text-color="item.isAvailable ? 'green darken-2' : 'red darken-2'"
          variant="flat"
          label
          small
          class="ma-0"
        >
          {{ item.isAvailable ? 'Available' : 'Unavailable' }}
        </v-chip>
      </template>

      <!-- Created At formatted -->
      <template #item.createdAt="{ item }">
        {{ new Date(item.createdAt).toLocaleDateString() }}
      </template>

      <!-- Actions with tooltips -->
      <template #item.actions="{ item }">
        <!-- Past Rentals Button -->
        <v-tooltip text="Past Rentals" location="top">
          <template #activator="{ props }">
            <v-btn
              icon
              size="small"
              color="info darken-2"
              @click="viewPastRentals(item)"
              v-bind="props"
              aria-label="View past rentals"
            >
              <v-icon>mdi-history</v-icon>
            </v-btn>
          </template>
        </v-tooltip>

        <v-tooltip text="Edit" location="top">
          <template #activator="{ props }">
            <v-btn
              icon
              size="small"
              color="blue darken-2"
              @click="editEquipment(item)"
              v-bind="props"
              aria-label="Edit equipment"
            >
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
          </template>
        </v-tooltip>

        <v-tooltip text="Delete" location="top">
          <template #activator="{ props }">
            <v-btn
              icon
              size="small"
              color="red darken-2"
              @click="deleteEquipmentHandler(item.id)"
              v-bind="props"
              aria-label="Delete equipment"
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
import { ref, onBeforeMount } from 'vue'
import { useRouter } from 'vue-router'
import AddEquipmentDialog from '@/components/dialog/equipment/AddEquipmentDialog.vue'
import { getAvailableEquipment } from '@/api/EquipmentController'
import { useAuthenticationStore } from '@/stores/Authentication'

const useAuthStore = useAuthenticationStore()

const router = useRouter()

const isAdmin = ref(useAuthStore.authRole === 'Admin')
const isAddEquipmentDialogOpen = ref(false)
const selectedEquipment = ref(null)

const headers = [
  { title: 'Name', value: 'name' },
  { title: 'Category', value: 'category' },
  { title: 'Condition', value: 'condition' },
  { title: 'Rental Price', value: 'rentalPrice' },
  { title: 'Available', value: 'isAvailable' },
  { title: 'Created At', value: 'createdAt' },
  ...(isAdmin.value ? [{ title: 'Actions', value: 'actions', sortable: false }] : []),
]

const equipment = ref([])

const loadEquipment = async () => {
  try {
    const equipments = await getAvailableEquipment()
    if (equipments) equipment.value = equipments
  } catch (error) {
    console.error('Failed to fetch equipment:', error)
  }
}

onBeforeMount(loadEquipment)

// ✅ Navigate to Past Rentals page
const viewPastRentals = (item) => {
  router.push({
    name: 'EquipmentRentalHistory',
    params: { id: item.id },
  })
}

const editEquipment = (item) => {
  selectedEquipment.value = item
  isAddEquipmentDialogOpen.value = true
}

const cleanup = () => {
  selectedEquipment.value = null
}

const refreshEquipments = () => {
  loadEquipment()
}

const deleteEquipmentHandler = async (id) => {
  if (confirm('Are you sure you want to delete this equipment?')) {
    try {
      await deleteEquipment(id)
      await loadEquipment()
    } catch (error) {
      console.error('Failed to delete equipment:', error)
    }
  }
}
</script>

<style scoped>
.text-decoration-none {
  text-decoration: none;
}

.font-weight-medium {
  font-weight: 500;
}
</style>
