<template>
  <v-container fluid class="pa-4">
    <!-- Back Button -->
    <v-btn
      class="mb-4"
      prepend-icon="mdi-arrow-left"
      variant="text"
      @click="goBack"
    >
      Back to Equipment
    </v-btn>

    <!-- Equipment Info Card -->
    <v-card v-if="equipment" class="mb-6" elevation="2">
      <v-card-title class="d-flex align-center justify-space-between bg-primary">
        <div class="text-h5 text-white">
          <v-icon class="mr-2">mdi-tools</v-icon>
          {{ equipment.name }}
        </div>
        <v-chip color="white" variant="elevated">
          {{ equipment.category }}
        </v-chip>
      </v-card-title>

      <v-card-text class="pt-4">
        <v-row dense>
          <v-col cols="12" sm="3">
            <div class="text-caption text-grey">Condition</div>
            <div class="text-body-1 font-weight-medium">{{ equipment.condition }}</div>
          </v-col>
          <v-col cols="12" sm="3">
            <div class="text-caption text-grey">Rental Price</div>
            <div class="text-body-1 font-weight-medium">${{ equipment.rentalPrice }}/day</div>
          </v-col>
          <v-col cols="12" sm="3">
            <div class="text-caption text-grey">Status</div>
            <v-chip
              :color="equipment.isAvailable ? 'success' : 'error'"
              size="small"
            >
              {{ equipment.isAvailable ? 'Available' : 'Rented' }}
            </v-chip>
          </v-col>
          <v-col cols="12" sm="3">
            <div class="text-caption text-grey">Total Rentals</div>
            <div class="text-body-1 font-weight-medium">{{ rentals.length }}</div>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>

    <!-- Past Rentals Table -->
    <v-card elevation="2">
      <v-card-title class="d-flex align-center">
        <v-icon class="mr-2" color="primary">mdi-history</v-icon>
        <span class="text-h6">Past Rentals</span>
      </v-card-title>

      <v-divider />

      <v-data-table
        :headers="headers"
        :items="rentals"
        :loading="loading"
        class="elevation-0"
        :items-per-page="10"
      >
        <!-- Customer Column -->
        <template v-slot:item.customer="{ item }">
          <div class="d-flex align-center py-2">
            <v-avatar color="primary" size="36" class="mr-3">
              <span class="text-white text-body-2">{{ getInitials(item.customer?.name) }}</span>
            </v-avatar>
            <div>
              <div class="font-weight-medium">{{ item.customer?.name || 'N/A' }}</div>
              <div class="text-caption text-grey">{{ item.customer?.email || '' }}</div>
            </div>
          </div>
        </template>

        <!-- Issued At Column -->
        <template v-slot:item.issuedAt="{ item }">
          <div>
            <div class="font-weight-medium">{{ formatDate(item.issuedAt) }}</div>
            <div class="text-caption text-grey">{{ formatTime(item.issuedAt) }}</div>
          </div>
        </template>

        <!-- Due Date Column -->
        <template v-slot:item.dueDate="{ item }">
          <div>
            <div class="font-weight-medium">{{ formatDate(item.dueDate) }}</div>
            <div class="text-caption text-grey">{{ formatTime(item.dueDate) }}</div>
          </div>
        </template>

        <!-- Returned At Column -->
        <template v-slot:item.returnedAt="{ item }">
          <div v-if="item.returnedAt">
            <div class="font-weight-medium">{{ formatDate(item.returnedAt) }}</div>
            <div class="text-caption text-grey">{{ formatTime(item.returnedAt) }}</div>
          </div>
          <v-chip v-else color="warning" size="small" variant="flat">
            Not Returned
          </v-chip>
        </template>

        <!-- Duration Column -->
        <template v-slot:item.duration="{ item }">
          <v-chip size="small" variant="tonal">
            {{ calculateDuration(item) }}
          </v-chip>
        </template>

        <!-- Status Column -->
        <template v-slot:item.status="{ item }">
          <v-chip
            :color="getStatusColor(item)"
            size="small"
            variant="elevated"
          >
            {{ getStatus(item) }}
          </v-chip>
        </template>

        <!-- Overdue Fee Column -->
        <template v-slot:item.overdueFee="{ item }">
          <span v-if="item.overdueFee && item.overdueFee > 0" class="text-error font-weight-bold">
            ${{ item.overdueFee.toFixed(2) }}
          </span>
          <span v-else class="text-success font-weight-medium">
            $0.00
          </span>
        </template>
      </v-data-table>

      <!-- Empty State -->
      <v-card-text v-if="!loading && rentals.length === 0">
        <v-alert type="info" variant="tonal">
          <v-alert-title>No Rental History</v-alert-title>
          This equipment has never been rented.
        </v-alert>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getEquipment, getEquipmentRentalHistory } from '@/api/EquipmentController'

const route = useRoute()
const router = useRouter()
const equipmentId = route.params.id

const equipment = ref(null)
const rentals = ref([])
const loading = ref(false)

// ✅ Removed Actions column from headers
const headers = [
  { title: 'Customer', key: 'customer', sortable: true },
  { title: 'Issued', key: 'issuedAt', sortable: true },
  { title: 'Due Date', key: 'dueDate', sortable: true },
  { title: 'Returned', key: 'returnedAt', sortable: true },
  { title: 'Duration', key: 'duration', sortable: false },
  { title: 'Status', key: 'status', sortable: true },
  { title: 'Overdue Fee', key: 'overdueFee', sortable: true, align: 'end' }
]

onMounted(() => {
  loadData()
})

const loadData = async () => {
  loading.value = true
  try {
    // Load equipment details
    const equipRes = await getEquipment(equipmentId)
    equipment.value = equipRes?.data || equipRes

    // Load rental history
    const rentalRes = await getEquipmentRentalHistory(equipmentId)
    rentals.value = rentalRes?.data || rentalRes || []

    console.log('Equipment:', equipment.value)
    console.log('Rentals:', rentals.value)
  } catch (error) {
    console.error('Error loading data:', error)
  } finally {
    loading.value = false
  }
}

const goBack = () => {
  router.push({ name: 'EquipmentDashboard' })
}

const getInitials = (name) => {
  if (!name) return '?'
  return name
    .split(' ')
    .map(word => word[0])
    .join('')
    .toUpperCase()
    .substring(0, 2)
}

const formatDate = (date) => {
  if (!date) return 'N/A'
  return new Date(date).toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric'
  })
}

const formatTime = (date) => {
  if (!date) return ''
  return new Date(date).toLocaleTimeString('en-US', {
    hour: '2-digit',
    minute: '2-digit'
  })
}

const calculateDuration = (rental) => {
  const start = new Date(rental.issuedAt)
  const end = rental.returnedAt ? new Date(rental.returnedAt) : new Date()
  const days = Math.ceil((end - start) / (1000 * 60 * 60 * 24))
  return `${days} day${days !== 1 ? 's' : ''}`
}

const getStatus = (rental) => {
  if (rental.returnedAt) return 'Completed'
  if (rental.isActive && new Date(rental.dueDate) < new Date()) return 'Overdue'
  if (rental.isActive) return 'Active'
  return 'Unknown'
}

const getStatusColor = (rental) => {
  if (rental.returnedAt) return 'success'
  if (rental.isActive && new Date(rental.dueDate) < new Date()) return 'error'
  if (rental.isActive) return 'primary'
  return 'grey'
}
</script>
