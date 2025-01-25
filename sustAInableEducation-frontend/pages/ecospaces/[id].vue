<template>
    <div class="w-full h-full prose-lg">
        <div class="background animate-anim" />
        <div class="w-screen flex flex-col items-center h-full bg-slate-50 pt-[4.5rem] p-4">
            <InviteDialog v-model="inviteDialogIsVisible" :joinCode="joinCode" :expirationDate="joinExpirationDate"
                v-on:generateCode="getJoinCode" />
            <UserDialog v-model="userDialogIsVisible" :participants="space!.participants" />
            <div class="top flex justify-between items-center mb-2 w-full">
                <Button label="Einladen" @click="showInviteDialog">
                    <template #icon>
                        <Icon name="ic:baseline-person-add" class="size-5" />
                    </template>
                </Button>
                <div v-if="isVoting">VOTING TIME</div>
                <Button label="Teilnehmer" :badge="space?.participants.length.toString()" @click="showUserDialog" />
            </div>
            <div
                class="panel w-full h-[45rem] rounded-xl relative border-solid border-slate-300 border-2 flex flex-col justify-center">
                <div class="content h-[32rem] mt-4 mx-4 relative overflow-y-scroll" ref="contentDiv">
                    <div class="hostcontrols w-full flex justify-end absolute" v-if="role === 'host'">
                        <Button label="Start Voting" @click="startVoting" size="small" />
                    </div>
                    <div v-for="part, index in space?.story.parts" class="p-4">
                        <Divider v-if="index !== 0" />
                        <h2 class="font-bold mb-2">{{ `${index + 1}: ${part.intertitle}` }}</h2>
                        <p class="mb-4">{{ part.text }}</p>
                        <ul class="list-disc">
                            <li v-for="choice in part.choices"
                                :class="{ 'font-bold bg-primary-200 rounded-lg p-1': choice.number === part.chosenNumber }">
                                <span>{{ `${choice.number}: ${choice.text}` }}</span>
                            </li>
                        </ul>
                    </div>
                    <div class="w-full h-full flex justify-center items-center" v-if="parts.length === 0 && !isLoading">
                        <Button label="Start" @click="generatePart" severity="primary" />
                    </div>
                    <div v-if="isLoading">
                        <Divider v-if="parts.length > 0" />
                        <Skeleton height="3.25rem" width="40rem" class="mb-2" />
                        <Skeleton height="1.75rem" width="94%" class="mb-2" />
                        <Skeleton height="1.75rem" width="98%" class="mb-2" />
                        <Skeleton height="1.75rem" width="96%" class="mb-2" />
                        <Skeleton height="1.75rem" width="97%" class="mb-2" />
                        <Skeleton height="1.75rem" width="95%" class="mb-2" />
                        <Skeleton height="1.75rem" width="90%" class="mb-8" />
                        <Skeleton height="1.75rem" width="68%" class="mb-2" />
                        <Skeleton height="1.75rem" width="65%" class="mb-2" />
                        <Skeleton height="1.75rem" width="70%" class="mb-2" />
                        <Skeleton height="1.75rem" width="73%" class="mb-2" />
                    </div>
                    <div v-if="result" class="w-full" ref="resultAccordion">
                        <Divider />
                        <h2 class="font-bold mb-2">Ergebnis</h2>
                        <p>{{ result.text }}</p>
                        <Accordion :value="Array.from(Array(parts.length).keys())" multiple>
                            <AccordionPanel :value="0" key="options">
                                <AccordionHeader>Gewählte Optionen</AccordionHeader>
                                <AccordionContent>
                                    <div class="flex flex-col">
                                        <p>Sie haben {{ result.negativeChoices.length }} negative und {{
                                            result.positiveChoices.length }} positive Entscheidungen getroffen.</p>
                                        <div class="flex flex-row">
                                            <Fieldset legend="Positiv" class="flex-1 !mr-2"
                                                v-if="result.positiveChoices">
                                                <ul class="list-disc">
                                                    <li v-for="choice in result.positiveChoices">{{ choice }}</li>
                                                </ul>
                                            </Fieldset>
                                            <Fieldset legend="Negativ" class="flex-1 !ml-2"
                                                v-if="result.negativeChoices">
                                                <ul class="list-disc">
                                                    <li v-for="choice in result.negativeChoices">{{ choice }}</li>
                                                </ul>
                                            </Fieldset>
                                        </div>
                                    </div>
                                </AccordionContent>
                            </AccordionPanel>
                            <AccordionPanel :value="1" key="learnings">
                                <AccordionHeader>Erkenntnisse</AccordionHeader>
                                <AccordionContent>
                                    <div class="flex">
                                        <ul class="list-disc">
                                            <li v-for="learning in result.learnings">{{ learning }}</li>
                                        </ul>
                                    </div>
                                </AccordionContent>
                            </AccordionPanel>
                            <AccordionPanel :value="2" key="discussionQuestions">
                                <AccordionHeader>Diskussionsfragen</AccordionHeader>
                                <AccordionContent>
                                    <div class="flex">
                                        <ul class="list-disc">
                                            <li v-for="question in result.discussionQuestions">{{ question }}</li>
                                        </ul>
                                    </div>
                                </AccordionContent>
                            </AccordionPanel>
                        </Accordion>
                    </div>
                    <div v-if="showReloadButton && !result" class="w-full flex justify-center items-center">
                        <Button @click="generatePart" severity="secondary">
                            <template #default>
                                <Icon name="ic:baseline-refresh" class="size-5" />
                                <span>Erneut versuchen</span>
                            </template>
                        </Button>
                    </div>
                </div>
                <div class="controls flex flex-col m-4" v-if="role === 'host'">
                    <Divider />
                    <div class="timer">
                        <Timer class="sm:hidden" v-model="timerValue" />
                    </div>
                    <div class="flex flex-col sm:flex-row sm:justify-between w-full">
                        <div class="w-full mb-2 sm:mb-0 sm:mx-5 flex-1">
                            <HostButton label="Option 1" :disabled="disableOptionButtons" :percentage="percentages[0]"
                                @click="selectOption(1)" :voting="showPercentages" />
                        </div>
                        <div class="w-full mb-2 sm:mb-0 sm:mx-5 flex-1">
                            <HostButton label="Option 2" :disabled="disableOptionButtons" :percentage="percentages[1]"
                                @click="selectOption(2)" :voting="showPercentages" />
                        </div>
                        <Knob class="hidden sm:block mx-5" v-model="timerValue.percent"
                            :valueTemplate="(number) => { return `${timerValue.time}` }" disabled :size="100">
                        </Knob>
                        <div class="w-full mb-2 sm:mb-0 sm:mx-5 flex-1">
                            <HostButton label="Option 3" :disabled="disableOptionButtons" :percentage="percentages[2]"
                                @click="selectOption(3)" :voting="showPercentages" />
                        </div>
                        <div class="w-full mb-2 sm:mb-0 sm:mx-5 flex-1">
                            <HostButton label="Option 4" :disabled="disableOptionButtons" :percentage="percentages[3]"
                                @click="selectOption(4)" :voting="showPercentages" />
                        </div>
                    </div>
                </div>
                <div class="controls flex flex-col m-4 text-xl" v-else>
                    <Divider />
                    <div class="timer">
                        <Timer class="sm:hidden" v-model="timerValue" />
                    </div>
                    <div class="flex flex-col sm:flex-row sm:justify-between w-full">
                        <Button class="mb-2 sm:mb-0 sm:mr-5 flex-1 sm:!text-2xl" label="Option 1" @click="voteOption(1)"
                            :disabled="disableOptionButtons" />
                        <Button class="mb-2 sm:mb-0 sm:mr-5 flex-1 sm:!text-2xl" label="Option 2" @click="voteOption(2)"
                            :disabled="disableOptionButtons" />
                        <Knob class="hidden sm:block mx-5" v-model="timerValue.percent"
                            :valueTemplate="(number) => { return `${timerValue.time}` }" disabled :size="100">
                        </Knob>
                        <Button class="mb-2 sm:mb-0 sm:mr-5 flex-1 sm:!text-2xl" label="Option 3" @click="voteOption(3)"
                            :disabled="disableOptionButtons" />
                        <Button class="mb-2 sm:mb-0 sm:mr-5 flex-1 sm:!text-2xl" label="Option 4" @click="voteOption(4)"
                            :disabled="disableOptionButtons" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import type { Part, EcoSpace, Result, Choice } from '~/types/EcoSpace';

