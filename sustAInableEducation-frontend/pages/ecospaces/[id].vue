<template>
    <div class="w-full h-full">
        <div class="background animate-anim" />
        <div class="w-screen flex flex-col justify-center items-center h-full bg-slate-50 pt-[4.5rem] p-4">
            <InviteDialog v-model="inviteDialogIsVisible" :joinCode="joinCode" :expirationDate="expirationDate"
                v-on:generateCode="getJoinCode" />
            <UserDialog v-model="userDialogIsVisible" :participants="participants" />
            <div class="top flex justify-between items-center mb-2 w-full">
                <Button label="Einladen" @click="showInviteDialog">
                    <template #icon>
                        <Icon name="ic:baseline-person-add" class="size-5" />
                    </template>
                </Button>
                <Button label="Teilnehmer" badge="20" @click="showUserDialog" />
            </div>
            <div
                class="panel w-full h-full rounded-xl relative border-solid border-slate-3s00 border-2 flex flex-col justify-center">
                <div class="content h-full mt-4 mx-4 relative overflow-y-scroll">
                    <div class="hostcontrols w-full flex justify-end absolute" v-if="role === 'host'">
                        <Button label="Start (Generate)" @click="generatePart" size="small" class="mx-2"/>
                        <Button label="Start Voting" @click="" size="small"/>
                    </div>
                    <div v-for="part,index in parts" class="p-4">
                        <h1 class="text-3xl font-bold mb-2">{{ `${index+1}: ${part.intertitle}` }}</h1>
                        <p class="text-lg mb-4">{{ part.text }}</p>
                        <ul class="list-disc text-lg">
                            <li v-for="choice in part.choices">
                                <p>{{ `${choice.number}: ${choice.text}` }}</p>
                            </li>
                        </ul>
                        <Divider/>
                    </div>
                    <div>
                        <Skeleton height="8rem" v-if="isLoading"/>
                    </div>
                </div>
                <div class="controls flex flex-col m-4">
                    <Divider />
                    <div class="timer">
                        <Timer class="sm:hidden" v-model="timerValue" />
                    </div>
                    <div class="flex flex-col sm:flex-row sm:justify-between w-full">
                        <Button class="mb-2 sm:mb-0 sm:mr-5 flex-1 sm:!text-2xl" label="Option 1"
                            @click="selectOption(1)" />
                        <Button class="mb-2 sm:mb-0 sm:mx-5 flex-1 sm:!text-2xl" label="Option 2"
                            @click="selectOption(2)" />
                        <Knob class="hidden sm:block mx-5" v-model="timerValue.percent"
                            :valueTemplate="(number) => { return `${timerValue.time}` }" disabled :size="100">
                        </Knob>
                        <Button class="mb-2 sm:mb-0 sm:mx-5 flex-1 sm:!text-2xl" label="Option 3"
                            @click="selectOption(3)" />
                        <Button class="sm:ml-5 flex-1 sm:!text-2xl" label="Option 4" @click="selectOption(4)" />
                    </div>
                    {{ role }}
                </div>
            </div>

        </div>
    </div>
</template>

<script setup lang="ts">
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import type { Participant, Part } from '~/types/EcoSpace';

const runtime = useRuntimeConfig()
const route = useRoute()
const id = route.params.id

const users = ref<Participant[]>([])

const cookieHeaders = useRequestHeaders(['cookie'])

const role = ref<string>('')

const enableStart = ref(true)

const isLoading = ref(false)
const parts = ref<Part[]>([])

await getSpace()

const connection = new HubConnectionBuilder()
    .withUrl(`${runtime.public.apiUrl}/spaceHub/${id}`, {
        withCredentials: true,
    })
    .configureLogging(LogLevel.Information)
    .build();

async function generatePart() {
    await connection.invoke("GeneratePart")
}

async function selectOption(number: Number) {
    if (parts.value.length > 0)
        await connection.invoke("SetChoice", number)
    await connection.invoke("GeneratePart")
}

async function voteOption(number: Number) {
    await connection.invoke("Vote", number)
}

connection.on("PartGenerating", () => {
    if(parts.value.length > 0) {
        enableStart.value = false
    }

    isLoading.value = true
})
connection.on("PartGenerated", (part: Part) => {
    isLoading.value = false
    parts.value.push(part)
})

async function startConnection() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        navigateTo('/login')
    }
};

startConnection()

const participants = ref<Participant[]>([])

const timerValue = ref({
    initialValue: 10,
    time: 10,
    percent: 0,
});

const inviteDialogIsVisible = ref<boolean | undefined>(false);

const userDialogIsVisible = ref<boolean | undefined>(false);

const joinCode = ref<string>('');
const expirationDate = ref<string>('');

var timerCount = 0;

var timerInterval: NodeJS.Timeout;

function setTimer() {
    timerValue.value.time = timerValue.value.initialValue;
    timerValue.value.percent = 0;
}

function startTimer() {
    if (timerCount > 0) {
        return;
    }
    let increment = 100 / timerValue.value.time;
    timerCount++;
    timerInterval = setInterval(() => {
        if (timerValue.value.time <= 0) {
            clearInterval(timerInterval);
            timerCount--;
            return;
        }
        timerValue.value.time--;
        timerValue.value.percent += increment;
    }, 1000);
}

function showUserDialog() {
    userDialogIsVisible.value = true;
}

function showInviteDialog() {
    inviteDialogIsVisible.value = true;
}

async function getSpace() {
    await $fetch(`${runtime.public.apiUrl}/spaces/${id}`, {
        credentials: 'include',
        headers: useRequestHeaders(['cookie']),
        onResponse: (response) => {
            if (response.response.ok) {
                users.value = response.response._data.participants
                parts.value = response.response._data.story.parts
                if(parts.value.length > 0) {
                    enableStart.value = false
                }
            } else {
                console.log("ERROR")
            }
        },
    })

    await $fetch(`${runtime.public.apiUrl}/account`, {
        credentials: 'include',
        headers: cookieHeaders,
        onResponse: (response) => {
            if (response.response.ok) {
                if(response.response._data.anonUserName ) {
                    let participantEntry = users.value.find((participant) => participant.userName === response.response._data.anonUserName)
                    if(participantEntry) {
                        if(participantEntry.isHost) {
                            role.value = 'host'
                        } else {
                            role.value = 'participant'
                        }
                    }
                }
            } else {
                console.log("ERROR")
            }
        },
    })
}

async function getJoinCode() {
    $fetch(`${runtime.public.apiUrl}/spaces/${id}/open`, {
        method: 'POST',
        credentials: 'include',
        body: JSON.stringify({ joinCode: joinCode.value }),
        onResponse: (response) => {
            if (response.response.ok) {
                joinCode.value = response.response._data.code
                expirationDate.value = response.response._data.expiresAt
            } else {
                console.log("ERROR")
            }
        },
    })
}

</script>