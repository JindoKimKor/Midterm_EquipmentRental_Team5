<template>
  <v-dialog max-width="500" v-model="isOpen">
    <template v-slot:activator="{ props: activatorProps }">
      <v-btn color="primary" v-bind="activatorProps">
        <v-icon start>mdi-account-plus</v-icon>
        Add Customer
      </v-btn>
    </template>

    <template v-slot:default="{ isActive }">
      <v-card title="Customer Form">
        <!--LISTEN TO THE FORM'S EVENT HERE -->
        <CustomerForm
          :customer="props.customer"
          @customer-saved="handleSave"
        />
      </v-card>
    </template>
  </v-dialog>
</template>

<script setup>
import { computed } from 'vue'
import { defineProps, defineEmits } from 'vue'
import CustomerForm from '../forms/CustomerForm.vue'

const emit = defineEmits(['update:modelValue', 'saved'])

const props = defineProps({
  modelValue: Boolean,
  customer: Object,
})

const isOpen = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val),
})

// function now gets called when the form emits 'customer-saved'
const handleSave = () => {
  emit('saved') // Tell the parent table to refresh
  isOpen.value = false // Close the dialog
}
</script>
