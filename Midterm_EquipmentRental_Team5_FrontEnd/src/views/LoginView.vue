<template>
  <v-container class="d-flex justify-center align-center" style="height: 100vh">
    <v-card class="pa-6" max-width="800" width="100%">
      <v-card-title class="text-h5">Login</v-card-title>
      <v-card-text>
        <v-form v-model="valid" @submit.prevent="login">
          <v-text-field
            v-model="email"
            label="Email"
            type="email"
            :rules="[rules.required, rules.email]"
            prepend-icon="mdi-email"
            required
          />
          <v-text-field
            v-model="password"
            label="Password"
            type="password"
            :rules="[rules.required]"
            prepend-icon="mdi-lock"
            required
          />
          <v-btn class="mt-4" color="primary" type="submit" :disabled="!valid" block> Login </v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref } from 'vue'

const email = ref('')
const password = ref('')
const valid = ref(false)

const rules = {
  required: (value) => !!value || 'Required.',
  email: (value) => /.+@.+\..+/.test(value) || 'E-mail must be valid.',
}

function login() {
  alert(`Email: ${email.value}\nPassword: ${password.value}`)
}
</script>
