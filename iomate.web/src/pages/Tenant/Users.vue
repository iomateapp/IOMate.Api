<template>
  <v-card>
    <v-card-title class="d-flex justify-space-between align-center">
      Usuários
      <v-btn color="primary" @click="openCreate">
        <v-icon start>mdi-plus</v-icon>
        Adicionar Usuário
      </v-btn>
    </v-card-title>

    <v-card-text>
      <v-table>
        <thead>
          <tr>
            <th></th>
            <th>Full Name</th>
            <th>User Name</th>
            <th>Email</th>
            <th class="text-center">Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in users" :key="user.id">
            <td>
              <v-avatar size="small" class="mr-2" color="primary">
                <span class="text-h6">{{ user.firstName.charAt(0) }}{{ user.lastName.charAt(0) }}</span>
              </v-avatar>
            </td>
            <td>{{ user.fullName }}</td>
            <td>{{ user.userName }}</td>
            <td>{{ user.email }}</td>
            <td class="text-center">
              <v-menu>
                <template #activator="{ props }">
                  <v-btn v-bind="props" icon variant="text">
                    <v-icon>mdi-dots-vertical</v-icon>
                  </v-btn>
                </template>
                <v-list>
                  <v-list-item @click="openEdit(user)">
                    <v-icon start color="primary">mdi-pencil</v-icon>
                    Editar
                  </v-list-item>
                  <v-list-item @click="deleteUser(user.id)">
                    <v-icon start color="red">mdi-delete</v-icon>
                    Excluir
                  </v-list-item>
                </v-list>
              </v-menu>
            </td>
          </tr>
        </tbody>
      </v-table>
    </v-card-text>
  </v-card>

  <BaseUserModal
    v-model="modalVisible"
    :title="modalTitle"
    :user="selectedUser"
    @save="saveUser"
  />
</template>

<script setup>
import { ref, onMounted } from 'vue';
import userService from '@/services/userService';
import BaseUserModal from '@/components/BaseUserModal.vue';

const users = ref([]);
const modalVisible = ref(false);
const modalTitle = ref('');
const selectedUser = ref({});

onMounted(loadUsers);

async function loadUsers() {
  users.value = await userService.getUsers();
}

function openCreate() {
  selectedUser.value = { id: null, firstName: '', lastName: '', userName: '', email: '' };
  modalTitle.value = 'Adicionar Usuário';
  modalVisible.value = true;
}

function openEdit(user) {
  selectedUser.value = { ...user };
  modalTitle.value = 'Editar Usuário';
  modalVisible.value = true;
}

async function saveUser(userData) {
  if (userData.id) {
    await userService.updateUser(userData, userData.id);
  } else {
    await userService.createUser(userData);
  }
  await loadUsers();
}

async function deleteUser(id) {
  await userService.deleteUser(id);
  await loadUsers();
}
</script>
