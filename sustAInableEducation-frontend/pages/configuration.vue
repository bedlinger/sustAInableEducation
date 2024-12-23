<template>
    <div class="w-full h-screen flex items-center justify-center">
        <div class="background animate-anim" />
        <div class="w-screen flex flex-col items-center justify-center h-full">
            <div
                class="bg-slate-50 shadow-xl rounded-xl flex flex-col p-8 items-center w-full max-w-[1000px] h-full max-h-[650px] ">
                <div class="max-h-full flex flex-col items-center w-full">
                    <h1 class="text-3xl font-bold mb-8 h-5">EcoSpace erstellen</h1>
                    <Stepper value="1" class="flex-1">
                        <Steplist class="">
                            <Divider />
                            <Step value="1">Thema</Step>
                            <Divider />
                            <Step value="2">Einstellungen</Step>
                            <Divider />
                        </Steplist>
                        <StepPanels class="">
                            <StepPanel v-slot="{ activateCallback }" value="1" class="w-full !bg-slate-50">
                                <Panel class="max-h-[400px] overflow-y-scroll">
                                    <div class="w-full flex flex-wrap justify-center">
                                        <img v-for="sdg in sdgAssets" :src="sdg.asset_path" :alt="sdg.name"
                                            class="w-40 h-40 m-2 cursor-pointer" :class="{ 'box-border border-4 border-white' : (sdg.id === selectedSdg) }" @click="selectSdg(sdg.id)" />
                                    </div>
                                </Panel>
                                <Divider />
                                <div class="bg-slate-50 flex justify-end">
                                    <Button label="Next" v-tooltip.bottom="{value: (selectedSdg === -1) ? 'Es muss eine Auswahl getroffen werden': null}" :disabled="selectedSdg === -1" @click="activateCallback('2')"/>
                                </div>
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




                    <!-- 
                    <div class="flex flex-col justify-between h-full bg-blue-500">
                        <Tabs value="0" class="w-full max-h-full">
                            <TabList>
                                <Tab value="0" class="w-1/2">SDGs</Tab>
                                <Tab value="1" class="w-1/2">Eigenes Thema</Tab>
                            </TabList>
                            <TabPanels class="max-h-[400px] w-full">
                                <TabPanel value="0"
                                    class="flex flex-wrap justify-center overflow-x-scroll max-h-full">
                                    <img v-for="sdg in SdgAsset.sdgs" :src="sdg.iconPath" :alt="sdg.name"
                                        class="w-48 h-48 m-2" />
                                </TabPanel>
                                <TabPanel value="1"
                                    class="flex flex-wrap justify-center h-full w-full">
                                    <p>Eigenes Thema</p>
                                </TabPanel>
                            </TabPanels>
                        </Tabs>
                        <div class="flex justify-end w-full mt-2 ">
                            <Button label="Weiter" />
                        </div>
                    </div>
                    -->
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

function selectSdg(newSdg: number) {
    console.log(selectedSdg.value)
    if (selectedSdg.value != -1) {
        let prev = Object.values(sdgAssets.value).find(sdg => sdg.id === selectedSdg.value)
        console.log(prev)
        prev!.asset_path = Object.values(SdgAsset.sdgs).find(sdg => sdg.id === selectedSdg.value)!.iconPath
    }
    if (selectedSdg.value === newSdg) {
        selectedSdg.value = -1
        return
    } else {
        let next = Object.values(sdgAssets.value).find(sdg => sdg.id === newSdg)
        next!.asset_path = Object.values(SdgAsset.sdgs).find(sdg => sdg.id === newSdg)!.gifPath
        selectedSdg.value = newSdg
    }
}


</script>
