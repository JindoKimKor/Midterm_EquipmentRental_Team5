import { useAuthenticationStore } from '@/stores/Authentication'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'LoginView',
      component: () => import('../views/LoginView.vue'),
    },
    {
      path: '/dashboard',
      name: 'DashboardView',
      component: () => import('../views/dashboard/DashboardView.vue'),
      meta: { requiresAuth: true },
      children: [
        {
          path: '',
          name: 'DashboardHomeView',
          component: () => import('../views/dashboard/HomeView.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: 'equipments',
          name: 'EquipmentDashboard',
          component: () => import('../views/dashboard/equipment/EquipmentView.vue'),
          meta: { requiresAuth: true, admin: true },
        },
        {
          path: 'equipments/:id',
          name: 'EquipmentDetailView',
          component: () => import('../views/dashboard/equipment/EquipmentDetailsView.vue'),
          props: true,
          meta: { requiresAuth: true },
        },
        {
          path: 'customers',
          name: 'CustomerDashboard',
          component: () => import('../views/dashboard/customer/CustomerView.vue'),
          meta: { requiresAuth: true, admin: true }, // added auth meta
        },
        {
          path: 'customers/:id',
          name: 'CustomerDetailView',
          component: () => import('../views/dashboard/customer/CustomerDetailsView.vue'),
          props: true,
          meta: { requiresAuth: true },
        },
        {
          path: 'customers/:id/rentals',
          name: 'CustomerRentalView',
          component: () => import('../views/dashboard/rental/RentalView.vue'),
          props: true,
          meta: { requiresAuth: true },
        },
        {
          path: 'rentals',
          name: 'RentalDashboard',
          component: () => import('../views/dashboard/rental/RentalView.vue'),
          meta: { requiresAuth: true, admin: true },
        },
        {
          path: 'rentals/:id',
          name: 'RentalDetailView',
          component: () => import('../views/dashboard/rental/RentalDetailsView.vue'),
          props: true,
          meta: { requiresAuth: true, admin: true },
        },
        {
          path: 'rentals/issue',
          name: 'IssueRentalView',
          component: () => import('../views/dashboard/rental/IssueEquipmentView.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: 'rentals/return',
          name: 'ReturnRentalView',
          component: () => import('../views/dashboard/rental/ReturnEquipmentView.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: 'rentals/extend',
          name: 'ExtendRentalView',
          component: () => import('../views/dashboard/rental/ExtendRentalView.vue'),
          meta: { requiresAuth: true, admin: true },
        },
        {
          path: 'rentals/cancel',
          name: 'CancelRentalView',
          component: () => import('../views/dashboard/rental/CancelRentalView.vue'),
          meta: { requiresAuth: true, admin: true },
        },
        {
          path: 'equipment/:id/rental-history',
          name: 'EquipmentRentalHistory',
          component: () => import('@/views/dashboard/equipment/EquipmentRentalHistoryView.vue'),
          meta: { requiresAuth: true, admin: true },
        },
      ],
    },
    {
      path: '/chat',
      name: 'ChatView',
      component: () => import('../views/ChatView.vue'),
      meta: { requiresAuth: true },
    },
  ],
})

router.beforeEach(async (to, from, next) => {
  const authStore = useAuthenticationStore()

  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth)
  const requiresAdmin = to.matched.some((record) => record.meta.admin)

  const doesAuthCookieExist = await authStore.checkAspNetCoreCookie()
  const userRole = authStore.authRole

  if (requiresAuth && !doesAuthCookieExist) {
    return next({ name: 'LoginView' })
  }

  if (requiresAdmin && userRole !== 'Admin') {
    return next({ name: 'DashboardHomeView' })
  }

  if (to.name === 'LoginView' && doesAuthCookieExist) {
    return next({ name: 'DashboardHomeView' })
  }
  next()
})

export default router
