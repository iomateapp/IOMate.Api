<template>
  <v-layout>
    <NavigationDrawer />
    <v-main>
      <TopBar />
      <v-container fluid class="pa-6">
        <router-view />
      </v-container>
    </v-main>
  </v-layout>
</template>

<script setup>
</script>

<style scoped>
.v-main {
  min-height: 100vh;
}
</style>