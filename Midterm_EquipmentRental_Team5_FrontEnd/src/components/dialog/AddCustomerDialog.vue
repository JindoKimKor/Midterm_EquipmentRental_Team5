<template>
  <v-dialog max-width="500" v-model="isOpen">
    <template v-slot:activator="{ props: activatorProps }">
      <v-btn color="primary" v-bind="activatorProps">
        <v-icon start>mdi-account-plus</v-icon>
        Add Customer
      </v-btn>
    </template>

    <template v-slot:default="{ isActive }">
      <v-card title="Dialog">
        <CustomerForm />
      </v-card>
    </template>
  </v-dialog>
</template>

<script setup>
import { computed } from 'vue'
import { defineProps, defineEmits } from 'vue'
import CustomerForm from '../forms/CustomerForm.vue'

const props = defineProps({
  modelValue: Boolean,
})

const emit = defineEmits(['update:modelValue'])

// Two-way binding for dialog open state
const isOpen = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val),
})
</script>
