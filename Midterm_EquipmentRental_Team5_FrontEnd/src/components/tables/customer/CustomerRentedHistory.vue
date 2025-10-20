<template>
  <v-data-table :headers="headers" :items="rentals" class="elevation-1">
    <!-- Custom rendering for equipment name -->
    <template #item.equipment="{ item }">
      {{ item.equipment?.name || 'Unknown' }}
    </template>

    <template #item.issuedAt="{ item }">
      {{ formatDate(item.issuedAt) }}
    </template>

    <template #item.dueDate="{ item }">
      {{ formatDate(item.dueDate) }}
    </template>

    <template #item.returnedAt="{ item }">
      {{ item.returnedAt ? formatDate(item.returnedAt) : 'Not Returned' }}
    </template>

    <template #item.isActive="{ item }">
      <v-chip :color="item.isActive ? 'green' : 'grey'" dark>
        {{ item.isActive ? 'Active' : 'Completed' }}
      </v-chip>
    </template>

    <template #item.overdueFee="{ item }">
      {{ item.overdueFee ? `$${item.overdueFee.toFixed(2)}` : 'None' }}
    </template>
  </v-data-table>
</template>

<script setup>
import { getCustomerRentalHistory } from '@/api/CustomerController'
import { ref, onMounted } from 'vue'

const props = defineProps({
  customerId: Boolean,
})

const headers = [
  { title: 'Equipment', value: 'equipment' },
  { title: 'Issued At', value: 'issuedAt' },
  { title: 'Due Date', value: 'dueDate' },
  { title: 'Returned At', value: 'returnedAt' },
  { title: 'Status', value: 'isActive' },
  { title: 'Overdue Fee', value: 'overdueFee' },
]

const rentals = ref([])

onMounted(async () => {
  const res = await getCustomerRentalHistory(props.customerId)
  if (res) rentals.value = res
})

function formatDate(dateStr) {
  const date = new Date(dateStr)
  return date.toLocaleDateString()
}
</script>
