<template>
  <v-container>
    <v-card class="pa-4">
      <v-card-title>
        <div class="text-h6">Rental Details</div>
        <v-spacer />
        <v-chip :color="statusColor(rental.status)" class="text-uppercase" dark>
          {{ rental.status }}
        </v-chip>
      </v-card-title>

      <v-divider class="my-2" />

      <v-card-text>
        <!-- Equipment Info -->
        <v-row>
          <v-col cols="12" md="6">
            <h4>Equipment</h4>
            <p><strong>Name:</strong> {{ rental.equipment.name }}</p>
            <p><strong>ID:</strong> {{ rental.equipment.id }}</p>
          </v-col>

          <!-- Customer Info -->
          <v-col cols="12" md="6">
            <h4>Customer</h4>
            <p><strong>Name:</strong> {{ rental.customer.name }}</p>
            <p><strong>Username:</strong> {{ rental.customer.username }}</p>
          </v-col>
        </v-row>

        <!-- Timestamps -->
        <v-row>
          <v-col cols="12" md="6">
            <h4>Issue Date</h4>
            <p>{{ rental.issueDate }}</p>
          </v-col>

          <v-col cols="12" md="6" v-if="rental.returnDate">
            <h4>Return Date</h4>
            <p>{{ rental.returnDate }}</p>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const rentalId = route.params.id

// Simulated data; replace with API call later
const rental = ref({
  id: rentalId,
  status: 'Returned',
  issueDate: '2025-09-01',
  returnDate: '2025-09-10',
  equipment: {
    id: 'EQUIP123',
    name: 'Nikon DSLR Camera',
  },
  customer: {
    name: 'Jane Doe',
    username: 'jdoe',
  },
})

const statusColor = (status) => {
  switch (status.toLowerCase()) {
    case 'active':
      return 'blue'
    case 'returned':
      return 'green'
    case 'overdue':
      return 'red'
    default:
      return 'grey'
  }
}
</script>
