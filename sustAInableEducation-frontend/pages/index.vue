<template>
<div class="w-full h-full">
    <div class="background"/>
    <div class="w-screen flex justify-center items-center h-full">
        <div class="bg-slate-50 shadow-xl flex justify-between flex-col rounded-xl items-center p-8">
            <h1 class="text-3xl font-bold mb-4">EcoSpace beitreten</h1>
            <p class="mb-4">
                Bitte gib den 6-stelligen Code ein, um dem Space beizutreten.
            </p>
            <InputOtp :length="6" v-model="value" class="mb-4"/>
            <Button class="">Beitreten</Button>
            <p class="mt-4">oder</p>
            <Button variant="link" label="Space erstellen" text/>
        </div>
    </div>
</div>
</template> 

<script setup lang="ts">
const value = ref('')
</script>