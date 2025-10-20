<template>
  <v-dialog max-width="500" v-model="isOpen">
    <template v-slot:activator="{ props: activatorProps }">
      <v-btn color="primary" v-bind="activatorProps">
        <v-icon start>mdi-plus</v-icon>
        Add Equipment
      </v-btn>
    </template>

    <template v-slot:default="{ isActive }">
      <EquipmentForm :equipment="equipment" @customer-saved="handleSave" />
    </template>
  </v-dialog>
</template>

<script setup>
import { computed, watch } from 'vue'
import { defineProps, defineEmits } from 'vue'
import EquipmentForm from '@/components/forms/EquipmentForm.vue'

const props = defineProps({
  modelValue: Boolean,
  equipment: Object,
})

const emit = defineEmits(['update:modelValue', 'saved', 'closed'])

const isOpen = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val),
})

const handleSave = () => {
  emit('saved')
  isOpen.value = false
}

const equipment = computed(() => props.equipment)

watch(isOpen, (newValue, oldValue) => {
  if (!newValue && oldValue) {
    emit('closed')
  }
})
</script>
