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
      component: () => import('../views/DashboardView.vue'),
      meta: { requiresAuth: true },
      children: [
        {
          path: '',
          name: 'DashboardHomeView',
          component: () => import('../views/dashboard/main/HomeView.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: 'equipment',
          name: 'EquipmentDashboard',
          component: () => import('../views/dashboard/main/EquipmentView.vue'),
          meta: { requiresAuth: true },
          children: [
            {
              path: ':id',
              name: 'EquipmentDetailView',
              component: () => import('../views/dashboard/EquipmentDetailsView.vue'),
              props: true,
            },
          ],
        },
        {
          path: 'customers',
          name: 'CustomerDashboard',
          component: () => import('../views/dashboard/main/CustomerView.vue'),
        },
        {
          path: 'customers/:id',
          name: 'CustomerDetailView',
          component: () => import('../views/dashboard/CustomerDetailsView.vue'),
          meta: { requiresAuth: true },
          props: true,
        },
        {
          path: 'rental',
          name: 'RenalDashboard',
          component: () => import('../views/dashboard/main/RentalView.vue'),
          meta: { requiresAuth: true },
          children: [
            {
              path: ':id',
              name: 'RentalDetailView',
              component: () => import('../views/dashboard/RentalDetailsView.vue'),
              meta: { requiresAuth: true },
              props: true,
            },
          ],
        },
      ],
    },
  ],
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthenticationStore()

  console.log(authStore.checkAuthToken())

  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth)

  if (requiresAuth && !authStore.checkAuthToken()) {
    next({ name: 'LoginView' })
  } else {
    next()
  }
})

export default router
