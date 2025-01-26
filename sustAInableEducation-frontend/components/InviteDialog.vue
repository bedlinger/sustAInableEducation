<template>
    <Dialog v-model:visible="model" :draggable="false" modal header="Teilnehmer einladen"
        class="w-96 sm:w-96 m-4 sm:m-0 min-h-96">
        <div v-if="props.joinCode">
            <Panel class="w-full h-full !p-0">
                <div class="flex justify-center items-center">
                    <qrcode-svg :value="`${runtimeConfig.public.hostUrl}/join/${joinCode}`" :size="300" level="H" />
                </div>
            </Panel>
            <div class="flex justify-center items-center mt-4">
                <InputText class="!text-3xl w-36 !text-center" :value="formatJoinCode()" size="large" readonly />
                <Button class="!aspect-square !w-fit !h-fit !p-[17px] ml-2" rounded>
                    <Icon name="ic:baseline-content-copy" class="size-6" @click="copyJoinCode" />
                </Button>
            </div>
            <Divider />
            <div class="w-full flex justify-between items-center">
                <span>Läuft ab in {{ codeTimer }}</span>
                <Button severity="secondary" label="Neu Generieren" @click="generateCode" />
            </div>
        </div>
        <div v-else class="flex justify-center items-center w-full h-96">
            <Button label="Einladungscode generieren" @click="emits('generateCode')" />
        </div>

    </Dialog>
</template>

<script setup lang="ts">
import { QrcodeSvg } from 'qrcode.vue'

const runtimeConfig = useRuntimeConfig()

const model = defineModel<boolean>();
const props = defineProps<{ joinCode: string, expirationDate: string }>();
const emits = defineEmits(['generateCode']);

const codeTimerSeconds = ref<number>(0)
const interval = ref<NodeJS.Timeout>()

watch(model, (newVal, oldVal) => {
    if (newVal) {
        if(props.expirationDate) {
            setCodeTimer()
        }
    } else {
        clearInterval(interval.value)
    }
})

watch(() => props.expirationDate, (newVal, oldVal) => {
    if (newVal) {
        setCodeTimer()
    }
})

const codeTimer = computed(() => {
    var minutes = String(Math.floor(codeTimerSeconds.value / 60)).padStart(2, '0');
    var seconds = String(Math.floor(codeTimerSeconds.value % 60)).padStart(2, '0');
    return `${minutes}:${seconds}`;
})

function formatJoinCode() {
    return `${props.joinCode.slice(0, 3)}-${props.joinCode.slice(3, 7)}`
}

function copyJoinCode() {
    navigator.clipboard.writeText(props.joinCode)
}

function setCodeTimer() {
    clearInterval(interval.value)
    console.log(`EXPIRATION: ${props.expirationDate}`)
    let expirationDate = Date.parse(props.expirationDate)

    interval.value = setInterval(function () {

        let now = new Date().getTime()
        let distance = expirationDate - now
        codeTimerSeconds.value = Math.floor(distance / 1000)

        if (codeTimerSeconds.value <= 0) {
            clearInterval(interval.value)
        }

    }, 1000)
}

function generateCode() {
    emits('generateCode')
}

</script>