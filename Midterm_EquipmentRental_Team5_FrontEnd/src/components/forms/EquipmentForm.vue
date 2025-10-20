<template>
  <v-container class="pa-4">
    <v-card>
      <v-card-title>Equipment Form</v-card-title>
      <v-card-text>
        <v-form ref="form" v-model="valid" @submit.prevent="submitForm">
          <v-text-field v-model="equipment.Name" label="Name" :rules="[rules.required]" required />

          <v-textarea
            v-model="equipment.Description"
            label="Description"
            rows="3"
            :rules="[rules.required]"
            required
          />

          <v-text-field
            v-model="equipment.Category"
            label="Category"
            :rules="[rules.required]"
            required
          />

          <v-text-field
            v-model="equipment.Condition"
            label="Condition"
            :rules="[rules.required]"
            required
          />

          <v-text-field
            v-model.number="equipment.RentalPrice"
            label="Rental Price"
            type="number"
            :rules="[rules.required, rules.positive]"
            required
          />

          <v-switch v-model="equipment.IsAvailable" label="Is Available" inset />

          <v-text-field
            v-model="equipment.CreatedAt"
            label="Created At"
            type="date"
            :rules="[rules.required]"
            required
          />

          <v-btn type="submit" color="primary" class="mt-4" :disabled="!valid"> Submit </v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { onBeforeMount, ref } from 'vue'
import { addEquipment } from '@/api/EquipmentController'

const valid = ref(false)

const props = defineProps({
  equipment: Object,
})

const equipment = ref({})

onBeforeMount(() => {
  equipment.value = {
    Id: props.equipment?.id ?? null,
    Name: props.equipment?.name ?? '',
    Description: props.equipment?.description ?? '',
    Category: props.equipment?.category ?? '',
    Condition: props.equipment?.condition ?? '',
    RentalPrice: props.equipment?.rentalPrice ?? 0,
    IsAvailable: props.equipment?.isAvailable ?? true,
    CreatedAt: props.equipment?.createdAt.slice(0, 10) ?? new Date().toISOString().slice(0, 10),
  }
})

const rules = {
  required: (v) => !!v || 'This field is required',
  positive: (v) => v >= 0 || 'Must be a positive number',
}

const submitForm = async () => {
  if (!valid.value) return

  try {
    const payload = { ...equipment.value }

    await addEquipment(payload)
    alert('Equipment successfully submitted!')

    resetForm()
  } catch (error) {
    console.error('Submission failed:', error)
    alert('Submission failed. Please try again.')
  }
}

function resetForm() {
  equipment.value = {
    id: 0,
    name: '',
    description: '',
    category: '',
    condition: '',
    rentalPrice: 0,
    isAvailable: true,
    createdAt: new Date().toISOString().slice(0, 10),
  }
}
</script>
