<template>
    <div class="w-full h-full">
        <div class="w-screen flex items-center h-full bg-slate-50 relative">
            <ConfirmDialog></ConfirmDialog>
            <Toast />
            <div class="sidebar-container w-80 h-full pt-16 border-solid border-slate-300 border-r-2 hidden sm:block">
                <div class="sidebar w-full h-full flex-col p-2 overflow-y-scroll flex">
                    <div id="sidebar-header">
                        <div class="flex items-center w-full">
                            <IconField class="w-full">
                                <InputIcon>
                                    <Icon name="ic:baseline-search" />
                                </InputIcon>
                                <InputText placeholder="Suchen" v-model="searchInput" fluid />
                            </IconField>
                        </div>
                        <Divider />
                    </div>
                    <div id="sidebar-content">
                        <QuizListEntry v-for="quiz in searchedQuizzes" :quiz="quiz"
                            v-model="quizRefsById[quiz.id].value" @click="selectQuizById(quiz.id)"
                            @delete="openDialog" />
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

            <MobileQuizSidebar class="sm:hidden" v-model="showSidebar" :searched-quizzes="searchedQuizzes"
                :search-input="searchInput" :quizzes="quizzes" :selected-quiz="selectedQuiz"
                :quiz-refs-by-id="quizRefsById" @open-delete-dialog="openDialog" @select-quiz="selectQuizById"
                @toggle-sidebar="showSidebar = !showSidebar" @search-update="updateSearch" />

            <div class="content flex-1 h-full overflow-y-scroll w-full">
                <div v-if="selectedQuiz" class="w-full h-full pt-20 p-4">
                    <div class="flex items-start flex-col w-full h-full">
                        <h1 class="text-4xl mb-4">{{ selectedQuiz.title }}</h1>
                        <Panel header="Informationen" class="!w-full mb-4">
                            <Divider />
                            <div class="text-lg flex flex-col">
                                <span>Anzahl der Fragen: {{ selectedQuiz.numberQuestions }}</span>
                                <div class="flex gap-3">
                                    <span>Ausgewählte Fragentypen: </span>
                                    <CheckboxGroup name="ingredient" class="flex flex-col gap-2">
                                        <div class="flex items-center gap-2">
                                            <Checkbox inputId="singleresponse" :value="0" disabled />
                                            <label for="singleresponse"> Multiple Choice </label>
                                        </div>
                                        <div class="flex items-center gap-2">
                                            <Checkbox inputId="multiresponse" :value="1" disabled />
                                            <label for="multiresponse"> Multiple Choice (Mehrfachauswahl) </label>
                                        </div>
                                        <div class="flex items-center gap-2">
                                            <Checkbox inputId="truefalse" :value="2" disabled />
                                            <label for="truefalse"> Wahr/Falsch </label>
                                        </div>
                                    </CheckboxGroup>
                                </div>
                            </div>
                        </Panel>
                        <div class="flex flex-col">
                            <h2 class="text-3xl">Versuche</h2>

                        </div>
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

const requestHeaders = useRequestHeaders(['cookie']);
const runtimeConfig = useRuntimeConfig();
const route = useRoute();

const confirmDialog = useConfirm();

const { refresh, data: quizzes } = await useFetch<Quiz[]>(`${runtimeConfig.public.apiUrl}/quizzes`,
    {
        method: 'GET',
        credentials: 'include',
        headers: requestHeaders,
        onResponse: (response) => {
            if (response.response.status === 401) {
                navigateTo('/login?redirect=' + route.fullPath);
            }
        }
    }
)

const selectedQuiz = ref<Quiz | null>(null);

const quizRefsById = quizzes.value ? quizzes.value.reduce((acc, quiz) => {
    acc[quiz.id] = ref(false);
    return acc;
}, {} as Record<string, Ref<boolean>>) : {};

const searchedQuizzes = computed(() => {
    if (quizzes.value === null) return [];
    return quizzes.value.filter((quiz) => quiz.title.toLowerCase().includes(searchInput.value.toLowerCase()));
})


const searchInput = ref('');

const showSidebar = ref(true);


function updateSearch(newVal: string) {
    searchInput.value = newVal;
}

function selectQuizById(id: string) {
    selectedQuiz.value = quizzes.value!.find((quiz) => quiz.id === id) || null;
    quizRefsById[id].value = true;
    Object.keys(quizRefsById).forEach(key => {
        if (key !== id) {
            quizRefsById[key].value = false;
        }
    });
    showSidebar.value = false;
}

async function deleteQuiz(id: string) {
    await $fetch(`${runtimeConfig.public.apiUrl}/quizzes/${id}`, {
        method: 'DELETE',
        credentials: 'include',
        headers: requestHeaders
    });
    refresh();
}

const openDialog = (id: string) => {
    confirmDialog.require({
        message: 'Sind Sie sich sicher, dass Sie dieses Quiz löschen möchten?',
        header: 'Endgültig Löschen',
        rejectProps: {
            label: 'Abbrechen',
            severity: 'secondary',
            outlined: true
        },
        acceptProps: {
            label: 'Löschen',
            severity: 'danger'
        },
        accept: () => {
            deleteQuiz(id);
        },
        reject: () => {

        }
    });
}
</script>