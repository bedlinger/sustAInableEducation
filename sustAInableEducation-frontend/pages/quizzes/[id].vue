<template>
  <Background/>
  <div class="w-full h-full flex justify-center items-center pt-20">
    <div v-if="quiz" id="content" class="w-full h-full bg-slate-50 p-4 flex flex-col">
      <div class="flex items-center justify-between">
        <h1 class="text-3xl">Quiz</h1>
        <span class="text-3xl">{{ selectedQuestionIndex + 1 }} / {{ quiz.questions.length }}</span>
      </div>
      
      <div class="w-full bg-white border border-slate-300 h-full rounded-xl p-4 flex flex-col justify-center gap-4 relative">
        <MeterGroup :value="meterValue">
          <template #label>
            <div></div>
          </template>
        </MeterGroup>
        <p class="text-2xl flex-1 text-center flex items-center" >{{ selectedQuestion.text }}</p>
        <div id="controls" class="flex flex-col gap-2 p-2">
          <QuizButton v-for="choice, index in selectedQuestion.choices" v-model="buttonRefs[index]"
            :label="choice.text" class="w-full" :disabled="disableAnswerButtons" @click="handleButtonClick(index)"/>
        </div>
        <div class="w-full flex justify-center items-center">
          <Button label="Weiter" :disabled="!refsIncludeTrue" @click="saveSelection" :class="result < 0 ? '' : 'invisible'"/>
        </div>
        <div v-if="result !== -1" class="p-4 flex flex-col items-center w-full h-full absolute top-0 left-0 rounded-xl bg-[rgba(255,255,255,0.5)]" style="backdrop-filter: blur(5px);">
          <p class="text-2xl text-center flex-1 flex justify-center items-center"> Du hast {{ result }}/{{ quiz.questions.length }} Fragen richtig beantwortet</p>
          <div>
            <Button label="Erneut versuchen" @click="resetQuiz" class="mr-2"/>
            <Button label="Zurück zur Übersicht" class="ml-2" severity="secondary" @click="navigateTo('/quizzes')"/>
          </div>
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

const selectedAnswers = ref<{questionId: string, response: number[]}[]>([]);

const disableAnswerButtons = ref(false);
const value = ref([{ label: '', value: 10, color: 'var(--p-primary-color)' }]);

const result = ref(-1)

const buttonRefs = ref<boolean[]>([]);

const refsIncludeTrue = computed(() => buttonRefs.value.includes(true));

watch(selectedQuestion, (newQuestion) => {3
  buttonRefs.value = newQuestion.choices.map(() => false);
}, { immediate: true });

const meterValue = computed(() => {
  let value = ((selectedQuestionIndex.value)/quiz.value!.questions.length) * 100
  if(result.value >= 0) {
    value = ((selectedQuestionIndex.value + 1)/quiz.value!.questions.length) * 100
  }
  return [{ label: '', value: value, color: 'var(--p-primary-color)' }];
});

async function handleButtonClick(index: number) {
  if(!selectedQuestion.value.isMultipleResponse) {
    buttonRefs.value = buttonRefs.value.map(() => false);
    buttonRefs.value[index] = true;
  }
}

async function getResult () {
  console.log(`${runtimeConfig.public.apiUrl}/quizzes/${route.params.id}/try`)
  console.table(selectedAnswers.value)
  await $fetch<{isCorrect: boolean}[]>(`${runtimeConfig.public.apiUrl}/quizzes/${route.params.id}/try`, {
    method: 'POST',
    credentials: 'include',
    headers: requestHeaders,
    body: JSON.stringify(selectedAnswers.value),
    onResponse({ response }) {
      if(response.ok) {
        result.value = response._data.filter((answer: { questioonId: string, isCorrect: boolean }) => answer.isCorrect).length;
      }
    }
  });
}

async function saveSelection() {
  const selected = selectedQuestion.value.choices.map((choice, index) => buttonRefs.value[index] ? choice.number : null).filter((id) => id !== null) as number[];
  selectedAnswers.value.push({questionId: selectedQuestion.value.id, response: selected});
  if(selectedQuestionIndex.value + 1 !== safeData.value.questions.length) {
    selectedQuestionIndex.value++;
  } else {
    disableAnswerButtons.value = true;
    await getResult();
  }
}

function resetQuiz() {
  selectedQuestionIndex.value = 0;
  selectedAnswers.value = [];
  result.value = -1;
  disableAnswerButtons.value = false;
}
</script>