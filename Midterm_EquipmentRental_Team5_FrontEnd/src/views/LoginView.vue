<template>
  <v-container class="d-flex justify-center align-center" style="height: 100vh">
    <v-card class="pa-6" max-width="800" width="100%">
      <v-card-title class="text-h5">Login</v-card-title>
      <v-card-text>
        <v-form v-model="valid" @submit.prevent="loginHandler">
          <v-text-field
            v-model="userName"
            label="Username"
            type="Username"
            :rules="[rules.required]"
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
import useAuthenicationStore from '@/stores/Authentication'
import { login } from '@/api/AuthController'
import router from '@/router'

const authStore = useAuthenicationStore()

const userName = ref('')
const password = ref('')
const valid = ref(false)

const rules = {
  required: (value) => !!value || 'Required.',
}

async function loginHandler() {
  const { token, user } = await login({
    Username: userName.value,
    Password: password.value,
  })
  console.log(user)
  if (token) {
    authStore.setToken(token)
    authStore.setRole(user.role)
    router.push('/dashboard')
  }
}
</script>
