<template>
  <Dialog v-model:visible="model" header="Passwort ändern" modal :draggable="false" dismissable-mask>
    <div class="flex flex-col gap-4">
      <div>
        <p class="text-lg">Altes Passwort</p>
        <InputText v-model="oldPassword" type="password" />
      </div>
      <div>
        <p class="text-lg">Neues Passwort</p>
        <InputText v-model="newPassword" type="password" />
      </div>
      <div>
        <p class="text-lg">Neues Passwort wiederholen</p>
        <InputText v-model="newPasswordRepeat" type="password" />
      </div>
      <div class="flex justify-between">
        <Button label="Abbrechen" @click="model = !model" outlined/>
        <Button label="Speichern" @click="" :loading="loading"/>
      </div>
    </div>
  </Dialog>
</template>

<script lang="ts" setup>
  const model = defineModel('visible', {required: true, type: Boolean});
  const emit = defineEmits(['success', 'fail']);

  const runtimeConfig = useRuntimeConfig();

  const oldPassword = ref('');
  const newPassword = ref('');
  const newPasswordRepeat = ref('');
  const loading = ref(false);
</script>