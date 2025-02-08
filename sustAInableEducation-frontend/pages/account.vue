<template>
  <div class="w-full h-full flex justify-center items-center mt-16">
    <div class="background animate-anim" />
    <div class="bg-white relative flex justify-center p-5 rounded-lg shadow-xl w-full mx-4 sm:mx-0 sm:w-fit">
      <div
        class="pp absolute top-[-4.5rem] rounded-full overflow-hidden border-4 border-white shadow-md flex justify-center items-center">
        <Image src="/img/profilepicture_placeholder.jpg" width="128" alt="Profilbild" preview>
          <template #previewicon>
            <div />
          </template>
        </Image>
      </div>
      <div class="flex flex-col mt-16 gap-4 w-full sm:min-w-80">
        <div>
          <p class="text-lg">Benutzername</p>
          <InputText id="username" value="CarbonFreeStewardship8151" type="text" fluid disabled />
        </div>
        <div>
          <p class="text-lg">Email</p>
          <InputText id="username" value="test@example.com" type="email" fluid disabled />
        </div>
        <div>
          <p class="text-lg">Passwort</p>
          <InputText id="username" value="sustAInableEducation" type="password" fluid disabled />
        </div>
        <div class="flex flex-col gap-2">
          <Divider />
          <Button label="Email ändern" outlined @click="" />
          <Button label="Passwort ändern" outlined @click="" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">

useHead({
  title: 'Kontoübersicht - sustAInableEducation'
})

const toggle = ref(false)
</script>