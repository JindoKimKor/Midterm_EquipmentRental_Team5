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
      children: [
        {
          path: '',
          name: 'DashboardHomeView',
          component: () => import('../views/dashboard/main/HomeView.vue'),
        },
        {
          path: 'equipment',
          name: 'EquipmentDashboard',
          component: () => import('../views/dashboard/main/EquipmentView.vue'),
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
          path: 'customer',
          name: 'CustomerDashboard',
          component: () => import('../views/dashboard/main/CustomerView.vue'),
          children: [
            {
              path: ':id',
              name: 'CustomerDetailView',
              component: () => import('../views/dashboard/CustomerDetailsView.vue'),
              props: true,
            },
          ],
        },
        {
          path: 'rental',
          name: 'RenalDashboard',
          component: () => import('../views/dashboard/main/RentalView.vue'),
          children: [
            {
              path: ':id',
              name: 'RentalDetailView',
              component: () => import('../views/dashboard/RentalDetailsView.vue'),
              props: true,
            },
          ],
        },
      ],
    },
  ],
})

export default router
