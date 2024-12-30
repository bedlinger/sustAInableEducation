<template>
    <div class="w-full h-full">
            <div class="background animate-anim" />
        <div class="w-screen flex justify-center items-center h-full ">
            <div class="bg-slate-50 shadow-xl flex justify-between flex-col rounded-xl items-center p-8">
                <h1 class="text-3xl font-bold mb-4">EcoSpace beitreten</h1>
                <p class="mb-4">
                    Bitte gib den 6-stelligen Code ein, um dem Space beizutreten.
                </p>
                <form action="" class="flex justify-center flex-col items-center">
                    <InputOtp :length="6" v-model="value" integerOnly class="mb-4" />
                    <Button class="w-3/6" type="submit">Beitreten</Button>
                </form>

                <p class="mt-4">oder</p>
                <NuxtLink to="/configuration">
                    <Button variant="link" label="EcoSpace erstellen"/>
                </NuxtLink>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
const value = ref('')
</script>