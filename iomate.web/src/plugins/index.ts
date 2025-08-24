// filepath: c:\iomatedev\iomate.web\src\plugins\index.ts
/**
 * plugins/index.ts
 *
 * Automatically included in `./src/main.ts`
 */

// Plugins
import vuetify from './vuetify'
import router from '../router'

// Types
import type { App } from 'vue'

// Registering global components
import TopBar from '@/components/TopBar.vue'
import Users from '@/pages/Tenant/Users.vue'
import NavigationDrawer from '@/components/NavigationDrawer.vue'

export function registerPlugins(app: App) {
  app
    .use(vuetify)
    .use(router)
    .component('TopBar', TopBar)
    .component('Users', Users)
    .component('NavigationDrawer', NavigationDrawer)
}