<template>
    <div class="h-8 rounded-lg w-full flex items-center opacity-70 mb-2 border-solid border-2 border-primary-500 bg-primary-300 justify-center ">
        <div class="w-full h-full">
            <div :style="getWidth" class="bg-primary-700 rounded-md h-full"/>
        </div>
        <p class="absolute font-bold text-white">{{ model?.time }}</p>
    </div>
</template>

<script setup lang="ts">
import type { TimerData } from '~/types/timer';

const model = defineModel<TimerData>();

const getWidth = computed(() => {
    if(model.value!.percent >= 100) {
        return `width: 100%`;
    }
    return `width: ${model.value?.percent}%`;
});
</script>