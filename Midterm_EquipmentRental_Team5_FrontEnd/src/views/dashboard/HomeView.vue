<template>
  <v-container fluid class="landing-page pa-0">
    <v-img
      src="https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=2340"
      height="350"
      cover
    >
      <!-- Dark overlay -->
      <div class="overlay">
        <v-row class="fill-height" align="center" justify="center">
          <v-col class="text-center text-white">
            <h1 class="display-2 font-weight-bold text-shadow">Welcome back, {{ userName }}!</h1>
            <p class="text-h6 text-shadow">
              You're logged in as a <strong>{{ userRole }}</strong>
            </p>
          </v-col>
        </v-row>
      </div>
    </v-img>

    <v-container>
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
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import useAuthenicationStore from '@/stores/Authentication'
import { useUserInformationStore } from '@/stores/UserInformation'

const router = useRouter()
const authStore = useAuthenicationStore()
const userStore = useUserInformationStore()

const userRole = ref(authStore.authRole)
const userName = ref(userStore.name)

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
    title: 'User Profile',
    description: 'Find user infromation and rental information',
    icon: 'mdi-magnify',
    route: `/dashboard/customers/${userStore.id}`,
  },
  {
    title: 'My Rentals',
    description: 'Current rental information',
    icon: 'mdi-clock-outline',
    route: `/dashboard/customers/${userStore.id}/rentals`,
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
.overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.45); /* adjustable opacity */
  z-index: 1;
  pointer-events: none;
}

.v-img > .v-row {
  position: relative;
  z-index: 2;
}

.text-shadow {
  text-shadow: 1px 2px 4px rgba(0, 0, 0, 0.6);
}
</style>
