<template>
    <div class="w-full h-screen flex items-center justify-center">
        <div class="background animate-anim" />
        <div class="w-screen flex flex-col items-center justify-center h-full">
            <div
                class="bg-slate-50 shadow-xl rounded-xl flex flex-col p-4 items-center w-full max-w-[1000px] h-full max-h-[650px] ">
                <div class="h-full max-h-[570px] flex flex-col items-center w-full">
                    <h1 class="text-3xl font-bold mb-8 h-5">EcoSpace erstellen</h1>
                    <Stepper value="1" class="flex-1 w-full h-full" linear>
                        <Steplist>
                            <Divider />
                            <Step value="1">Thema</Step>
                            <Divider />
                            <Step value="2">Einstellungen</Step>
                            <Divider />
                        </Steplist>
                        <StepPanels class="!p-0 w-full h-full">
                            <StepPanel v-slot="{ activateCallback }" value="1" class="w-full h-full">
                                <Tabs value="0" class="w-full h-full flex">
                                    <TabList class="flex flex-none">
                                        <Tab value="0" as="div"
                                            class="h-10 flex-1 flex justify-center items-center !bg-white !rounded-tl-xl">
                                            <span>SDGs</span>
                                        </Tab>
                                        <Tab value="1" as="div"
                                            class="h-10 flex-1 flex justify-center items-center !bg-white !rounded-tr-xl">
                                            <span>Eigenes Thema</span>
                                        </Tab>
                                    </TabList>
                                    <TabPanels class="!p-0 !pt-2 !px-4 w-full flex-1">
                                        <TabPanel value="0" class="flex flex-wrap flex-col h-full w-full">
                                            <Panel class="h-full max-h-[395px] w-full overflow-y-scroll">
                                                <div class="w-full flex flex-wrap justify-center">
                                                    <img v-for="sdg in sdgAssets" :src="sdg.asset_path" :alt="sdg.name"
                                                        class="w-40 h-40 m-2 cursor-pointer"
                                                        :class="{ 'box-border border-4 border-white': (sdg.id === selectedSdg) }"
                                                        @click="selectSdg(sdg.id)" />
                                                </div>
                                            </Panel>
                                            <div class="w-full">
                                                <Divider />
                                                <div class="flex items-center justify-end "
                                                    :class="{ 'justify-between': selectedSdg != -1 }">
                                                    <p v-if="selectedSdg != -1"> Ausgewählt: {{
                                                        getSdgAsset(selectedSdg)?.name }}</p>
                                                    <Button label="Next"
                                                        v-tooltip.bottom="{ value: (selectedSdg === -1) ? 'Es muss eine Auswahl getroffen werden' : null }"
                                                        :disabled="selectedSdg === -1" @click="activateCallback('2')" />
                                                </div>
                                            </div>
                                        </TabPanel>
                                        <TabPanel value="1" class="flex flex-wrap flex-col h-full w-full">
                                            <Panel class="h-full max-h-[395px] w-full">
                                                <div class="text-xl flex flex-col w-full">
                                                    <div>
                                                        <div class="flex items-center">
                                                            <div class="flex items-center">
                                                                <span>
                                                                    Das Thema, welches die Geschichte behandeln soll, ist
                                                                    das
                                                                </span>
                                                                
                                                                <div class="flex ml-1 items-center">
                                                                    <Button class="!text-xl !p-0" @click="focusTextarea"
                                                                        label="Thema" variant="link" />
                                                                    <Textarea id="description" class="ml-2 resize-none" v-model="topicInput"
                                                                        rows="1" cols="20" autoResize />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <span>
                                                            und dieses Thema soll folgende Punkte
                                                            thematisieren:
                                                        </span>
                                                    </div>
                                                    <ul class="list-decimal ml-10 flex flex-col max-w-96 mt-2">
                                                        <li v-for="ref, index in bulletPoints" class="mb-2">
                                                            <div class="flex items-center justify-between">
                                                                <Textarea class="w-fit" v-model="ref.value" rows="1"
                                                                    cols="50" autoResize />
                                                                <div v-if="bulletPoints.length != 1"
                                                                    @click="removeBulletPoint(index)"
                                                                    class="ml-2 rounded-full border-2 border-solid border-red-500 size-fit flex justify-center cursor-pointer">
                                                                    <Icon name="ic:baseline-minus"
                                                                        class="bg-red-500 size-5" />
                                                                </div>

                                                            </div>
                                                        </li>
                                                        <Button class="!p-0" rounded @click="addBulletPoint()"
                                                            v-if="bulletPoints.length < 3">
                                                            <Icon name="ic:baseline-plus"
                                                                class="bg-white mx-4 size-6" />
                                                        </Button>
                                                    </ul>
                                                </div>
                                            </Panel>
                                            <div class="w-full">
                                                <Divider />
                                                <div class="flex items-center justify-end">
                                                    <Button label="Next"
                                                        v-tooltip.bottom="{ value: topicTooltip }"
                                                        :disabled="!topicFilledOut" @click="activateCallback('2')" />
                                                </div>
                                            </div>
                                        </TabPanel>
                                    </TabPanels>
                                </Tabs>

                            </StepPanel>
                            <StepPanel v-slot="{ activateCallback }" value="2">
                                <div class="flex flex-col h-48">
                                    <div
                                        class="border-2 border-dashed border-surface-200 dark:border-surface-700 rounded bg-surface-50 dark:bg-surface-950 flex-auto flex justify-center items-center font-medium">
                                        Content II</div>
                                </div>
                                <div class="flex pt-6 justify-between">
                                    <Button label="Back" severity="secondary" icon="pi pi-arrow-left"
                                        @click="activateCallback('1')" />
                                    <Button label="Next" icon="pi pi-arrow-right" iconPos="right"
                                        @click="activateCallback('3')" />
                                </div>
                            </StepPanel>
                        </StepPanels>
                    </Stepper>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { SdgAsset } from '~/types/sdgs';

