<template>
    <div class="h-16 bg-primary-700 w-screen flex justify-between items-center px-2 py-4 sticky top-0">
        <NuxtLink to="/" class="inline-block">
            <img src="/public/sustainableeducation_logo_white.svg" alt="sustAInableEducation Logo" class="h-16" />
        </NuxtLink>
        <div class="flex justify-center items-center">
            <NuxtLink to="/ecospaces" class="text-white mx-4 text-xl">EcoSpaces</NuxtLink>
            <NuxtLink to="/quizzes" class="text-white mx-4 text-xl">Quizzes</NuxtLink>
            <Icon name="system-uicons:user-male-circle" class="bg-white mx-4 size-10"/>
        </div>
    </div>
</template>