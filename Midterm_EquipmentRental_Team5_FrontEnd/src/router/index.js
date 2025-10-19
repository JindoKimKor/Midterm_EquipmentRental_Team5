import useAuthenticationStore from '@/stores/Authentication'
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
          meta: { requiresAuth: true },
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
          meta: { requiresAuth: true }, // added auth meta
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
          meta: { requiresAuth: true },
        },
        {
          path: 'rentals/:id',
          name: 'RentalDetailView',
          component: () => import('../views/dashboard/rental/RentalDetailsView.vue'),
          props: true,
          meta: { requiresAuth: true },
        },
        {
          path: 'rentals/issue',
          name: 'RentalIssueView',
          component: () => import('../views/dashboard/rental/IssueEquipmentView.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: 'rentals/return',
          name: 'RentalReturnView',
          component: () => import('../views/dashboard/rental/ReturnEquipmentView.vue'),
          meta: { requiresAuth: true },
        },
      ],
    },
  ],
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthenticationStore()

  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth)

  const doesRoleExist = authStore.checkAuthRole()
  const doesTokenExist = authStore.checkAuthToken()

  if (requiresAuth && !doesRoleExist && !doesTokenExist) {
    next({ name: 'LoginView' })
  } else {
    next()
  }
})

export default router
