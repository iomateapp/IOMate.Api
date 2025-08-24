<template>
  <v-container>
    <v-card>
      <v-tabs v-model="tab" background-color="primary" dark>
        <v-tab value="users">Usuários</v-tab>
      </v-tabs>
      <v-window v-model="tab">
        <v-window-item value="users">
          <Users />
        </v-window-item>
      </v-window>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref } from 'vue'
import Users from './Users.vue';

const tab = ref('users')
const users = ref([
  { id: 1, name: 'Alice', email: 'alice@email.com' },
  { id: 2, name: 'Bob', email: 'bob@email.com' },
])
</script>