<template>
  <v-container class="pa-4">
    <v-card>
      <v-card-title>Customer Form</v-card-title>
      <v-card-text>
        <v-form ref="form" v-model="valid" @submit.prevent="submitForm">
          <v-text-field
            v-model="customer.userName"
            label="Username"
            :rules="[rules.required]"
            required
          ></v-text-field>

          <v-text-field
            v-model="customer.password"
            label="Password"
            type="password"
            :rules="[rules.required]"
            required
          ></v-text-field>

          <v-text-field
            v-model="customer.name"
            label="Name"
            :rules="[rules.required]"
            required
          ></v-text-field>

          <v-text-field
            v-model="customer.email"
            label="Email"
            type="email"
            :rules="[rules.required, rules.email]"
            required
          ></v-text-field>

          <v-text-field
            v-model="customer.role"
            label="Role"
            :rules="[rules.required]"
            required
          ></v-text-field>

          <!-- Hidden Id field (for editing existing records) -->
          <input type="hidden" v-model="customer.id" />

          <v-btn type="submit" color="primary" class="mt-4">Submit</v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { createCustomer } from '@/api/CustomerController'
import { ref } from 'vue'

const valid = ref(false)

const customer = ref({
  id: 0,
  name: '',
  email: '',
  userName: '',
  password: '',
  role: '',
})

const rules = {
  required: (v) => !!v || 'This field is required',
  email: (v) => !v || /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v) || 'Email must be valid',
}

const submitForm = () => {
  if (valid.value) {
    createCustomer(customer.value)
  }
}
</script>
