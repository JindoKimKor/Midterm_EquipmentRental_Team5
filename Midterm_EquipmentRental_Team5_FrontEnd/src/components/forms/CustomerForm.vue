<template>
  <v-container class="pa-4">
    <v-card>
      <v-card-title>Customer Form</v-card-title>
      <v-card-text>
        <v-form ref="form" v-model="valid" @submit.prevent="submitForm">
          <v-text-field
            v-model="customerModel.userName"
            label="Username"
            :rules="[rules.required]"
            required
          ></v-text-field>

          <v-text-field
            v-model="customerModel.password"
            label="Password"
            type="password"
            :rules="[rules.required]"
            required
          ></v-text-field>

          <v-text-field
            v-model="customerModel.name"
            label="Name"
            :rules="[rules.required]"
            required
          ></v-text-field>

          <v-text-field
            v-model="customerModel.email"
            label="Email"
            type="email"
            :rules="[rules.required, rules.email]"
            required
          ></v-text-field>

          <v-text-field
            v-model="customerModel.role"
            label="Role"
            :rules="[rules.required]"
            required
          ></v-text-field>
          <input type="hidden" v-model="customerModel.id" />
          <v-btn type="submit" color="primary" class="mt-4">Submit</v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { createCustomer, updateCustomer } from '@/api/CustomerController'
import { onBeforeMount, ref, defineEmits} from 'vue'

const emit = defineEmits(['customer-saved'])

const valid = ref(false)

const props = defineProps({
  modelValue: Boolean,
  customer: Object,
})

const customerModel = ref({
  id: 0,
  name: '',
  email: '',
  userName: '',
  password: '',
  role: '',
})

onBeforeMount(() => {
  const { customer } = props
  if (customer) {
    customerModel.value = {
      id: customer.id,
      name: customer.name,
      email: customer.email,
      userName: customer.userName,
      password: customer.password,
      role: customer.role,
    }
  } else {
    customerModel.value = {
      id: 0,
      name: '',
      email: '',
      userName: '',
      password: '',
      role: '',
    }
  }
})

const rules = {
  required: (v) => !!v || 'This field is required',
  email: (v) => !v || /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v) || 'Email must be valid',
}

const submitForm = async () => {
  if (valid.value) {
    try {
      if (customerModel.value.id) {
        // 4. Wait for the API call to finish
        await updateCustomer(customerModel.value.id, customerModel.value)
      } else {
        // 4. Wait for the API call to finish
        await createCustomer(customerModel.value)
      }
      // 5. After success, emit the event to the parent
      emit('customer-saved')
    } catch (error) {
      console.error('Failed to save the customer:', error)
      // Optionally, show an error message to the user here
    }
  }
}
</script>
