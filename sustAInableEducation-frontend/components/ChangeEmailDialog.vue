<template>
  <Dialog v-model:visible="model" header="Passwort ändern" modal :draggable="false" dismissable-mask class="max-w-md w-full mx-4">
    <div class="w-full flex flex-col gap-4">
      <div>
        <p class="text-lg">Neue Email</p>
        <InputText v-model="email" type="email" fluid toggle-mask :feedback="false"/>
        <Message v-if="validator.email" severity="error" size="small" class="mt-2">{{ validator.email }}</Message>
      </div>
      <div>
        <p class="text-lg">Passwort</p>
        <Password v-model="password" type="password" fluid toggle-mask :feedback="false"/>
      </div>
      <div class="flex justify-between">
        <Button label="Abbrechen" @click="model = !model" outlined />
        <Button label="Speichern" @click="changeEmail" :loading="loading" :disabled="loading || !validator.valid" />
      </div>
    </div>
  </Dialog>
</template>

<script lang="ts" setup>
const model = defineModel('visible', { required: true, type: Boolean });
const emit = defineEmits(['success', 'fail']);

watch(() => model.value, (value) => {
  if(value) {
    password.value = '';
    email.value = '';
  }
});

const runtimeConfig = useRuntimeConfig();

const password = ref('');
const email = ref('');
const loading = ref(false);

const validator = computed(() => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  let valid = emailRegex.test(email.value) && password.value.length > 0;
  
  return {
    email: (emailRegex.test(email.value) || email.value.length === 0) ? '' : 'Bitte geben Sie eine gültige Email-Adresse ein',
    valid: valid
  }
})


async function changeEmail() {
  loading.value = true;

  await $fetch(`${runtimeConfig.public.apiUrl}/account/changeemail`, {
    method: 'POST',
    credentials: 'include',
    body: {
      password: password.value,
      newEmail: email.value,
    },
    onResponse: (response) => {
      if (response.response.ok) {
        emit('success');
        model.value = false;
      } else {
        emit('fail');
      }
    }
  });

  loading.value = false;
}
</script>