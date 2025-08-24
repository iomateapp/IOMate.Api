<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="5" lg="4">
        <v-card elevation="12" class="pa-6">
          <v-row class="mb-4" align="center" justify="space-between">
            <div>
              <h2 class="mb-0">Login</h2>
              <p class="subtitle-1">Access your account</p>
            </div>
            <v-avatar size="48">
              <v-img :src="logo" alt="logo" />
            </v-avatar>
          </v-row>

          <v-form ref="formRef" lazy-validation>
            <v-text-field
              v-model="credentials.email"
              label="Email"
              type="text"
              :error-messages="fieldErrors.email"
              required
              prepend-inner-icon="mdi-account"
            />

            <v-text-field
                v-model="credentials.password"
                :type="showPassword ? 'text' : 'password'"
                label="Password"
                :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                @click:append-inner="showPassword = !showPassword"
                :error-messages="fieldErrors.password"
                required
                prepend-inner-icon="mdi-lock"
            />

            <v-btn
              class="mt-4"
              :loading="loading"
              :disabled="loading"
              color="primary"
              large
              block
              @click="onSubmit"
            >
              Sign in
            </v-btn>

            <v-alert v-if="error" type="error" class="mt-4" dense>
              {{ error }}
            </v-alert>
          </v-form>
        </v-card>
      </v-col>
    </v-row>
  <!-- Snackbar global agora é exibido em App.vue -->
  </v-container>
</template>

<script setup>


import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import logo from '@/assets/iomate-logo-icon.png';
import authService from '@/services/authService';
import { useAuth } from '@/contexts/authContext';
import { useGlobalSnackbar } from '@/contexts/globalSnackbar';

const router = useRouter();
const formRef = ref(null);
const loading = ref(false);
const showPassword = ref(false);
const error = ref('');
const snackbar = useGlobalSnackbar();
const fieldErrors = reactive({ email: [], password: [] });

const { setAuth } = useAuth();

const credentials = reactive({
  email: '',
  password: ''
});

async function onSubmit() {
  error.value = '';
  fieldErrors.email = [];
  fieldErrors.password = [];
  if (!formRef.value) return;

  const validForm = await formRef.value.validate();
  if (!validForm) return;

  loading.value = true;
  try {
    const data = await authService.authenticate({
      email: credentials.email,
      password: credentials.password,
    });

    if (data && data.token) {
      setAuth({ token: data.token });
      router.push({ path: '/' });
    } else {
      error.value = 'Unexpected server response.';
    }
  } catch (err) {
    let response = err?.response?.data || err;
    if (response?.ValidationErrors) {
      for (const fieldError of response.ValidationErrors) {
        const field = fieldError.Field?.toLowerCase();
        if (fieldErrors[field]) {
          fieldErrors[field].push(fieldError.Message);
        }
      }
    }
    if (response?.Message) {
      snackbar.notify(response.Message, { color: 'error' });
    } else {
      error.value = err.message || 'Login failed. Please check your credentials.';
    }
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.fill-height {
  min-height: 100vh;
}
.text-decoration-none {
  text-decoration: none;
}
</style>