const runtime = useRuntimeConfig()
const route = useRoute()
const router = useRouter()
const id = route.params.id



const space = ref<EcoSpace | null>(null)

const hasVoted = ref(false)
const isVoting = ref(false)
const percentages = ref([0, 0, 0, 0])
const showPercentages = ref(false)

const parts = computed(() => {
    if (space.value)
        return space.value?.story.parts
    return []
})

const disableOptionButtons = computed(() => {
    if (role.value === 'host') {
        return !!space.value?.story.result || isLoading.value || parts.value.length === 0 || isVoting.value
    }
    return !isVoting.value || hasVoted.value
})

const result = computed(() => {
    if (space.value)
        return space.value?.story.result
    return null
})

const cookieHeaders = useRequestHeaders(['cookie'])

const role = ref<string>('')

const resultAccordion = ref<HTMLDivElement | null>(null);

const enableStart = ref(true)

const isLoading = ref(false)

const showReloadButton = ref(false)

await getSpace()

const connection = new HubConnectionBuilder()
    .withUrl(`${runtime.public.apiUrl}/spaceHub/${id}`, {
        withCredentials: true,
    })
    .configureLogging(LogLevel.Information)
    .build();

async function generatePart() {
    if (showReloadButton.value)
        showReloadButton.value = false
    try {
        await connection.invoke("GeneratePart")
    } catch (err) {
        isLoading.value = false
        showReloadButton.value = true
    }

}

