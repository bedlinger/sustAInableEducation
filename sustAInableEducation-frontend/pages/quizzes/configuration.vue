<template>
  <div class="w-full h-screen flex items-center justify-center">
      <Background/>
      <div class="w-screen flex flex-col items-center justify-center h-full">
          <div
              class="bg-slate-50 shadow-xl rounded-xl flex flex-col p-4 items-center w-full max-w-[1000px] h-full max-h-[650px] ">
              <div class="h-full max-h-[570px] flex flex-col items-center w-full">
                  <h1 class="text-3xl font-bold mb-8 h-5">Quiz erstellen</h1>
                  <Stepper value="1" class="flex-1 w-full h-full" linear>
                      <Steplist class="!w-full">
                          <Divider class="!hidden sm:!block" />
                          <Step value="1">EcoSpace</Step>
                          <Divider />
                          <Step value="2">Einstellungen</Step>
                          <Divider class="!hidden sm:!block" />
                      </Steplist>
                      <StepPanels class="!p-0 w-full h-full max-h-[520px]">
                          <StepPanel v-slot="{ activateCallback }" value="1" class="w-full h-full rounded-xl">
                              <div class="w-full h-full flex flex-col px-5 pt-5">
                                <h1 class="text-3xl font-bold mb-2">EcoSpace auswählen</h1>
                                <div class="w-full h-full flex flex-col justify-between ">
                                  <Listbox v-if="spaces.length > 0" v-model="selectedSpace" :options="spaces" optionLabel="story.title" class="!flex-1">
                                    <template #option="{ option }">
                                      <div class="flex flex-col w-full h-full">
                                        <span class="text-xl">{{ option.story.title }}</span>
                                        <div>
                                          <span>Erstellt am {{ formatDate(option.createdAt) }}</span>
                                        </div>
                                      </div>
                                    </template>
                                  </Listbox>
                                  <div class="w-full">
                                    <Divider/>
                                    <div class="flex justify-end">
                                      <Button label="Weiter" @click="activateCallback('2')" :disabled="!selectedSpace" />
                                    </div>
                                  </div>
                                </div>
                                
                              </div>
                          </StepPanel>
                          <StepPanel v-slot="{ activateCallback }" value="2" class="w-full h-full max-h-[520px]">
                              <div class="flex flex-col justify-between h-full  max-h-[520px] px-5 pt-5 bg-slate-50">
                                  <div class="flex-1">
                                    <div class="flex flex-col">
                                      <span class="text-xl">Ausgewählter Space</span>
                                      <InputText v-if="selectedSpace" v-model="selectedSpace.story.title" disabled/>
                                    </div>
                                    <div class="flex flex-col mt-4">
                                      <span class="text-xl">Fragentypen</span>
                                      <div class="flex pt-2 px-2">
                                        <Checkbox v-model="quizTypes" input-id="singleresponse" :value="0" class="mr-1" />
                                        <label for="singleresponse">Multiple Choice</label>
                                      </div>
                                      <div class="flex pt-2 px-2">
                                        <Checkbox v-model="quizTypes" input-id="multiresponse" :value="1" class="mr-1" />
                                        <label for="multiresponse">Multiple Choice (Mehrfachauswahl)</label>
                                      </div>
                                      <div class="flex pt-2 px-2">
                                        <Checkbox v-model="quizTypes" input-id="truefalse" :value="2" class="mr-1" />
                                        <label for="truefalse">Wahr/Falsch</label>
                                      </div>
                                    </div>
                                    <div class="flex flex-col mt-4">
                                      <span class="text-xl">Fragenanzahl</span>
                                      <InputNumber v-model="questionAmount" :min="1" :max="20" show-buttons />
                                    </div>
                                  </div>
                                  <div class="w-full">
                                      <Divider />
                                      <div class="flex items-center justify-between">
                                          <Button label="Zurück" severity="secondary"
                                              @click="activateCallback('1')" />
                                          <Button label="Quiz erstellen"
                                              :disabled="!isFilledOut || loading"
                                              @click="createQuiz"
                                              :loading="loading" />
                                      </div>
                                  </div>
                              </div>
                          </StepPanel>
                      </StepPanels>
                  </Stepper>
              </div>
          </div>
      </div>
  </div>
</template>

<script setup lang="ts">
import { Step } from 'primevue';
import { Configuration } from '~/types/configuration';
import type { EcoSpace } from '~/types/EcoSpace';
import { SdgAsset } from '~/types/sdgs';

useHead({
  title: 'EcoSpace erstellen - sustAInableEducation'
})

const headers = useRequestHeaders(['cookie'])
const runtimeConfig = useRuntimeConfig()
const route = useRoute()

const loading = ref(false)

const questionAmount = ref(1)
const quizTypes = ref();

const selectedSpace = ref();
const spaces = ref<EcoSpace[]>([]);

const { data, error, refresh} = useFetch<EcoSpace[]>(`${runtimeConfig.public.apiUrl}/spaces`, {
  method: 'GET',
  credentials: 'include',
  headers: useRequestHeaders(['cookie']),
})

if(error.value && error.value.statusCode === 401) {
  navigateTo(`/login?redirect=${route.fullPath}`)
}

if(data.value) {
  console.log("PRE")
  spaces.value = data.value.filter(space => {
    if(space.story) {
      return space.story.result !== null
    }
    return false
  })
  console.log("POST2")
}

function formatDate(date: string) {
  return new Date(date).toLocaleDateString('de-DE', { year: 'numeric', month: 'long', day: '2-digit' })
}

const isFilledOut = computed(() => {
  if(quizTypes.value) {
    if(quizTypes.value.length > 0) {
      return selectedSpace && questionAmount
    }
  }
  return false
})

async function createQuiz() {
  loading.value = true
  await $fetch(`${runtimeConfig.public.apiUrl}/quizzes`, {
    method: 'POST',
    credentials: 'include',
    headers: headers,
    body: {
      spaceId: selectedSpace.value.id,
      numberQuestions: questionAmount.value,
      types: quizTypes.value
    },
    onResponse: (response) => {
      if(response.response.ok) {
        navigateTo(`/quizzes/${response.response._data.id}`)
      }
    }
  })
  loading.value = false
}

</script>