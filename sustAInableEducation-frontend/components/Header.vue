<template>
    <div class="h-16 bg-primary-700 w-screen flex justify-between items-center px-2 py-4 sticky top-0">
        <Toast />
        <NuxtLink to="/" class="inline-block">
            <img src="/public/sustainableeducation_logo_white.svg" alt="sustAInableEducation Logo" class="h-16" />
        </NuxtLink>

        <div class="flex justify-center items-center">
            <NuxtLink to="/spaces" class="text-white mx-4 text-xl hidden sm:block">EcoSpaces</NuxtLink>
            <NuxtLink to="/quizzes" class="text-white mx-4 text-xl hidden sm:block">Quizzes</NuxtLink>
            <div class="text-white mx-4 text-xl sm:flex justify-center items-center hidden"
                :class="!(['/login', '/register'].includes(route.path)) ? 'cursor-pointer' : ''" @click="toggleMenu">
                <Image ref="img" :src="profileImage" class="bg-white mx-4 size-11 rounded-full overflow-hidden" />
            </div>
            <p @click="showNavDrawer = !showNavDrawer" class="sm:hidden">ALARM</p>
            <Drawer v-model:visible="showNavDrawer" header="Navigation" position="right">
                <template #container="{ closeCallback }">
                    <div class="flex flex-col h-full">
                        <div class="flex items-center justify-between px-4 pt-4">
                            <img src="/public/sustainableeducation_logo_green.svg" alt="sustAInableEducation Logo"
                                class="h-16" />
                            <Button type="button" @click="closeCallback" rounded outlined class="!p-3">
                                <template #default>
                                    <div class="flex items-center justify-center">
                                        <Icon name="ic:baseline-close" />
                                    </div>
                                </template>
                            </Button>
                        </div>
                        <div class="flex-1 p-4 flex gap-2 flex-col">
                            <a class="text-3xl text-primary-700" @click="navigate('/spaces')">EcoSpaces</a>
                            <a class="text-3xl text-primary-700" @click="navigate('/quizzes')">Quizzes</a>
                        </div>
                        <div>
                            <Divider class="!mx-4"/>
                            <a @click="navigate('/account')" class="flex justify-center gap-4 items-center p-4">
                                <Avatar :image="profileImage" shape="circle" />
                                <span class="font-bold">{{ username }}</span>
                            </a>
                        </div>
                    </div>
                </template>
            </Drawer>
            <Menu v-if="true" ref="menu" :model="items" :popup="true" @show="isMenuOpen = true"
                @hide="isMenuOpen = false" />
        </div>
    </div>
</template>

<script setup lang="ts">
import type { Account } from '~/types/Account';

const showNavDrawer = ref(false);


const runtimeConfig = useRuntimeConfig();
const route = useRoute();
const router = useRouter();

const username = ref('USERNAME');
const picturePath = ref<string | null>(null);

const profileImage = computed(() => {
    return picturePath.value ? `${runtimeConfig.public.apiUrl}${picturePath.value}` : '/img/profilepicture_placeholder.jpg';
});

const menu = ref();

// Reaktives items-Objekt für das Menü
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

const { data, error, refresh } = useFetch<Account | null>(`${runtimeConfig.public.apiUrl}/account`, {
    headers: useRequestHeaders(['cookie']),
    method: 'GET',
    credentials: 'include',
});

if (data.value) {
    picturePath.value = data.value.profileImage;
    username.value = data.value.anonUserName;
    updateMenu();
}

const isMenuOpen = ref(false);

watch(() => route.path, async () => {
    if (!(['/login', '/register'].includes(route.path))) {
        refresh();
        if (data.value) {
            picturePath.value = data.value.profileImage;
            username.value = data.value.anonUserName;
            updateMenu();
        }
        if (error.value && error.value.statusCode === 401) {
            navigateTo(`/login?redirect=${route.fullPath}`);
        }
    } else {
        picturePath.value = null;
    }
});
function updateMenu() {
    items.value[0].items[0].label = username.value;
}

async function logout() {
    await $fetch(`${runtimeConfig.public.apiUrl}/account/logout`, {
        method: 'POST',
        credentials: 'include',
        onResponse: (response) => {
            if (response.response.ok) {
                navigateTo('/login?redirect=' + route.fullPath);
            }
        }
    });
}

const toggleMenu = (event: Event) => {
    if (!(['/login', '/register'].includes(route.path))) {
        menu.value.toggle(event);
    }
};

function navigate(path: string) {
    showNavDrawer.value = false;
    navigateTo(path);
}
</script>