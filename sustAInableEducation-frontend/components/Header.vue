<template>
    <div class="h-16 bg-primary-700 w-screen flex justify-between items-center px-2 py-4 sticky top-0">
        <Toast />
        <NuxtLink to="/" class="inline-block">
            <img src="/public/sustainableeducation_logo_white.svg" alt="sustAInableEducation Logo" class="h-16" />
        </NuxtLink>

        <div class="flex justify-center items-center">
            <NuxtLink to="/ecospaces" class="text-white mx-4 text-xl">EcoSpaces</NuxtLink>
            <NuxtLink to="/quizzes" class="text-white mx-4 text-xl">Quizzes</NuxtLink>
            <div class="text-white mx-4 text-xl flex justify-center items-center" :class="route.path !== '/login' ? 'cursor-pointer' : ''" @click="toggleMenu">
                <Icon name="ic:baseline-account-circle" class="bg-white mx-4 size-10" />
            </div>
            <Menu ref="menu" :model="items" :popup="true"/>
        </div>
    </div>
</template>

<script setup lang="ts">
const runtimeConfig = useRuntimeConfig();
const route = useRoute();


const menu = ref();

const items = ref([
    {
        label: 'Sie sind eingeloggt',
        items: [
            {
                label: 'Logout',
                command: () => logout()
            }
        ]
    }

]);

async function logout() {
    console.log("Test")
    await $fetch(`${runtimeConfig.public.apiUrl}/account/logout`, {
        method: 'POST',
        credentials: 'include',
        onResponse: (response) => {
            if (response.response.status === 200) {
                // MAybe add toast
            }
        }
    })
}

const toggleMenu = (event: Event) => {
    if (route.path !== '/login') {
        menu.value.toggle(event);
    }
    
};

</script>