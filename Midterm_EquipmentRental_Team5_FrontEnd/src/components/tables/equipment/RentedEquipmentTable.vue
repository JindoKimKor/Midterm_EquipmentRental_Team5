<template>
  <v-container fluid>
    <v-card class="mb-6">
      <v-card-title>
        Rentals
        <v-spacer />
        <v-btn color="primary" @click="goToIssueForm">Issue Equipment</v-btn>
      </v-card-title>

      <v-data-table
        :headers="headers"
        :items="rentals"
        item-value="id"
        :items-per-page="10"
        class="elevation-1"
        :item-class="getRowClass"
      >
        <!-- Format Issue Date -->
        <template #item.issueDate="{ item }">
          {{ formatDate(item.issueDate) }}
        </template>

        <!-- Rental Status -->
        <template #item.status="{ item }">
          <v-chip :color="getStatusColor(item)" dark small>
            {{ getStatusLabel(item) }}
          </v-chip>
        </template>

        <!-- Actions -->
        <template #item.actions="{ item }">
          <v-btn icon @click="viewDetails(item.id)">
            <v-icon>mdi-eye</v-icon>
          </v-btn>

          <v-btn v-if="isAdmin && !item.returnedAt" icon color="red" @click="forceReturn(item.id)">
            <v-icon>mdi-backup-restore</v-icon>
          </v-btn>

          <v-btn
            v-if="isAdmin && !item.returnedAt"
            icon
            color="orange"
            @click="cancelRental(item.id)"
          >
            <v-icon>mdi-cancel</v-icon>
          </v-btn>
        </template>
      </v-data-table>
    </v-card>

    <!-- Snackbar -->
    <v-snackbar v-model="snackbar.show" :color="snackbar.color" timeout="3000">
      {{ snackbar.message }}
    </v-snackbar>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

// Simulate role-based access
const isAdmin = true // Replace with auth state logic
const currentUserId = 2 // Replace with current user ID

const rentals = ref([])
const router = useRouter()

const snackbar = ref({
  show: false,
  message: '',
  color: '',
})

// Table headers
const headers = [
  { title: 'Equipment', value: 'equipmentName' },
  { title: 'Customer', value: 'customerName' },
  { title: 'Issue Date', value: 'issueDate' },
  { title: 'Status', value: 'status' },
  { title: 'Actions', value: 'actions', sortable: false },
]

// --- Actions ---

const fetchRentals = async () => {
  try {
    const url = isAdmin ? '/api/rentals' : `/api/customers/${currentUserId}/rentals`
    const res = await fetch(url)
    if (!res.ok) throw new Error('Failed to fetch rentals')
    const data = await res.json()
    rentals.value = data
  } catch (err) {
    snackbar.value = { show: true, message: 'Failed to load rentals', color: 'error' }
  }
}

const forceReturn = async (id) => {
  if (!confirm('Force return this rental?')) return
  try {
    const res = await fetch(`/api/rentals/${id}`, {
      method: 'DELETE',
    })
    if (!res.ok) throw new Error('Failed to force return')
    snackbar.value = { show: true, message: 'Rental forcibly returned', color: 'success' }
    fetchRentals()
  } catch (err) {
    snackbar.value = { show: true, message: 'Error processing request', color: 'error' }
  }
}

const cancelRental = async (id) => {
  if (!confirm('Cancel this rental?')) return
  try {
    const res = await fetch(`/api/rentals/${id}`, {
      method: 'DELETE',
    })
    if (!res.ok) throw new Error('Failed to cancel rental')
    snackbar.value = { show: true, message: 'Rental canceled', color: 'info' }
    fetchRentals()
  } catch (err) {
    snackbar.value = { show: true, message: 'Cancel failed', color: 'error' }
  }
}

const viewDetails = (id) => {
  router.push(`/dashboard/rentals/${id}`)
}

const goToIssueForm = () => {
  router.push(`/dashboard/rental/issue`)
}

// --- Helpers ---

const formatDate = (date) => {
  return new Date(date).toLocaleDateString()
}

const getStatusLabel = (item) => {
  if (item.returnedAt) return 'Returned'
  if (isOverdue(item)) return 'Overdue'
  return 'Active'
}

const getStatusColor = (item) => {
  if (item.returnedAt) return 'green'
  if (isOverdue(item)) return 'red'
  return 'orange'
}

const isOverdue = (item) => {
  const now = new Date()
  const due = new Date(item.dueDate)
  return due < now && !item.returnedAt
}

const getRowClass = (item) => {
  return isOverdue(item) && !item.returnedAt ? 'bg-red-lighten-5' : ''
}

onMounted(fetchRentals)
</script>

<style scoped>
.bg-red-lighten-5 {
  background-color: #ffebee !important;
}
</style>
