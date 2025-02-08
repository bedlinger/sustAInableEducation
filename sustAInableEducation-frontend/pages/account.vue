<template>
  <div class="w-full h-full flex justify-center items-center mt-16">
    <div class="background animate-anim" />
    <div class="bg-white min-h-[30rem] relative flex justify-center p-5 rounded-lg shadow-xl">
      <div
        class="pp absolute top-[-4.5rem] rounded-full overflow-hidden border-4 border-white shadow-md flex justify-center items-center">
        <Image src="/img/profilepicture_placeholder.jpg" width="128" alt="Profilbild" preview>
          <template #previewicon>
            <div/>
          </template>
        </Image>
      </div>
      <div class="flex flex-col mt-16 gap-4 min-w-72">
        <div>
          <p class="text-lg">Benutzername</p>
          <div class="relative flex items-center w-full">
            <InputText id="username" value="CarbonFreeStewardship8151" type="text" fluid disabled />
            <div class="absolute right-0 cursor-pointer h-full aspect-square flex justify-center items-center">
              <Icon name="ic:baseline-edit" class="w-5 h-5 text-gray-400" />
            </div>
          </div>
        </div>
        <div>
          <p class="text-lg">Email</p>
          <div class="relative w-full flex items-center">
            <InputText id="username" value="test@example.com" type="email" fluid disabled />
            <div class="absolute right-0 cursor-pointer h-full aspect-square flex justify-center items-center">
              <Icon name="ic:baseline-edit" class="w-5 h-5 text-gray-400" />
            </div>
          </div>
        </div>
        <div>
          <p class="text-lg">Passwort</p>
          <div class="relative w-full flex items-center">
            <InputText id="username" value="Schueler123!" type="password" fluid disabled />
            <div class="absolute right-0 cursor-pointer h-full aspect-square flex justify-center items-center">
              <Icon name="ic:baseline-edit" class="w-5 h-5 text-gray-400" />
            </div>
          </div>

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