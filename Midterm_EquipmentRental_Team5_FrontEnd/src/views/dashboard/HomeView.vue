<template>
  <v-container fluid class="landing-page pa-0">
    <!-- Hero Section -->
    <v-parallax
      height="350"
      src="https://images.unsplash.com/photo-1603570412141-5c38367c0dfd?auto=format&fit=crop&w=1350&q=80"
    >
      <v-row class="fill-height" align="center" justify="center">
        <v-col class="text-center text-white">
          <h1 class="display-2 font-weight-bold">Welcome back, {{ userName }}!</h1>
          <p class="text-h6">
            You're logged in as a <strong>{{ userRole }}</strong>
          </p>
        </v-col>
      </v-row>
    </v-parallax>

    <!-- Quick Actions -->
    <v-container>
      <v-row justify="center" class="mb-8">
        <v-col cols="12" md="8" class="text-center">
          <h2 class="text-h4 font-weight-bold">Quick Actions</h2>
          <p class="text-subtitle-1">Get started quickly with your daily tasks</p>
        </v-col>
      </v-row>

      <!-- Admin Actions -->
      <v-row v-if="userRole === 'Admin'" justify="center" class="text-center">
        <v-col cols="12" sm="6" md="4" v-for="card in adminCards" :key="card.title">
          <v-card class="pa-4 hoverable" elevation="4">
            <v-icon size="48" color="primary">{{ card.icon }}</v-icon>
            <h3 class="text-h6 mt-2">{{ card.title }}</h3>
            <p class="text-body-2 mb-4">{{ card.description }}</p>
            <v-btn color="primary" @click="goTo(card.route)">Go</v-btn>
          </v-card>
        </v-col>
      </v-row>

      <!-- Customer Actions -->
      <v-row v-else justify="center" class="text-center">
        <v-col cols="12" sm="6" md="5" v-for="card in customerCards" :key="card.title">
          <v-card class="pa-4 hoverable" elevation="4">
            <v-icon size="48" color="teal-darken-2">{{ card.icon }}</v-icon>
            <h3 class="text-h6 mt-2">{{ card.title }}</h3>
            <p class="text-body-2 mb-4">{{ card.description }}</p>
            <v-btn color="teal-darken-2" @click="goTo(card.route)">Go</v-btn>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import useAuthenicationStore from '@/stores/Authentication'

const router = useRouter()
const authStore = useAuthenicationStore()

const userRole = ref(authStore.authRole)

onMounted(() => {})

// Action Cards
const adminCards = [
  {
    title: 'Manage Equipment',
    description: 'Add, update, or remove rental equipment.',
    icon: 'mdi-hammer-wrench',
    route: '/dashboard/equipments',
  },
  {
    title: 'Manage Customers',
    description: 'View and manage customer accounts.',
    icon: 'mdi-account-group',
    route: '/dashboard/customers',
  },
  {
    title: 'Manage Rentals',
    description: 'Track issued and returned equipment.',
    icon: 'mdi-clipboard-list',
    route: '/dashboard/rentals',
  },
]

const customerCards = [
  {
    title: 'Browse Equipment',
    description: 'Find tools and machines to rent.',
    icon: 'mdi-magnify',
    route: '/equipment',
  },
  {
    title: 'My Rentals',
    description: 'View your current and past rentals.',
    icon: 'mdi-clock-outline',
    route: '/rentals',
  },
]

const goTo = (path) => {
  router.push(path)
}
</script>

<style scoped>
.landing-page {
  font-family: 'Roboto', sans-serif;
}
.v-card {
  transition: transform 0.2s ease;
}
.v-card:hover {
  transform: translateY(-5px);
}
</style>
