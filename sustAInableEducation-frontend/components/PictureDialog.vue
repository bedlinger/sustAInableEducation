<template>
  <Dialog v-model:visible="model" header="Profilbild generieren" modal :draggable="false" dismissable-mask class="max-w-md w-full mx-4">
    <div class="w-full flex flex-col justify-center items-center gap-4">
      <div>
        <Image class="!rounded-2xl !overflow-hidden" :src="selectedStyle?.imagePath" width="256" alt="Profilbild" preview>
          <template #previewicon>
            <div />
          </template>
        </Image>
        <p class="text-md ml-2" v-if="selectedStyle">Beispielbild</p>
      </div>
      <div class="w-full">
        <Select v-model="selectedStyle" :options="styles" optionLabel="name" placeholder="Style auswählen" class="w-full" fluid />
      </div>
      <Divider/>
      <div class="flex justify-between w-full">
        <Button label="Abbrechen" @click="model = !model" outlined />
        <Button label="Speichern" @click="generatePicture" :loading="loading" :disabled="loading || !selectedStyle"/>
      </div>
    </div>
  </Dialog>
</template>

<script lang="ts" setup>
const model = defineModel('visible', { required: true, type: Boolean });
const emit = defineEmits(['success', 'fail']);

watch(() => model.value, (value) => {
  if(value) {
    selectedStyle.value = null;
    loading.value = false;
  }
});

const runtimeConfig = useRuntimeConfig();

const loading = ref(false);

const selectedStyle = ref();
const styles = ref([
    { name: 'Cartoon', id: 0, imagePath: '/img/profilepictures/cartoon.png' },
    { name: 'Pop-Art', id: 1, imagePath: '/img/profilepictures/popart.png' },
    { name: 'PixelArt', id: 2, imagePath: '/img/profilepictures/pixelart.png' },
    { name: 'FantasyArt', id: 3, imagePath: '/img/profilepictures/fantasy.png' },
    { name: 'Stencil', id: 4, imagePath: '/img/profilepictures/stencil.png' },
    { name: 'PaperCraft', id: 5, imagePath: '/img/profilepictures/papercraft.png' },
    { name: 'Risographie', id: 6, imagePath: '/img/profilepictures/risograph.png' },
    { name: 'Cyberpunk ', id: 7, imagePath: '/img/profilepictures/cyberpunk.png' },
    { name: 'PencilSketch', id: 8, imagePath: '/img/profilepictures/pencilsketch.png' },
    { name: 'PaperCollage', id: 9, imagePath: '/img/profilepictures/papercollage.png' },
    { name: 'Psychedelic', id: 10, imagePath: '/img/profilepictures/psychedelic.png' },
    { name: 'StreetArt', id: 11, imagePath: '/img/profilepictures/streetart.png' },
    { name: 'Ukiyo-e', id: 12, imagePath: '/img/profilepictures/ukiyoe.png' },
    { name: 'Manga', id: 13, imagePath: '/img/profilepictures/manga.png' },
    { name: 'Medieval', id: 14, imagePath: '/img/profilepictures/medieval.png' }
    
]);

async function generatePicture() {
  loading.value = true;

  await $fetch(`${runtimeConfig.public.apiUrl}/account/profilepicture`, {
    method: 'POST',
    credentials: 'include',
    body: {
      style: selectedStyle.value.id
    },
    onResponse: (response) => {
      if (response.response.ok) {
        emit('success');
        model.value = false;
      } else {
        emit('fail');
      }
    }
  });

  loading.value = false;
}
</script>