// filepath: c:\iomatedev\iomate.web\src\router\index.ts
/**
 * router/index.ts
 *
 * Automatic routes for `./src/pages/*.vue`
 */

// Composables
import Layout from '@/layouts/Layout.vue'
import Login from '@/layouts/Login.vue'
import Tenant from '@/pages/Tenant/Tenant.vue'
import Users from '@/pages/Tenant/Users.vue'
import authorizationStorage from '@/services/authorizationStorage'
import { createRouter, createWebHistory } from 'vue-router/auto'

const routes = [
  {
    path: '/',
    component: Layout,
    meta: { requiresAuth: true },
    children: [
      { path: 'tenant', name: 'Tenant', component: Tenant },
      { path: 'users', name: 'Users', component: Users },
    ],
  },
  {
    path: '/login',
    component: Login
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    ...routes
  ],
})

// Workaround for https://github.com/vitejs/vite/issues/11804
router.onError((err, to) => {
  if (err?.message?.includes?.('Failed to fetch dynamically imported module')) {
    if (localStorage.getItem('vuetify:dynamic-reload')) {
      console.error('Dynamic import error, reloading page did not fix it', err)
    } else {
      console.log('Reloading page to fix dynamic import error')
      localStorage.setItem('vuetify:dynamic-reload', 'true')
      location.assign(to.fullPath)
    }
  } else {
    console.error(err)
  }
})

router.isReady().then(() => {
  localStorage.removeItem('vuetify:dynamic-reload')
})

router.beforeEach((to, from, next) => {
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);

  const token = authorizationStorage.getToken();


  if (requiresAuth && !token) {
    next({ path: '/login' });
  } else {
    next();
  }
});

export default router