<template>
<div class="h-screen">
    <Header />
    <div class="absolute inset-x-0 top-0 h-screen bg-gradient-to-br bg-no-repeat bg-cover from-transparent via-20% via-primary-200 to-50% -z-50 blur-3xl;"/>
    <div class="w-screen h-[calc(100vh-64px)] flex justify-center items-center">
        <div class="bg-white shadow-xl flex justify-between flex-col rounded-xl items-center p-8">
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