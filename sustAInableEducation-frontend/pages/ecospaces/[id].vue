<template>
    <div class="w-full h-full">
        <div class="background animate-anim" />
        <div class="w-screen flex flex-col justify-center items-center h-full bg-slate-50 pt-[4.5rem] p-4">
            <InviteDialog v-model="inviteDialogIsVisible" :joinCode="joinCode" v-on:generateCode="getJoinCode" />
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
                <div class="content h-full w-full">
                    
                </div>
                <div class="controls flex flex-col m-4">
                    <Divider />
                    <div class="timer">
                        <Timer class="sm:hidden" v-model="timerValue" />
                    </div>
                    <div class="flex flex-col sm:flex-row sm:justify-between w-full">
                        <Button class="mb-2 sm:mb-0 sm:mr-5 flex-1 sm:!text-2xl" label="Option 1" @click="setTimer" />
                        <Button class="mb-2 sm:mb-0 sm:mx-5 flex-1 sm:!text-2xl" label="Option 2" @click="startTimer" />
                        <Knob class="hidden sm:block mx-5" v-model="timerValue.percent"
                            :valueTemplate="(number) => { return `${timerValue.time}` }" disabled :size="100">
                        </Knob>
                        <Button class="mb-2 sm:mb-0 sm:mx-5 flex-1 sm:!text-2xl" label="Option 3" @click="" />
                        <Button class="sm:ml-5 flex-1 sm:!text-2xl" label="Option 4" />
                    </div>

                </div>
            </div>

        </div>
    </div>
</template>

<script setup lang="ts">
import type { Participant } from '~/types/EcoSpace';

const runtime = useRuntimeConfig()

definePageMeta({
    validate: async (route) => {
        var spaceId = route.path.substring(route.path.lastIndexOf('/') + 1);

        var isValid = false

        await $fetch(`http://localhost:8080/spaces/${spaceId}`, {
            method: 'GET',
            credentials: 'include',
            headers: useRequestHeaders(['cookie']),
            onResponse: (response) => {
                if(response.response.ok) {
                    isValid = true;
                } else {
                    isValid = false;
                }
            }
        }).then(() => {
            return isValid;
        })
        .catch(() => {
            return isValid
        })

        return isValid
    }
})

const participants = ref<Participant[]>([])

const timerValue = ref({
    initialValue: 10,
    time: 10,
    percent: 0,
});

const inviteDialogIsVisible = ref<boolean | undefined>(false);

const userDialogIsVisible = ref<boolean | undefined>(false);

const joinCode = ref<string>('');

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

async function getJoinCode() {
    joinCode.value = "123456" // TODO: Implement
    /*
    $fetch(`${runtime.public.apiUrl}/${}`, {
        method: 'POST',
        body: JSON.stringify({ joinCode: joinCode.value }),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        */
}

</script>