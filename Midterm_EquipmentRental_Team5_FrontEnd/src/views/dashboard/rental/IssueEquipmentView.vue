<template>
  <v-container fluid class="pa-4" style="max-width: 800px">
    <v-card elevation="3" rounded>
      <v-toolbar color="primary" dark flat>
        <v-toolbar-title>Issue Equipment</v-toolbar-title>
      </v-toolbar>

      <v-card-text>
        <IssueRentalForm v-if="role === 'Admin'" />
        <CustomerIssueRentalForm v-if="role === 'User'" />
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import CustomerIssueRentalForm from '@/components/forms/CustomerIssueRentalForm.vue'
import IssueRentalForm from '@/components/forms/IssueRentalForm.vue'
import { useUserInformationStore } from '@/stores/UserInformation'
import { ref } from 'vue'

const userInfoStore = useUserInformationStore()

const role = ref(userInfoStore.role)
</script>
