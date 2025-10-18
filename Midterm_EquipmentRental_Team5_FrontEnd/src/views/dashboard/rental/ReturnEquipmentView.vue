<template>
  <v-container fluid class="pa-4" style="max-width: 800px">
    <v-card elevation="3" rounded>
      <v-toolbar color="deep-purple accent-4" dark flat>
        <v-toolbar-title>Return Rental</v-toolbar-title>
      </v-toolbar>

      <v-card-text>
        <ReturnRentalForm />
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import ReturnRentalForm from '@/components/forms/ReturnRentalForm.vue'
</script>