const selectedSdg = ref(-1)

const sdgAssets = ref(Object.fromEntries(
    Object.entries(SdgAsset.sdgs).map(([key, { iconPath, gifPath, ...rest }]) => [
        key,
        {
            ...rest,
            asset_path: iconPath
        }
    ])
))

const bulletPoints = ref([ref('')])

const topicInput = ref('')

const topicFilledOut = computed(() => {
    return topicInput.value.length > 0 && bulletPoints.value.every(ref => ref.value.length > 0)
})

const topicTooltip = computed(() => {
    if(topicInput.value.length === 0) {
        return 'Das Thema muss ausgefüllt werden'
    }
    if(bulletPoints.value.some(ref => ref.value.length === 0)) {
        return 'Alle Punkte müssen ausgefüllt werden'
    }
    return null
})


function getSdgAsset(id: number) {
    return Object.values(sdgAssets.value).find(sdg => sdg.id === id)
}

function selectSdg(newSdg: number) {
    console.log(selectedSdg.value)
    if (selectedSdg.value != -1) {
        let prev = getSdgAsset(selectedSdg.value)
        console.log(prev)
        prev!.asset_path = Object.values(SdgAsset.sdgs).find(sdg => sdg.id === selectedSdg.value)!.iconPath
    }
    if (selectedSdg.value === newSdg) {
        selectedSdg.value = -1
        return
    } else {
        let next = getSdgAsset(newSdg)
        next!.asset_path = Object.values(SdgAsset.sdgs).find(sdg => sdg.id === newSdg)!.gifPath
        selectedSdg.value = newSdg
    }
}

function focusTextarea() {
    const textarea = document.getElementById('description') as HTMLTextAreaElement
    textarea.focus()
}

function addBulletPoint() {
    bulletPoints.value.push(ref(''))
    console.log(bulletPoints)
}

function removeBulletPoint(index: number) {
    bulletPoints.value.splice(index, 1)
}


</script>
