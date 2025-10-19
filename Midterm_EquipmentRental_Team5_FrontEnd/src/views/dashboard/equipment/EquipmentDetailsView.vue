<template>
  <v-container>
    <v-row>
      <!-- Equipment Image -->
      <v-col cols="12" md="4">
        <v-img
          :src="equipment.image || 'https://via.placeholder.com/300x200?text=No+Image'"
          alt="Equipment Image"
          height="200"
          class="rounded"
        />
      </v-col>

      <!-- Equipment Info -->
      <v-col cols="12" md="8">
        <v-card>
          <v-card-title>
            <span class="text-h6">{{ equipment.name }}</span>
            <v-spacer />
            <div v-if="isAdmin">
              <v-btn icon color="blue" @click="editEquipment">
                <v-icon>mdi-pencil</v-icon>
              </v-btn>
              <v-btn icon color="red" @click="deleteEquipment">
                <v-icon>mdi-delete</v-icon>
              </v-btn>
            </div>
          </v-card-title>

          <v-card-text>
            <v-list dense>
              <v-list-item>
                <v-list-item-title>
                  <strong>Category:</strong> {{ equipment.category }}
                </v-list-item-title>
              </v-list-item>

              <v-list-item>
                <v-list-item-title>
                  <strong>Condition:</strong> {{ equipment.condition }}
                </v-list-item-title>
              </v-list-item>

              <v-list-item>
                <v-list-item-title>
                  <strong>Status:</strong>
                  <v-chip :color="statusColor(status)" dark>
                    {{ status }}
                  </v-chip>
                </v-list-item-title>
              </v-list-item>

              <v-list-item v-if="isAdmin">
                <v-list-item-title>
                  <!--
                  <router-link
                    :to="{ name: 'EquipmentHistory', params: { equipmentId: equipment.id } }"
                  >
                    View Rental History
                  </router-link>
                  -->
                </v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>

          <v-card-actions v-if="isUser && status === 'Available'">
            <v-btn color="green" @click="issueEquipment">
              <v-icon start>mdi-hand-extended</v-icon>
              Issue Equipment
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { onBeforeMount, ref, computed } from 'vue'
import { useRoute } from 'vue-router'
import { getEquipment } from '@/api/EquipmentController'

const route = useRoute()
const equipmentId = route.params.id

const isAdmin = true
const isUser = !isAdmin

const equipment = ref({})

onBeforeMount(async () => {
  equipment.value = await getEquipment(equipmentId)
})

// Derive status string from isAvailable boolean
const status = computed(() => {
  return equipment.value.isAvailable ? 'Available' : 'Unavailable'
})

const statusColor = (status) => {
  switch (status.toLowerCase()) {
    case 'available':
      return 'green'
    case 'rented':
      return 'blue'
    case 'maintenance':
      return 'orange'
    case 'unavailable':
      return 'grey'
    default:
      return 'grey'
  }
}

const editEquipment = () => {
  console.log('Navigate to edit page for equipment ID:', equipment.value.id)
}

const deleteEquipment = () => {
  if (confirm('Delete this equipment?')) {
    console.log('Deleting equipment', equipment.value.id)
    // Add your deletion logic here
  }
}

const issueEquipment = () => {
  console.log('Issue this equipment', equipment.value.id)
  // Add your issue logic here
}
</script>
