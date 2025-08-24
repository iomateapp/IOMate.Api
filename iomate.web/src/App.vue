
<template>
  <v-app>
    <v-main>
      <router-view />
    </v-main>
    <v-snackbar v-model="snackbar.show.value" :color="snackbar.color.value" :timeout="snackbar.timeout.value" auto-height>
      <div class="d-flex align-center pr-4">
        <v-icon class="pr-3"  size="large">
          {{
            snackbar.color.value === 'error' ? 'mdi-alert-circle' :
            snackbar.color.value === 'success' ? 'mdi-check-circle' :
            snackbar.color.value === 'warning' ? 'mdi-alert' :
            'mdi-information'
          }}
        </v-icon>
        <div>
          <div v-if="snackbar.title.value && snackbar.title.value !== ''">
            <strong>{{ snackbar.title.value }}</strong>
          </div>
          <div>{{ snackbar.message.value }}</div>
        </div>
      </div>
      <template #actions>
        <v-btn icon @click="snackbar.show.value = false">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </template>
    </v-snackbar>
  </v-app>
</template>

<script lang="ts" setup>
import { provideAuth } from '@/contexts/authContext';
import { provideGlobalSnackbar } from '@/contexts/globalSnackbar';
provideAuth();
const snackbar = provideGlobalSnackbar();
</script>
