<template>
    <div class="h-16 bg-primary-700 w-screen flex justify-between items-center px-2 py-4 sticky top-0">
        <Toast />
        <NuxtLink to="/" class="inline-block">
            <img src="/public/sustainableeducation_logo_white.svg" alt="sustAInableEducation Logo" class="h-16" />
        </NuxtLink>

        <div class="flex justify-center items-center">
            <NuxtLink to="/spaces" class="text-white mx-4 text-xl">EcoSpaces</NuxtLink>
            <NuxtLink to="/quizzes" class="text-white mx-4 text-xl">Quizzes</NuxtLink>
            <div class="text-white mx-4 text-xl flex justify-center items-center"
                :class="!(['/login', '/register'].includes(route.path)) ? 'cursor-pointer' : ''" @click="toggleMenu">
                <Image v-if="showImage" ref="img" :src="profileImage" class="bg-white mx-4 size-11 rounded-full overflow-hidden" />
            </div>
            <Menu ref="menu" :model="items" :popup="true" @show="isMenuOpen = true" @hide="isMenuOpen = false" />
        </div>
    </div>
</template>

<script setup lang="ts">
import type { Account } from '~/types/Account';

const runtimeConfig = useRuntimeConfig();
const route = useRoute();
const router = useRouter();

const username = ref('USERNAME');

const picturePath = ref<string | null>(null)
const profileImage = computed(() => {
    return picturePath.value ? `${runtimeConfig.public.apiUrl}${picturePath.value}` : '/img/profilepicture_placeholder.jpg'
})

const menu = ref();
const isMenuOpen = ref(false);

const showImage = ref(true);

const { data, error, refresh } = useFetch<Account | null>(`${runtimeConfig.public.apiUrl}/account`, {
    headers: useRequestHeaders(['cookie']),
    method: 'GET',
    credentials: 'include'
});

if (!(['/login', '/register'].includes(route.path)) && error.value && error.value.statusCode === 401) {
        navigateTo(`/login?redirect=${route.fullPath}`)
}

if (data.value) {
    picturePath.value = data.value.profileImage;
    username.value = data.value.anonUserName;
    rerenderImage();
}

const items = ref([
    {
        items: [
            {
                label: username.value,
                command: () => router.push('/account')
            },
            {
                label: 'Abmelden',
                command: () => logout()
            },
        ]
    }
]);

watch(() => route.path, async () => {
    if (!(['/login', '/register'].includes(route.path))) {
        refresh()
        if(data.value) {
            picturePath.value = data.value.profileImage
            username.value = data.value.anonUserName
            rerenderImage()
        }
        if(error.value && error.value.statusCode === 401) {
            navigateTo(`/login?redirect=${route.fullPath}`)
        }
    } else {
        picturePath.value = null
    }
});

async function logout() {
    await $fetch(`${runtimeConfig.public.apiUrl}/account/logout`, {
        method: 'POST',
        credentials: 'include',
        onResponse: (response) => {
            if (response.response.ok) {
                navigateTo('/login?redirect=' + route.fullPath)
            }
        }
    })
}

async function rerenderImage() {
    showImage.value = false
    await nextTick()
    showImage.value = true
}

const toggleMenu = (event: Event) => {
    if (!(['/login', '/register'].includes(route.path))) {
        menu.value.toggle(event);
    }
};
</script>