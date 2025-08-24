<template>
  <v-navigation-drawer
    expand-on-hover
    rail
  >
    <v-list>
      <v-list-item
        :prepend-avatar="logo"
        :subtitle="$t('app.subtitle')"
        :title="$t('app.title')"
      ></v-list-item>
    </v-list>

    <v-divider></v-divider>

    <v-list density="compact" nav>
      <v-list-item prepend-icon="mdi-home" title="Home" value="home"></v-list-item>
      <v-list-item prepend-icon="mdi-domain" title="Tenant" value="tenant" to="/tenant"></v-list-item>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup>
import logo from '@/assets/iomate-logo-icon.png'
</script>