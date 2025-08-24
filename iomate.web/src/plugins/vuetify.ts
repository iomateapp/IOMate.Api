// filepath: c:\iomatedev\iomate.web\src\plugins\vuetify.ts
/**
 * plugins/vuetify.ts
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

// Composables
import { createVuetify } from 'vuetify'

// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides
export default createVuetify({
  theme: {
    defaultTheme: 'system',
  },
  components: {
    VAppBar: {
      elevation: 2,
    },
    VMenu: {
      closeOnContentClick: false,
    },
  },
})