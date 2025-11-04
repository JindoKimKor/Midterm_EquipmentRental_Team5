<template>
  <v-container class="d-flex justify-center align-center" style="height: 100vh">
    <v-card class="pa-6" max-width="800" width="100%">
      <v-card-title class="text-h5">Login</v-card-title>
      <v-card-text>
        <v-form v-model="valid" @submit.prevent="loginHandler">
          <v-text-field
            v-model="userName"
            label="Username"
            type="text"
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

          <!-- Divider -->
          <div class="text-center my-4">OR</div>

          <!-- Google Sign-In Button -->
          <v-btn color="white" class="mt-2" block @click="googleLogin">
            <v-icon start color="red">mdi-google</v-icon>
            Sign in with Google
          </v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref } from 'vue'
import { googleAuthentication, login } from '@/api/AuthController'
import router from '@/router'

const userName = ref('')
const password = ref('')
const valid = ref(false)

const rules = {
  required: (value) => !!value || 'Required.',
}

async function loginHandler() {
  try {
    await login({
      Username: userName.value,
      Password: password.value,
    })
    router.push('/dashboard')
  } catch (error) {
    console.error('Login failed:', error)
    alert('Login failed. Please check your credentials and try again.')
  }
}

const googleLogin = () => {
  googleAuthentication()
}
</script>
