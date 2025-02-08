<template>
  <Dialog v-model:visible="model" header="Passwort ändern" modal :draggable="false" dismissable-mask class="max-w-md w-full mx-4">
    <div class="w-full flex flex-col gap-4">
      <div>
        <p class="text-lg">Altes Passwort</p>
        <Password v-model="oldPassword" type="password" fluid toggle-mask :feedback="false"/>
        <Message v-if="validator.oldPassword" severity="error" size="small" class="mt-2">{{ validator.oldPassword }}</Message>
      </div>
      <div>
        <p class="text-lg">Neues Passwort</p>
        <Password v-model="newPassword" type="password" fluid toggle-mask :feedback="false"/>
        <Message v-if="validator.newPassword" severity="error" size="small" class="mt-2">{{ validator.newPassword }}</Message>
      </div>
      <div>
        <p class="text-lg">Neues Passwort wiederholen</p>
        <Password v-model="newPasswordRepeat" type="password" fluid toggle-mask :feedback="false"/>
        <Message v-if="validator.newPasswordRepeat" severity="error" size="small" class="mt-2">{{ validator.newPasswordRepeat }}</Message>
      </div>
      <div class="flex justify-between">
        <Button label="Abbrechen" @click="model = !model" outlined />
        <Button label="Speichern" @click="changePassword" :loading="loading" :disabled="loading || !validator.valid" />
      </div>
    </div>
  </Dialog>
</template>

<script lang="ts" setup>
const model = defineModel('visible', { required: true, type: Boolean });
const emit = defineEmits(['success', 'fail']);

const runtimeConfig = useRuntimeConfig();

const oldPassword = ref('');
const newPassword = ref('');
const newPasswordRepeat = ref('');
const loading = ref(false);

const validator = computed(() => {
  let newPasswordError = '';
  if(newPassword.value.length < 8) {
    newPasswordError = 'Das Passwort muss mindestens 8 Zeichen lang sein';
  } else if(includesSpecialCharacter(newPassword.value)) {
    newPasswordError = 'Das Passwort muss mindestens ein Sonderzeichen enthalten';
  } else if(includesNumber(newPassword.value)) {
    newPasswordError = 'Das Passwort muss mindestens eine Ziffer enthalten';
  } else if(includesUppercase(newPassword.value)) {
    newPasswordError = 'Das Passwort muss mindestens einen Großbuchstaben enthalten';
  } else if(includesLowercase(newPassword.value)) {
    newPasswordError = 'Das Passwort muss mindestens einen Kleinbuchstaben enthalten';
  }
  if(newPassword.value.length === 0) newPasswordError = '';

  let valid = newPasswordError.length <= 0 && newPasswordRepeat.value === newPassword.value && newPasswordRepeat.value.length > 0 && oldPassword.value.length > 0;

  return {
    oldPassword: '',
    newPassword: newPasswordError,
    newPasswordRepeat: newPasswordRepeat.value !== newPassword.value && newPasswordRepeat.value.length > 0 ? 'Die Passwörter stimmen nicht überein' : '',
    valid: valid
  }
})

function includesSpecialCharacter(str: string) {
  let RegEx = /^[a-z0-9]+$/i;
  return RegEx.test(str);
}

function includesNumber(str: string) {
  let RegEx = /\d/;
  return !RegEx.test(str);
}

function includesUppercase(str: string) {
  let RegEx = /[A-ZÄÖÜ]/;
  return !RegEx.test(str);
}

function includesLowercase(str: string) {
  let RegEx = /[a-zäöü]/;
  return !RegEx.test(str);
}


async function changePassword() {
  loading.value = true;

  await $fetch(`${runtimeConfig.public.apiUrl}/account/changepassword`, {
    method: 'POST',
    credentials: 'include',
    body: {
      oldPassword: oldPassword.value,
      newPassword: newPassword.value,
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