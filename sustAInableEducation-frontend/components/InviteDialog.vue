<template>
    <Dialog v-model:visible="model" :draggable="false" modal header="Teilnehmer einladen" class="sm:w-96 m-4 sm:m-0 min-h-96">
        <div v-if="props.joinCode">
            <Panel class="w-full h-full !p-0">
                <div class="flex justify-center items-center">
                    <qrcode-svg :value="'https://app.sustainable-edu.at'" :size="300" level="H" />
                </div>
            </Panel>
            <div class="flex justify-center items-center mt-4">
                <InputText class="!text-3xl w-36 !text-center" :value="formatJoinCode()" size="large" readonly />
                <Button class="!aspect-square !w-fit !h-fit !p-[17px] ml-2" rounded>
                    <Icon name="ic:baseline-content-copy" class="size-6" @click="copyJoinCode" />
                </Button>
            </div>
            <Divider/>
            <div class="w-full flex justify-between items-center">
                <span>Läuft ab in 9:53 TODO</span>
                <Button severity="secondary" label="Neu Generieren" @click="emits('generateCode')"/>
            </div>
        </div>
        <div v-else class="flex justify-center items-center w-full bg-red-300 h-96">
            <Button label="Einladungscode generieren" @click="emits('generateCode')"/>
        </div>

    </Dialog>
</template>

<script setup lang="ts">
import { QrcodeSvg } from 'qrcode.vue'

const model = defineModel<boolean>();
const props = defineProps<{ joinCode: string }>();
const emits = defineEmits(['generateCode']);

function formatJoinCode() {
    return `${props.joinCode.slice(0, 3)}-${props.joinCode.slice(3, 7)}`
}

function copyJoinCode() {
    navigator.clipboard.writeText(props.joinCode)
}


</script>