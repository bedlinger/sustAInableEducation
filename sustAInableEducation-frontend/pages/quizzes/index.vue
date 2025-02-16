<template>
    <div class="w-full h-full">
        <div class="w-screen flex items-center h-full bg-slate-50 relative">
            <Toast />
            <div class="sidebar-container w-80 h-full pt-16 border-solid border-slate-300 border-r-2 hidden sm:block">
                <div class="sidebar w-full h-full flex-col p-2 overflow-y-scroll flex">
                    <div id="sidebar-header">
                        <div class="flex items-center">
                            <IconField class="mr-2">
                                <InputIcon>
                                    <Icon name="ic:baseline-search" />
                                </InputIcon>
                                <InputText placeholder="Suchen" v-model="searchInput" class="w-full" />
                            </IconField>
                        </div>
                    </div>
                    <div id="sidebar-content">
                        <QuizListEntry v-for="quiz in quizzes" :quiz="quiz" />
                        <NuxtLink to="/quizzes/configuration">
                            <Button label="Quiz erstellen" rounded size="small" class="w-full !text-">
                                <template #icon>
                                    <Icon name="ic:baseline-add" class="size-5" />
                                </template>
                            </Button>
                        </NuxtLink>
                        <div v-if="quizzes?.length === 0 || !quizzes" class="mt-2">
                            <Message class="text-md flex justify-center items-center w-full">
                                Es gibt noch keine Quizzes
                            </Message>
                        </div>
                    </div>
                </div>
            </div>
            <div class="content flex-1 h-full overflow-y-scroll">
                <div v-if="selectedQuiz" class="w-full pt-20 p-4">
                    <div class="flex items-start flex-col w-full h-full">


                    </div>
                </div>
                <div v-else class="pt-20 w-full h-full flex items-center justify-center">
                    <p class="text-lg">Bitte wählen Sie ein Quiz aus der Liste aus.</p>

                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import type { Quiz } from '~/types/Quiz';


const searchInput = ref('');

const quizzes = ref<Quiz[]>([]);
const selectedQuiz = ref<Quiz | null>(null);
</script>