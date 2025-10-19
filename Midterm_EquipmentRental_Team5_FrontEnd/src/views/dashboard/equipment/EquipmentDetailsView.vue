<template>
  <v-container>
    <v-row dense>
      <!-- Equipment Image -->
      <v-col cols="12" md="4">
        <v-img
          :src="
            equipment.image ||
            'https://m.media-amazon.com/images/I/61UEK6ShB2L._UF1000,1000_QL80_.jpg'
          "
          alt="Equipment Image"
          height="240"
          class="rounded-lg elevation-2"
          cover
        />
      </v-col>

      <!-- Equipment Details -->
      <v-col cols="12" md="8">
        <v-card class="rounded-lg elevation-2">
          <v-divider />

          <v-card-text class="pt-4">
            <v-row dense>
              <v-col cols="12" sm="6">
                <div class="mb-2"><strong>Category:</strong> {{ equipment.category || '—' }}</div>
                <div class="mb-2"><strong>Condition:</strong> {{ equipment.condition || '—' }}</div>
              </v-col>

              <v-col cols="12" sm="6">
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

onBeforeMount(async () => {
  equipment.value = await getEquipment(equipmentId)
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
