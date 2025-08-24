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

          <v-form ref="formRef" v-model="valid" lazy-validation>
            <v-text-field
              v-model="credentials.username"
              label="Username"
              type="text"
              :rules="usernameRules"
              required
              prepend-inner-icon="mdi-account"
            />

            <v-text-field
                v-model="credentials.password"
                :type="showPassword ? 'text' : 'password'"
                label="Password"
                :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                @click:append-inner="showPassword = !showPassword"
                :rules="passwordRules"
                required
                prepend-inner-icon="mdi-lock"
            />

            <v-btn
              class="mt-4"
              :loading="loading"
              :disabled="!valid || loading"
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
  </v-container>
</template>

<script setup>
import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import logo from '@/assets/iomate-logo-icon.png';
import userService from '@/services/userService';

const router = useRouter();
const formRef = ref(null);
const valid = ref(false);
const loading = ref(false);
const showPassword = ref(false);
const error = ref('');

const credentials = reactive({
  email: '',
  password: '',
  remember: false,
});

const emailRules = [
  v => !!v || 'Email is required',
  v => /\S+@\S+\.\S+/.test(v) || 'Invalid email',
];

const passwordRules = [
  v => !!v || 'Password is required',
  v => (v && v.length >= 6) || 'Password must be at least 6 characters',
];

async function onSubmit() {
  error.value = '';
  if (!formRef.value) return;

  const validForm = await formRef.value.validate();
  if (!validForm) return;

  loading.value = true;
  try {
    const data = await userService.login({
      email: credentials.username,
      password: credentials.password,
      remember: credentials.remember,
    });

    if (data && data.accessToken) {
      localStorage.setItem('auth_token', `Bearer ${data.accessToken}`);
      router.push({ path: '/' });
    } else {
      error.value = 'Unexpected server response.';
    }
  } catch (err) {
    error.value = err.message || 'Login failed. Please check your credentials.';
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