async function selectOption(number: Number) {
    if (space.value !== null) {
        if (parts.value.length > 0) {
            try {
                await connection.invoke("SetChoice", number)
            } catch (err) { }

            try {
                await connection.invoke("GeneratePart")
            } catch (err) {
                console.error("ALARM")
                isLoading.value = false
                showReloadButton.value = true
            }
        }
    }
}

async function voteOption(number: Number) {
    try {
        await connection.invoke("Vote", number)
        hasVoted.value = true
    } catch (err) {
        console.error("There was an error voting")
    }
}

async function startVoting() {
    await connection.invoke("StartVoting")
}

if (role.value === 'host') {
    connection.on("VotingUpdated", async (choices: Choice[]) => {
        let sum = choices.reduce((a, b) => a + b.numberVotes, 0)
        percentages.value = choices.map((choice) => Math.round((choice.numberVotes / sum) * 100))
    })
}

connection.on("PartGenerating", async () => {
    if (parts.value.length > 0) {
        enableStart.value = false
    }
    showPercentages.value = false
    isLoading.value = true
    await nextTick()
    scrollToBottom()
})
connection.on("PartGenerated", async (part: Part) => {
    isLoading.value = false
    resetTimer()
    parts.value.push(part)
    await nextTick()
    scrollToBottom()
})

connection.on("ResultGenerated", async (result: Result) => {
    space.value!.story.result = result
    isLoading.value = false
    await nextTick()
    scrollToResult()
})

connection.on("VotingStarted", async (expirationStr: string) => {
    hasVoted.value = false
    isVoting.value = true
    showPercentages.value = true
    percentages.value = [0, 0, 0, 0]
    startTimer(expirationStr)
})

connection.on("ErrorOccured", (msg: string) => {
    isLoading.value = false;
    showReloadButton.value = true;
})

connection.on("ChoiceSet", (choice: number) => {
    parts.value[parts.value.length - 1].chosenNumber = choice
})

async function startConnection() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        router.push('/login')
    }
};
if (import.meta.client) {
    startConnection()
}

const timerValue = ref({
    initialValue: space.value!.votingTimeSeconds,
    time: space.value!.votingTimeSeconds,
    percent: 100,
});

const inviteDialogIsVisible = ref<boolean | undefined>(false);

const userDialogIsVisible = ref<boolean | undefined>(false);

const joinCode = ref<string>('');
const joinExpirationDate = ref<string>('');

var timerInterval: NodeJS.Timeout;

function resetTimer() {
    timerValue.value.time = timerValue.value.initialValue;
    timerValue.value.percent = 100;
}

function startTimer(expirationStr: string) {
    clearInterval(timerInterval)
    let expirationDate = Date.parse(expirationStr)

    timerInterval = setInterval(function () {

        let now = new Date().getTime()
        let distance = expirationDate - now
        timerValue.value.time = Math.floor(distance / 1000)
        timerValue.value.percent = Math.round((timerValue.value.time / timerValue.value.initialValue) * 100)

        if (timerValue.value.time <= 0) {
            isVoting.value = false
            clearInterval(timerInterval)
        }

    }, 1000)
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
                space.value = response.response._data
                if (parts.value.length > 0) {
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
                if (response.response._data.anonUserName) {
                    let participantEntry = space.value?.participants.find((participant) => participant.userName === response.response._data.anonUserName)
                    if (participantEntry) {
                        if (participantEntry.isHost) {
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
                joinExpirationDate.value = response.response._data.expiresAt
            } else {
                console.log("ERROR")
            }
        },
    })
}

const contentDiv = ref<HTMLDivElement | null>(null);
const scrollToBottom = () => {
    const el = contentDiv.value;

    if (el) {
        el.scrollTo({
            top: el.scrollHeight,
            behavior: 'smooth'
        })
    }
};

const scrollToResult = () => {
    const contentDivVal = contentDiv.value;
    const resultVal = resultAccordion.value;

    if (contentDivVal && resultVal) {
        // Calculate the target's position relative to the container
        const targetPosition = resultVal.offsetTop - contentDivVal.offsetTop;

        // Scroll the container to the target's position
        contentDivVal.scrollTo({
            top: targetPosition,
            behavior: 'smooth',
        });
    }
};

function printLogs() {
    console.table(parts.value)
}
</script>