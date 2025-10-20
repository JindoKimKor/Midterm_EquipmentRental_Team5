<template>
  <v-navigation-drawer app permanent>
    <v-list dense nav aria-label="Primary Navigation">
      <template v-for="(section, index) in navigation" :key="`section-${index}`">
        <v-subheader>{{ section.categoryTitle }}</v-subheader>
        <v-list-item
          v-for="(item, idx) in section.items"
          :key="`item-${index}-${idx}`"
          :to="item.url"
          router
          link
          exact-active-class="v-item--active"
          :aria-label="item.title"
          @click="item.function?.()"
        >
          <template v-slot:prepend>
            <v-icon>{{ item.prependIcon }}</v-icon>
          </template>

          <v-list-item-content>
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-divider v-if="index !== navigation.length - 1" class="my-2" />
      </template>
      <v-switch
        v-model="isDarkTheme"
        :label="`${isDarkTheme ? 'Dark' : 'Light'}`"
        @change="theme.toggle()"
        inset
      />
    </v-list>
  </v-navigation-drawer>
</template>

<script setup>
import { ref, computed } from 'vue'
import useAuthenticationStore from '@/stores/Authentication'
import { useUserInformationStore } from '@/stores/UserInformation'
import { useTheme } from 'vuetify'

const theme = useTheme()
const themeName = ref(theme.name)

const role = ref('Admin')
const authStore = useAuthenticationStore()
const userStore = useUserInformationStore()

const navigation = computed(() => {
  return role.value === authStore.authRole ? AdminNavigationItems : UserNavigationItems
})

const UserNavigationItems = [
  {
    categoryTitle: 'User Profile',
    items: [
      {
        url: `/dashboard/customers/${userStore.id}`,
        title: 'My Profile',
        prependIcon: 'mdi-account',
      },
    ],
  },
  {
    categoryTitle: 'Rental Management',
    items: [
      {
        url: `/dashboard/customers/${userStore.id}/rentals`,
        title: 'My Rentals',
        prependIcon: 'mdi-history',
      },
      {
        url: '/dashboard/rentals/issue',
        title: 'Issue Equipment',
        prependIcon: 'mdi-arrow-up-bold-box',
      },
      {
        url: '/dashboard/rentals/return',
        title: 'Return Equipment',
        prependIcon: 'mdi-arrow-down-bold-box',
      },
    ],
  },
]

const AdminNavigationItems = [
  {
    categoryTitle: 'Customer Management',
    items: [
      {
        url: '/dashboard/customers',
        title: 'Customers',
        prependIcon: 'mdi-account-group',
      },
    ],
  },
  {
    categoryTitle: 'Equipment Management',
    items: [
      {
        url: '/dashboard/equipments',
        title: 'Equipment',
        prependIcon: 'mdi-checkbox-marked-outline',
      },
    ],
  },
  {
    categoryTitle: 'Rental Management',
    items: [
      {
        url: '/dashboard/rentals',
        title: 'Rentals',
        prependIcon: 'mdi-clipboard-list',
      },
      {
        url: '/dashboard/rentals/issue',
        title: 'Issue Rental',
        prependIcon: 'mdi-truck-fast-outline', // Issuing = dispatching
      },
      {
        url: '/dashboard/rentals/return',
        title: 'Return Rental',
        prependIcon: 'mdi-arrow-down-bold-box',
      },
      {
        url: '/dashboard/rentals/extend',
        title: 'Extend Rental',
        prependIcon: 'mdi-calendar-plus',
      },
      {
        url: '/dashboard/rentals/cancel',
        title: 'Cancel Rental',
        prependIcon: 'mdi-cancel',
      },
    ],
  },
  {
    categoryTitle: '',
    items: [
      {
        url: '/',
        title: 'Logout',
        prependIcon: 'mdi-logout',
        function: () => authStore.logout(),
      },
    ],
  },
]
</script>
