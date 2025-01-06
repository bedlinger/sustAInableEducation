<template>
    <div class="w-full h-full">
        <div class="background animate-anim" />
        <div class="w-screen flex justify-center items-center h-full bg-slate-50 pt-20 p-4">
            <div class="panel w-full h-full rounded-xl relative border-solid border-slate-3s00 border-2 flex flex-col justify-center">
                <div class="content h-full w-full">
                    
                </div>
                <div class="controls flex flex-col m-4">
                    <Divider/>
                    <div class="timer">
                        <Timer class="sm:hidden" v-model="timerValue"/>
                    </div>
                    <div class="flex flex-col sm:flex-row sm:justify-between w-full">
                        <Button class="mb-2 sm:mb-0 sm:mr-5 flex-1 sm:!text-2xl" label="Option 1" @click="setTimer"/>
                        <Button class="mb-2 sm:mb-0 sm:mx-5 flex-1 sm:!text-2xl" label="Option 2" @click="startTimer"/>
                        <Knob class="hidden sm:block mx-5" v-model="timerValue.percent" :valueTemplate="(number) => { return `${timerValue.time}`}" disabled :size="100">
                        </Knob>
                        <Button class="mb-2 sm:mb-0 sm:mx-5 flex-1 sm:!text-2xl" label="Option 3" @click=""/>
                        <Button class="sm:ml-5 flex-1 sm:!text-2xl" label="Option 4"/>
                    </div>
                    
                </div>
            </div>
                
        </div>
    </div>
</template>

<script setup lang="ts">

const timerValue = ref({
    initialValue: 10,
    time: 10,
    percent: 0,
});

var timerCount = 0;

var timerInterval : NodeJS.Timeout;

function setTimer() {
    timerValue.value.time = timerValue.value.initialValue;
    timerValue.value.percent = 0;
}

function startTimer() {
    if(timerCount > 0) {
        return;
    }
    let increment = 100 / timerValue.value.time;
    timerCount++;
    timerInterval = setInterval(() => {
        if(timerValue.value.time <= 0) {
            clearInterval(timerInterval);
            timerCount--;
            return;
        }
        timerValue.value.time--;
        timerValue.value.percent += increment;
    }, 1000);
}


</script>