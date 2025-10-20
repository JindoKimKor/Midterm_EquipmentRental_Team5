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

          <v-select
            v-model="equipment.Category"
            label="Category"
            :items="categoryOptions"
            :rules="[rules.required]"
            required
          />

          <v-select
            v-model="equipment.Condition"
            label="Condition"
            :items="conditionOptions"
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
import { addEquipment, updateEquipment } from '@/api/EquipmentController'

const valid = ref(false)

const props = defineProps({
  equipment: Object,
})

const emit = defineEmits(['customerSaved'])

const equipment = ref({})

const categoryOptions = [
  'Heavy Machinery',
  'Power Tools',
  'Vehicles',
  'Safety',
  'Surveying'
]

const conditionOptions = [
  'New',
  'Excellent',
  'Good',
  'Fair',
  'Poor'
]

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
    if (payload.Id) {
      await updateEquipment(payload.Id, payload)
    } else {
      await addEquipment(payload)
    }

    alert('Equipment successfully submitted!')
    emit('customerSaved')
    resetForm()
  } catch (error) {
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
