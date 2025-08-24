<template>
  <v-app-bar flat color="white">
    <v-spacer />
    <v-menu offset-y>
      <template #activator="{ props }">
        <v-btn icon v-bind="props">
          <v-avatar color="error" size="small">
            <span class="text-h7">{{ user.initials }}</span>
          </v-avatar>
        </v-btn>
      </template>
      <v-card>
        <v-card-text>
          <div class="mx-auto text-center">
            <v-avatar size="large" color="error">
              <span class="text-h5">{{ user.initials }}</span>
            </v-avatar>
            <h3>{{ user.fullName }}</h3>
            <p class="text-caption mt-1">
              {{ user.email }}
            </p>
            <v-divider class="my-3"></v-divider>
            <v-btn variant="text" rounded>
              Edit Account
            </v-btn>
            <v-divider class="my-3"></v-divider>
            <v-btn variant="text" rounded @click="logout">
              Disconnect
            </v-btn>
          </div>
        </v-card-text>
      </v-card>
    </v-menu>
  </v-app-bar>
</template>

<script setup>
import router from '@/router';
import userService from '@/services/userService';

const user = {
  initials: 'JD',
  fullName: 'John Doe',
  email: 'john.doe@doe.com',
}

function logout() {
  userService.logout();
  router.push({ path: '/login' });
}
</script>