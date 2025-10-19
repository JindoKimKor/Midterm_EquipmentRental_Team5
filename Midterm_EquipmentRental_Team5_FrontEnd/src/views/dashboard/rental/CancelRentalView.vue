<template>
  <v-container fluid class="pa-4" style="max-width: 800px">
    <v-card elevation="3" rounded>
      <v-toolbar color="primary" dark flat>
        <v-toolbar-title>Cancel Rental</v-toolbar-title>
      </v-toolbar>
      <v-card-text>
        <CancelRentalForm />
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import CancelRentalForm from '@/components/forms/CancelRentalForm.vue'
</script>
