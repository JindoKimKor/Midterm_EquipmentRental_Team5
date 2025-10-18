<template>
  <v-card>
    <v-card-title>
      Customer List
      <v-spacer />
      <add-customer-dialog v-model="isAddCustomerDialogOpen" />
    </v-card-title>

    <v-data-table :headers="headers" :items="customersArr" :items-per-page="10" class="elevation-1">
      <template #item.name="{ item }">
        <router-link :to="`/equipments/${item.id}`" class="text-decoration-none">
          {{ item.name }}
        </router-link>
      </template>
      <template #item.actions="{ item }">
        <v-btn icon size="small" color="blue" @click="editCustomerHandler(item)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>
        <v-btn icon size="small" color="red" @click="deleteCustomerHandler(item.id)">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import AddCustomerDialog from '../dialog/AddCustomerDialog.vue'
import {
  getAllCustomers,
  deleteCustomer,
  createCustomer,
  updateCustomer,
} from '@/api/CustomerController'

let isAddCustomerDialogOpen = ref(false)
const customersArr = ref([])

const headers = [
  { title: 'Username', value: 'userName' },
  { title: 'Password', value: 'password' },
  { title: 'Name', value: 'name' },
  { title: 'Email', value: 'email' },
  { title: 'Role', value: 'role' },
  { title: 'Actions', value: 'actions' },
]

onMounted(async () => {
  const customers = await getAllCustomers()
  customersArr.value = customers
})

const editCustomerHandler = (item) => {
  isAddCustomerDialogOpen.value = true
}

const deleteCustomerHandler = (id) => {
  if (confirm('Delete customer and rental history?')) {
    deleteCustomer(id)
  }
}
</script>
