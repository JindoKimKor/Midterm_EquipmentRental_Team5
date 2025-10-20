<template>
  <v-dialog
    max-width="500"
    v-model="isOpen"
    :attach="true"
    scroll-strategy="block"
  >
    <template v-slot:activator="{ props: activatorProps }" v-if="showActivator">
      <v-btn
        :color="buttonColor"
        v-bind="activatorProps"
        :prepend-icon="buttonIcon"
        :variant="buttonVariant"
      >
        {{ buttonText }}
      </v-btn>
    </template>

    <template v-slot:default>
      <CustomerForm
        v-if="isOpen"
        :customer="props.customer"
        :is-edit-mode="!!props.customer?.id"
        @customer-saved="handleSave"
      />
    </template>
  </v-dialog>
</template>

<script setup>
import { computed } from 'vue'
import CustomerForm from '../forms/CustomerForm.vue'

const emit = defineEmits(['update:modelValue', 'saved'])

const props = defineProps({
  modelValue: Boolean,
  customer: Object,
  showActivator: {
    type: Boolean,
    default: true
  },
  buttonText: {
    type: String,
    default: 'Add Customer'
  },
  buttonIcon: {
    type: String,
    default: 'mdi-account-plus'
  },
  buttonColor: {
    type: String,
    default: 'primary'
  },
  buttonVariant: {
    type: String,
    default: 'elevated'
  }
})

const isOpen = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val),
})

const handleSave = () => {
  emit('saved')
  isOpen.value = false
}
</script>
