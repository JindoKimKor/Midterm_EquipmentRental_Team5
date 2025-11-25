<template>
  <v-container class="pa-4">
    <v-card>
      <v-card-title>{{ isEdit ? 'Edit Equipment' : 'Add Equipment' }}</v-card-title>

      <v-card-text>
        <v-form ref="formRef" v-model="valid" @submit.prevent="submitForm">
          <v-text-field v-model="form.Name" label="Name" :rules="[rules.required]" />

          <v-textarea
            v-model="form.Description"
            label="Description"
            rows="3"
            :rules="[rules.required]"
          />

          <v-select
            v-model="form.Category"
            label="Category"
            :items="categoryOptions"
            :rules="[rules.required]"
          />

          <v-select
            v-model="form.Condition"
            label="Condition"
            :items="conditionOptions"
            :rules="[rules.required]"
          />

          <v-text-field
            v-model.number="form.RentalPrice"
            label="Rental Price"
            type="number"
            :rules="[rules.required, rules.positive]"
          />

          <v-switch v-model="form.IsAvailable" label="Is Available" inset />

          <v-text-field
            v-model="form.CreatedAt"
            label="Created At"
            type="date"
            :rules="[rules.required]"
          />

          <v-btn type="submit" color="primary" class="mt-4" :disabled="!valid">
            {{ isEdit ? 'Update Equipment' : 'Add Equipment' }}
          </v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref, computed, onBeforeMount } from 'vue'
import { addEquipment, updateEquipment, getEquipment } from '@/api/EquipmentController'

/* ------------------------------
   Props / Emits
------------------------------ */
const props = defineProps({
  equipment: { type: Object, default: () => ({}) },
})

const emit = defineEmits(['customerSaved'])

/* ------------------------------
   Form & Validation
------------------------------ */
const valid = ref(false)
const formRef = ref(null)

const form = ref({
  Id: null,
  Name: '',
  Description: '',
  Category: '',
  Condition: '',
  RentalPrice: 0,
  IsAvailable: true,
  CreatedAt: new Date().toISOString().slice(0, 10),
})

/* Edit mode detection */
const isEdit = computed(() => !!form.value.Id)

/* Options */
const categoryOptions = ['Heavy Machinery', 'Power Tools', 'Vehicles', 'Safety', 'Surveying']
const conditionOptions = ['New', 'Excellent', 'Good', 'Fair', 'Poor']

/* ------------------------------
   Load Data (Only in Edit Mode)
------------------------------ */
onBeforeMount(async () => {
  if (props.equipment?.id) {
    const e = await getEquipment(props.equipment.id)

    form.value = {
      Id: e.id,
      Name: e.name,
      Description: e.description,
      Category: e.category,
      Condition: e.condition,
      RentalPrice: e.rentalPrice,
      IsAvailable: e.isAvailable,
      CreatedAt: e.createdAt?.slice(0, 10),
    }
  }
})

/* ------------------------------
   Validation Rules
------------------------------ */
const rules = {
  required: (v) => !!v || 'This field is required',
  positive: (v) => v >= 0 || 'Must be a positive number',
}

/* ------------------------------
   Submit Handler
------------------------------ */
const submitForm = async () => {
  if (!valid.value) return

  try {
    const payload = { ...form.value }

    if (isEdit.value) {
      await updateEquipment(payload.Id, payload)
      alert('Equipment updated successfully!')
    } else {
      await addEquipment(payload)
      alert('Equipment added successfully!')
    }

    emit('customerSaved')
    resetForm()
  } catch (error) {
    console.error(error)
    alert('Submission failed. Please try again.')
  }
}

/* ------------------------------
   Reset Form
------------------------------ */
function resetForm() {
  form.value = {
    Id: null,
    Name: '',
    Description: '',
    Category: '',
    Condition: '',
    RentalPrice: 0,
    IsAvailable: true,
    CreatedAt: new Date().toISOString().slice(0, 10),
  }
}
</script>
