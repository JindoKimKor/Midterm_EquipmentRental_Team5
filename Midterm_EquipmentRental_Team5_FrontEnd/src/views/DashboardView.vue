<template>
  <NavigationDrawer />
  <v-container class="d-flex flex-column">
    <RouterView />
  </v-container>
</template>

<script setup>
import NavigationDrawer from '@/components/navigations/NavigationDrawer.vue'
</script>
