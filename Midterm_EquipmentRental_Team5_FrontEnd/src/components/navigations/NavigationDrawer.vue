<template>
  <v-navigation-drawer app permanent>
    <v-list dense nav>
      <div v-for="(section, index) in navigation" :key="index">
        <v-subheader>{{ section.categoryTitle }}</v-subheader>

        <v-list-item
          v-for="(item, idx) in section.items"
          :key="idx"
          :to="resolveUrl(item.url)"
          :title="item.title"
          :prepend-icon="item.prependIcon"
        />
      </div>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup>
import { onBeforeMount, ref } from 'vue'

const isAdmin = true
const isUser = !isAdmin
const userId = 123
const role = ref('Admin')

const navigation = ref([])

onBeforeMount(() => {
  if (role.value === 'Admin') {
    navigation.value = AdminNavigationItems
  } else {
    navigation.value = UserNavigationItems
  }
})

// Util to replace placeholder tokens in URL
const resolveUrl = (url) => {
  return url.replace(':userId', userId)
}

// Navigation for normal users
const UserNavigationItems = [
  {
    categoryTitle: 'User Profile',
    items: [
      {
        url: '/dashboard/customers/:userId',
        title: 'My Profile',
        prependIcon: 'mdi-account',
      },
    ],
  },
  {
    categoryTitle: 'Rental Management',
    items: [
      {
        url: '/dashboard/customers/:userId/rentals',
        title: 'My Rentals',
        prependIcon: 'mdi-history',
      },
      {
        url: '/dashboard/customers/:userId/active-rental',
        title: 'Active Rental',
        prependIcon: 'mdi-checkbox-marked-outline',
      },
    ],
  },
]

// Navigation for admins
const AdminNavigationItems = [
  {
    categoryTitle: 'Customer Management',
    items: [
      {
        url: '/dashboard/customer',
        title: 'Customer List',
        prependIcon: 'mdi-account-group',
      },
      {
        url: '/dashboard/customers/:userId',
        title: 'My Profile',
        prependIcon: 'mdi-account',
      },
    ],
  },
  {
    categoryTitle: 'Equipment Management',
    items: [
      {
        url: '/dashboard/customers/:userId/active-rental',
        title: 'Active Rental',
        prependIcon: 'mdi-checkbox-marked-outline',
      },
      {
        url: '/dashboard/customers/:userId/rentals',
        title: 'My Rentals',
        prependIcon: 'mdi-history',
      },
    ],
  },
  {
    categoryTitle: 'Rental Management',
    items: [
      {
        url: '/dashboard/rental',
        title: 'All Rentals',
        prependIcon: 'mdi-clipboard-list',
      },
      {
        url: '/dashboard/rental/issue',
        title: 'Issue Equipment',
        prependIcon: 'mdi-arrow-up-bold-box',
      },
      {
        url: '/dashboard/rental/return',
        title: 'Return Equipment',
        prependIcon: 'mdi-arrow-down-bold-box',
      },
      {
        url: '/dashboard/rental/active',
        title: 'Active Rentals',
        prependIcon: 'mdi-playlist-check',
      },
      {
        url: '/dashboard/rental/completed',
        title: 'Completed Rentals',
        prependIcon: 'mdi-calendar-check',
      },
      {
        url: '/dashboard/rental/overdue',
        title: 'Overdue Rentals',
        prependIcon: 'mdi-alert',
      },
      {
        url: '/dashboard/rental/equipment-history',
        title: 'Equipment History',
        prependIcon: 'mdi-history',
      },
    ],
  },
]
</script>
