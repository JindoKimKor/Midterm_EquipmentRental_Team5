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
        <CustomerForm :customer="props.customer" />
      </v-card>
    </template>
  </v-dialog>
</template>

<script setup>
import { computed, onBeforeMount } from 'vue'
import { defineProps, defineEmits } from 'vue'
import CustomerForm from '../forms/CustomerForm.vue'

const emit = defineEmits(['update:modelValue'])

const props = defineProps({
  modelValue: Boolean,
  customer: Object,
})

const isOpen = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val),
})
</script>
