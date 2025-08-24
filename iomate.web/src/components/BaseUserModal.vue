<template>
  <v-dialog v-model="internalVisible" max-width="500">
    <v-card>
      <v-card-title>{{ title }}</v-card-title>
      <v-card-text>
        <v-form ref="formRef" v-model="formValid">
          <v-text-field
            v-model="localUser.firstName"
            label="First Name"
            :rules="[v => !!v || 'First name é obrigatório']"
            required
          />
          <v-text-field
            v-model="localUser.lastName"
            label="Last Name"
            :rules="[v => !!v || 'Last name é obrigatório']"
            required
          />
          <v-text-field
            v-model="localUser.userName"
            label="User Name"
            required
          />
          <v-text-field
            v-model="localUser.email"
            label="Email"
            type="email"
            :rules="[v => /\S+@\S+\.\S+/.test(v) || 'Email inválido']"
            required
          />
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn text @click="handleClose">Cancelar</v-btn>
        <v-btn color="primary" @click="handleSave">Salvar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup>
import { ref, watch } from 'vue';

const props = defineProps({
  title: { type: String, default: 'Usuário' },
  modelValue: { type: Boolean, default: false },
  user: {
    type: Object,
    default: () => ({
      id: null,
      firstName: '',
      lastName: '',
      userName: '',
      email: ''
    })
  }
});

const emit = defineEmits(['update:modelValue', 'save', 'close']);

const internalVisible = ref(props.modelValue);
const localUser = ref({ ...props.user });
const formRef = ref(null);
const formValid = ref(false);

watch(
  () => props.modelValue,
  val => {
    internalVisible.value = val;
    if (val) {
      localUser.value = { ...props.user };
    }
  }
);

watch(internalVisible, val => emit('update:modelValue', val));

function handleClose() {
  emit('close');
  internalVisible.value = false;
}

async function handleSave() {
  const valid = await formRef.value.validate();
  if (!valid) return;
  emit('save', localUser.value);
  internalVisible.value = false;
}
</script>
