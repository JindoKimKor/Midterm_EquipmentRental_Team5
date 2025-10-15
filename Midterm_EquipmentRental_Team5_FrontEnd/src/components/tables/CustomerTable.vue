<template>
  <v-card>
    <v-card-title>
      Customer List
      <v-spacer />
      <v-btn v-if="isAdmin" color="primary" @click="goToCreateCustomer">
        <v-icon start>mdi-account-plus</v-icon>
        Add Customer
      </v-btn>
    </v-card-title>

    <v-data-table :headers="headers" :items="customers" :items-per-page="10" class="elevation-1">
      <template #item.activeRental="{ item }">
        <span v-if="item.activeRental"> ✅ </span>
        <span v-else> ❌ </span>
      </template>

      <template #item.actions="{ item }">
        <v-btn icon size="small" @click="viewCustomer(item.id)">
          <v-icon>mdi-eye</v-icon>
        </v-btn>

        <v-btn v-if="isAdmin" icon size="small" color="blue" @click="editCustomer(item.id)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>

        <v-btn v-if="isAdmin" icon size="small" color="red" @click="deleteCustomer(item.id)">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref } from 'vue'

// Simulated role check
const isAdmin = true // Replace with real logic
const userId = 5 // Replace with logged-in user ID

// Table headers
const headers = [
  { title: 'Name', value: 'name' },
  { title: 'Username', value: 'username' },
  { title: 'Role', value: 'role' },
  { title: 'Active Rental', value: 'activeRental' },
  { title: 'Actions', value: 'actions', sortable: false },
]

// Dummy customer data
const customers = ref([
  {
    id: 1,
    name: 'Alice Johnson',
    username: 'alicej',
    role: 'Admin',
    activeRental: true,
  },
  {
    id: 2,
    name: 'Bob Smith',
    username: 'bobsmith',
    role: 'User',
    activeRental: false,
  },
])

// Action methods (stubbed)
const goToCreateCustomer = () => {
  console.log('Navigate to create customer form')
}

const viewCustomer = (id) => {
  console.log('Viewing customer', id)
}

const editCustomer = (id) => {
  console.log('Editing customer', id)
}

const deleteCustomer = (id) => {
  if (confirm('Delete customer and rental history?')) {
    console.log('Deleting customer', id)
  }
}
</script>
