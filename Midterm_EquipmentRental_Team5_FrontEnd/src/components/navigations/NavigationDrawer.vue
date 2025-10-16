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

      <v-divider class="my-2" />

      <v-subheader>Equipment Management</v-subheader>
      <v-list-item
        :to="`/dashboard/customers/${userId}/active-rental`"
        title="Active Rental"
        prepend-icon="mdi-checkbox-marked-outline"
      />

      <v-list-item
        :to="`/dashboard/customers/${userId}/active-rental`"
        title="Active Rental"
        prepend-icon="mdi-checkbox-marked-outline"
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

      <v-list-item
        :to="`/dashboard/customers/${userId}/active-rental`"
        title="Active Rental"
        prepend-icon="mdi-checkbox-marked-outline"
      />

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
const isAdmin = true
const isUser = !isAdmin
const userId = 123

const UserNavigationItems = [
  {
    categoryTitle: 'User Profile',
    items: [{ url: 'dsa', title: 'View Profile', prependIdon: 'dsa' }],
  },
  {
    category: 'Rental Management',
    items: [
      { url: 'dsa', title: 'My Rentals', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Rental Return Form', prependIdon: 'dsa' },
    ],
  },
]

const AdminNavigationItems = [
  {
    categoryTitle: 'Customer Management',
    items: [
      { url: 'dsa', title: 'Customers,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Equipment,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Rentals,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Equipment,', prependIdon: 'dsa' },
    ],
  },
  {
    category: 'Equipment Management',
    items: [
      { url: 'dsa', title: 'Customer,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Equipment,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Rentals,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Equipment,', prependIdon: 'dsa' },
    ],
  },
  {
    category: 'Rental Management',
    items: [
      { url: 'dsa', title: 'All Rentals,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Issue Equipment,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Return Equipment,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Overdue Rentals,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Completed Rentals,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Extend Rental,', prependIdon: 'dsa' },
      { url: 'dsa', title: 'Cancel Rental,', prependIdon: 'dsa' },
    ],
  },
]
</script>
