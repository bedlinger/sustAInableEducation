<template>
  <Background/>
  <div class="w-full h-full flex justify-center items-center pt-20">
    <div v-if="quiz" id="content" class="w-full h-full bg-slate-50 p-4 flex flex-col">
      <div class="flex items-center justify-between">
        <h1 class="text-3xl">Quiz</h1>
        <span class="text-3xl">1/{{ quiz.questions.length }}</span>
      </div>
      
      <div class="w-full bg-white border border-slate-300 h-full rounded-xl p-4 flex flex-col justify-center gap-4">
        <MeterGroup :value="value" class="">
          <template #label>
            <div></div>
          </template>
        </MeterGroup>
        {{ model }}
        <p class="text-2xl flex-1 text-center flex items-center" >{{ selectedQuestion.text }}</p>
        <div id="controls" class="flex flex-col gap-2 relative">
          <Button v-for="choice, index in selectedQuestion.choices"
            :label="choice.text" class="w-full" :disabled="disableAnswerButtons" @click="">
          <template #default>
            <p class="text-lg">{{ choice.text }}</p>
          </template>
          </Button>
        </div>
        <div class="w-full flex justify-center items-center">
          <Button label="Weiter" disabled/>
        </div>
      </div>
    </div>
    <div v-else class="w-full h-full flex justify-center items-center">
      

    </div>
  </div>
</template>

<script lang="ts" setup>
import type { Question, Quiz } from '~/types/Quiz';

const model = ref(false)

const requestHeaders = useRequestHeaders(['cookie']);
const route = useRoute();
const runtimeConfig = useRuntimeConfig();

const { data: quiz, refresh } = useFetch<Quiz>(`${runtimeConfig.public.apiUrl}/quizzes/${route.params.id}`, {
  method: 'GET',
  credentials: 'include',
  headers: requestHeaders,
  onResponse: (response) => {
    if(!response.response.ok) {
      if (response.response.status === 401) {
        navigateTo('/login?redirect=' + route.fullPath);
      } else {
        navigateTo('/quizzes');
      }
    }
  }
});

const selectedQuestionIndex = ref(0);
const selectedQuestion = computed<Question>(() => quiz.value?.questions[selectedQuestionIndex.value] || { id: '', number: 0, text: '', choices: [], isMultipleResponse: false });
const questionAnswered = ref(false)
const answeredCorrect = ref(false)

const disableAnswerButtons = ref(false)


const value = ref([{ label: '', value: 10, color: 'var(--p-primary-color)' }]);


async function validateAnswers() {
  disableAnswerButtons.value = true
  await $fetch(`${runtimeConfig.public.apiUrl}/quizzes/${route.params.id}/try`, {
    method: 'POST',
    credentials: 'include',
    headers: requestHeaders,
    body: [
      {
        questionId: selectedQuestion.value.id,
        response: [(index+1)]
      }
    ],
    onResponse: (response) => {
      if (response.response.ok) {
        questionAnswered.value = true
        if(response.response._data) {
          answeredCorrect.value = response.response._data.isCorrect
        } else {
          answeredCorrect.value = false
        }
      } else {
        disableAnswerButtons.value = false
        //TODO add Toast
      }
    }
  })
}



</script>