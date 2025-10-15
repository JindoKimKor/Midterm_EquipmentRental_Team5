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
          path: 'home',
          name: 'DashboardHomeView',
          component: () => import('../views/dashboard/HomeView.vue'),
        },
        {
          path: 'equipments/:id',
          name: 'EquipmentDetailView',
          component: () => import('../views/dashboard/EquipmentDetailsView.vue'),
          props: true,
        },
        {
          path: 'customers/:id',
          name: 'CustomerDetailView',
          component: () => import('../views/dashboard/CustomerDetailsView.vue'),
          props: true,
        },
        {
          path: 'rentals/:id',
          name: 'RentalDetailView',
          component: () => import('../views/dashboard/RentalDetailsView.vue'),
          props: true,
        },
      ],
    },
  ],
})

export default router
