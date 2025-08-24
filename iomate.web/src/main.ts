/**
 * main.ts
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Plugins
import { registerPlugins } from '@/plugins'

// Components
import App from './App.vue'

// Composables
import { createApp } from 'vue'

// Styles
import 'unfonts.css'

import 'vuetify/styles'
import './api/apiInterceptor';

import i18n from './plugins/i18n'
import { registerAuthContext } from './plugins/auth'

const app = createApp(App)

app.use(i18n)

registerPlugins(app)

registerAuthContext(app)

app.mount('#app')
