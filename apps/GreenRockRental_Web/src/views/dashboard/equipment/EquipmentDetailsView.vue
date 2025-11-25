<template>
  <v-container>
    <v-row dense>
      <!-- Equipment Image -->
      <v-col cols="12" md="4">
        <v-img
          :src="getImageUrl(equipment.imageUrl)"
          alt="Equipment Image"
          height="240"
          class="rounded-lg elevation-2"
          cover
        >
          <template v-slot:placeholder>
            <v-row class="fill-height ma-0" align="center" justify="center">
              <v-progress-circular indeterminate color="grey-lighten-5" />
            </v-row>
          </template>
        </v-img>
      </v-col>

      <!-- Equipment Details -->
      <v-col cols="12" md="8">
        <v-card class="rounded-lg elevation-2">
          <v-card-title class="text-h5">
            {{ equipment.name || 'Equipment Details' }}
          </v-card-title>

          <v-divider />

          <v-card-text class="pt-4">
            <div class="mb-3">
              <strong>Description:</strong>
              {{ equipment.description || '—' }}
            </div>

            <v-row dense>
              <v-col cols="12" sm="6">
                <div class="mb-2"><strong>Category:</strong> {{ equipment.category || '—' }}</div>
                <div class="mb-2"><strong>Condition:</strong> {{ equipment.condition || '—' }}</div>
              </v-col>

              <v-col cols="12" sm="6">
                <div class="mb-2">
                  <strong>Rental Price:</strong> ${{ equipment.rentalPrice || '0' }}/day
                </div>
                <div class="mb-2 d-flex align-center">
                  <strong class="me-2">Status:</strong>
                  <v-chip :color="statusColor(status)" dark size="small">
                    {{ status }}
                  </v-chip>
                </div>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { ref, onBeforeMount, computed } from 'vue'
import { useRoute } from 'vue-router'
import { getEquipment } from '@/api/EquipmentController'

const route = useRoute()
const equipmentId = route.params.id

const equipment = ref({})

// ✅ Add this function to get the full image URL
const getImageUrl = (imageUrl) => {
  if (!imageUrl) {
    // Return placeholder if no image
    return '/images/equipment/placeholder.png'
  }
  // If your API is on a different port (like localhost:5115)
  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5115'
  return `${apiBaseUrl}${imageUrl}`
}

onBeforeMount(async () => {
  try {
    const response = await getEquipment(equipmentId)
    equipment.value = response.data || response
    console.log('Equipment loaded:', equipment.value)
  } catch (error) {
    console.error('Error loading equipment:', error)
  }
})

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
</script>

<style scoped>
.v-img {
  object-fit: cover;
}

.v-btn {
  transition: 0.2s ease-in-out;
}
</style>
