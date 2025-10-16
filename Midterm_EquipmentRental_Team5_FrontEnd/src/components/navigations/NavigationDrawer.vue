<template>
  <v-navigation-drawer app permanent>
    <v-list dense nav>
      <v-subheader>Customer Management</v-subheader>

      <v-list-item
        to="/dashboard/customer"
        title="Customer List"
        prepend-icon="mdi-account-group"
      />

      <v-list-item
        :to="`/dashboard/customers/${userId}`"
        title="My Profile"
        prepend-icon="mdi-account"
      />

      <v-list-item
        :to="`/dashboard/customers/${userId}/rentals`"
        title="My Rentals"
        prepend-icon="mdi-history"
      />

      <v-list-item
        :to="`/dashboard/customers/${userId}/active-rental`"
        title="Active Rental"
        prepend-icon="mdi-checkbox-marked-outline"
      />

      <v-divider class="my-2" />

      <v-subheader>Customer Management</v-subheader>

      <v-divider class="my-2" />
      <v-subheader>Rental Management</v-subheader>

      <v-list-item to="/dashboard/rental" title="All Rentals" prepend-icon="mdi-clipboard-list" />

      <v-list-item
        v-if="isUser"
        to="/dashboard/rental/my"
        title="My Rentals"
        prepend-icon="mdi-clipboard-account"
      />

      <v-list-item
        to="/dashboard/rental/issue"
        title="Issue Equipment"
        prepend-icon="mdi-arrow-up-bold-box"
      />

      <v-list-item
        to="/dashboard/rental/return"
        title="Return Equipment"
        prepend-icon="mdi-arrow-down-bold-box"
      />

      <v-list-item
        to="/dashboard/rental/active"
        title="Active Rentals"
        prepend-icon="mdi-playlist-check"
      />

      <v-list-item
        to="/dashboard/rental/completed"
        title="Completed Rentals"
        prepend-icon="mdi-calendar-check"
      />

      <v-list-item
        to="/dashboard/rental/overdue"
        title="Overdue Rentals"
        prepend-icon="mdi-alert"
      />

      <v-list-item
        to="/dashboard/rental/equipment-history"
        title="Equipment History"
        prepend-icon="mdi-history"
      />
    </v-list>
  </v-navigation-drawer>
</template>

<script setup>
const isAdmin = true // TODO: Replace with actual role check (e.g., from auth store or token)
const isUser = !isAdmin
const userId = 123 // TODO: Replace with actual logged-in user's ID
</script>
