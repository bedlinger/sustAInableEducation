<template>
  <Background/>
  <div class="w-full h-full flex justify-center items-center pt-20">
    <div v-if="quiz" id="content" class="w-full h-full bg-slate-50 p-4 flex flex-col">
      <div class="flex items-center justify-between">
        <h1 class="text-3xl">Quiz {{ buttonRefs }}</h1>
        <span class="text-3xl">1/{{ quiz.questions.length }}</span>
      </div>
      
      <div class="w-full bg-white border border-slate-300 h-full rounded-xl p-4 flex flex-col justify-center gap-4">
        <MeterGroup :value="value" class="">
          <template #label>
            <div></div>
          </template>
        </MeterGroup>
        <p class="text-2xl flex-1 text-center flex items-center" >{{ selectedQuestion.text }}</p>
        <div id="controls" class="flex flex-col gap-2 relative">
          <QuizButton v-for="choice, index in selectedQuestion.choices" v-model="buttonRefs[index]"
            :label="choice.text" class="w-full" :disabled="disableAnswerButtons" @click="handleButtonClick(index)"/>
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

const safeData = computed(() => quiz.value || { questions: [] });
const selectedQuestionIndex = ref(0);
const selectedQuestion = computed<Question>(() => safeData.value.questions[selectedQuestionIndex.value] || { id: '', number: 0, text: '', choices: [], isMultipleResponse: false });

const disableAnswerButtons = ref(false);
const value = ref([{ label: '', value: 10, color: 'var(--p-primary-color)' }]);

const buttonRefs = ref<boolean[]>([]);

watch(selectedQuestion, (newQuestion) => {3
  buttonRefs.value = newQuestion.choices.map(() => false);
}, { immediate: true });

function handleButtonClick(index: number) {
  if(!selectedQuestion.value.isMultipleResponse) {
    buttonRefs.value = buttonRefs.value.map(() => false);
    buttonRefs.value[index] = true;
  }
}
</script>