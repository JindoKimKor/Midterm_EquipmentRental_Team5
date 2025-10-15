<template>
  <v-navigation-drawer app permanent>
    <v-list dense nav>
      <v-subheader>Customer Management</v-subheader>

      <v-list-item
        v-if="isAdmin || isUser"
        to="/customers"
        title="Customer List"
        prepend-icon="mdi-account-group"
      />

      <v-list-item
        v-if="isAdmin"
        to="/customers/create"
        title="Add Customer"
        prepend-icon="mdi-account-plus"
      />

      <v-list-item
        v-if="isUser"
        :to="`/customers/${userId}`"
        title="My Profile"
        prepend-icon="mdi-account"
      />

      <v-list-item
        v-if="isUser"
        :to="`/customers/${userId}/rentals`"
        title="My Rentals"
        prepend-icon="mdi-history"
      />

      <v-list-item
        v-if="isUser"
        :to="`/customers/${userId}/active-rental`"
        title="Active Rental"
        prepend-icon="mdi-checkbox-marked-outline"
      />

      <v-divider class="my-2" />

      <v-subheader>Rental Management</v-subheader>

      <v-list-item
        v-if="isAdmin"
        to="/rentals"
        title="All Rentals"
        prepend-icon="mdi-clipboard-list"
      />

      <v-list-item
        v-if="isUser"
        to="/rentals/my"
        title="My Rentals"
        prepend-icon="mdi-clipboard-account"
      />

      <v-list-item
        v-if="isAdmin"
        to="/rentals/issue"
        title="Issue Equipment"
        prepend-icon="mdi-arrow-up-bold-box"
      />

      <v-list-item
        v-if="isAdmin || isUser"
        to="/rentals/return"
        title="Return Equipment"
        prepend-icon="mdi-arrow-down-bold-box"
      />

      <v-list-item
        v-if="isAdmin"
        to="/rentals/active"
        title="Active Rentals"
        prepend-icon="mdi-playlist-check"
      />

      <v-list-item
        v-if="isAdmin"
        to="/rentals/completed"
        title="Completed Rentals"
        prepend-icon="mdi-calendar-check"
      />

      <v-list-item
        v-if="isAdmin"
        to="/rentals/overdue"
        title="Overdue Rentals"
        prepend-icon="mdi-alert"
      />

      <v-list-item
        v-if="isAdmin"
        to="/rentals/equipment-history"
        title="Equipment History"
        prepend-icon="mdi-history"
      />
    </v-list>
  </v-navigation-drawer>
</template>

<script setup>
const isAdmin = true // replace with actual role check
const isUser = !isAdmin
const userId = 123 // replace with logged-in user's ID
</script>
