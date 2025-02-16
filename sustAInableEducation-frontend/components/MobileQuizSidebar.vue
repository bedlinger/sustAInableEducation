<template>
  <div class="sidebar-container w-full flex absolute h-full pt-16 pb-3 z-10 bg-white" v-if="model">
    <div class="sidebar flex-1 w-full flex-col overflow-y-scroll flex">
      <div id="sidebar-header">
        <div class="w-full flex items-center justify-end py-2">
          <div @click="emit('toggleSidebar')" class="bg-slate-400 flex items-center opacity-80 justify-center p-2 pr-5 cursor-pointer rounded-tl-xl rounded-bl-xl">
            <Icon name="ic:sharp-keyboard-double-arrow-left" class="size-7"/>
          </div>
        </div>
        <div class="flex items-center mx-2">
          <IconField class="mr-2 flex-1">
            <InputIcon>
              <Icon name="ic:baseline-search" />
            </InputIcon>
            <InputText placeholder="Suchen" v-model="searchInputRef" class="w-full" />
          </IconField>
        </div>
        <Divider class="!w-full !mx-2" />
      </div>
      <div id="sidebar-content" class="flex flex-col">
        <QuizListEntry v-for="quiz in props.searchedQuizzes" :quiz="quiz" @delete="emit('openDeleteDialog')"
          v-model="quizRefsById[quiz.id].value" @click="emit('selectQuiz', quiz.id)" class="mx-2"/>
        <NuxtLink to="/configuration" class=" mx-2">
          <Button label="Quiz erstellen" rounded size="small" class="w-full">
            <template #icon>
              <Icon name="ic:baseline-add" class="size-5" />
            </template>
          </Button>
        </NuxtLink>
        <div v-if="searchedQuizzes.length === 0" class="mt-2">
          <Message class="text-md flex justify-center items-center w-full">
            Es gibt noch keine Quizzes
          </Message>
        </div>
      </div>
    </div>
  </div>
  <div class="absolute top-[4.5rem] opacity-80" v-else>
    <div @click="emit('toggleSidebar')" class="bg-slate-400 flex items-center justify-center p-2 pr-5 cursor-pointer rounded-tr-xl rounded-br-xl">
      <Icon name="ic:sharp-keyboard-double-arrow-right" class="size-7" @click="" />
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { Quiz } from '~/types/Quiz';

const model = defineModel<boolean>(); // opened/closed

const searchInputRef = ref('');

watch(searchInputRef, (newValue) => {
  emit('searchUpdate', newValue);
});

const props = defineProps<{
  searchedQuizzes: Quiz[],
  quizzes: Quiz[] | null,
  selectedQuiz: Quiz | null,
  quizRefsById: Record<string, globalThis.Ref<boolean, boolean>>
}>();

const emit = defineEmits(['selectQuiz', 'openDeleteDialog', 'toggleSidebar', 'searchUpdate']);

</script>