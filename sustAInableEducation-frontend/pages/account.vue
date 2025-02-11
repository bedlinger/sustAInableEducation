<template>
  
  <div class="w-full h-full flex justify-center items-center mt-16">
    <Toast />
    <PictureDialog v-model:visible="showPictureDialog" @success="pictureSuccess" @fail="pictureFail" />
    <ChangePasswordDialog v-model:visible="showChangePasswordDialog" @success="passSuccess" @fail="passFail" />
    <ChangeEmailDialog v-model:visible="showChangeEmailDialog" @success="emailSuccess" @fail="emailFail" />
    <Background/>
    <div class="bg-white relative flex justify-center p-5 rounded-lg shadow-xl w-full mx-4 sm:mx-0 sm:w-fit">
      <div
        class="pp absolute top-[-4.5rem] rounded-full overflow-hidden border-4 border-white shadow-md flex justify-center items-center">
        <Image :src="profileImage" width="128" alt="Profilbild" preview>
          <template #previewicon>
            <div />
          </template>
        </Image>
      </div>
      <div class="flex flex-col mt-16 gap-4 w-full sm:min-w-80">
        <div>
          <p class="text-lg">Benutzername</p>
          <InputText id="username" v-model="username" type="text" fluid disabled />
        </div>
        <div>
          <p class="text-lg">Email</p>
          <InputText id="username" v-model="email" type="email" fluid disabled />
        </div>
        <div>
          <p class="text-lg">Passwort</p>
          <Password id="username" v-model="password" fluid disabled />
        </div>
        <div class="flex flex-col gap-2">
          <Divider />
          <Button label="Email ändern" outlined @click="showChangeEmailDialog = true" />
          <Button label="Passwort ändern" outlined @click="showChangePasswordDialog = true" />
          <Button label="Profilbild generieren" outlined @click="showPictureDialog = true" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Meta } from '#build/components'
import type { Account } from '~/types/Account'

useHead({
  title: 'Kontoübersicht - sustAInableEducation'
})

const toast = useToast()
const route = useRoute()
const router = useRouter()
const runtimeConfig = useRuntimeConfig()

const headers = useRequestHeaders(['cookie'])

// Change password dialog
const showChangePasswordDialog = ref(false)
const showChangeEmailDialog = ref(false)
const showPictureDialog = ref(false)

const username = ref('USERNAME')
const email = ref('EMAIL@EMAIL.COM')
const password = ref('sustAInableEducation')
const profilePicture = ref<string | null>(null)

const fetchSuccessfull = ref(false)

const profileImage = computed(() => {
  return data.value?.profileImage ? `${runtimeConfig.public.apiUrl}${data.value.profileImage}` : '/img/profilepicture_placeholder.jpg'
})


const { data, error, refresh } = useFetch<Account | null>(`${runtimeConfig.public.apiUrl}/account`, {
  method: 'GET',
  credentials: 'include',
  headers: headers
})

if(error.value) {
  navigateTo(`/login?redirect=${route.fullPath}`)
}

if(data.value) {
  username.value = data.value.anonUserName
  email.value = data.value.email
  profilePicture.value = data.value.profileImage
}

const passSuccess = () => {
  toast.add({ severity: 'success', summary: 'Passwort geändert', detail: 'Das Passwort wurde erfolgreich geändert', life: 5000 })
}

const passFail  = () => {
  toast.add({ severity: 'error', summary: 'Fehler', detail: 'Das Passwort konnte nicht geändert werden', life: 5000 })
}

async function emailSuccess() {
  toast.add({ severity: 'success', summary: 'E-Mail geändert', detail: 'Die E-Mail wurde erfolgreich geändert', life: 5000 })
  await refresh()
  if(data.value) {
    email.value = data.value.email
  }
}

const emailFail  = () => {
  toast.add({ severity: 'error', summary: 'Fehler', detail: 'Die E-Mail konnte nicht geändert werden', life: 5000 })
}

const pictureSuccess = async () => {
  toast.add({ severity: 'success', summary: 'Profilbild generiert', detail: 'Das Profilbild wurde erfolgreich geändert', life: 5000 })
  await refresh()
  if(data.value) {
    profilePicture.value = data.value.profileImage
  }
}

const pictureFail  = () => {
  toast.add({ severity: 'error', summary: 'Fehler', detail: 'Das Bild konnte nicht generiert werden', life: 5000 })
}

</script>