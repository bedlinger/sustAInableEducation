<template>
  <div class="sidebar-container w-full absolute h-full pt-16 bg-white">
    <div class="sidebar w-full h-full flex-col overflow-y-scroll flex">
      <div id="sidebar-header">
        <div class="w-full flex items-center justify-end py-2">
          <div class="bg-slate-400 flex items-center justify-center p-2 pr-5 cursor-pointer rounded-tl-xl rounded-bl-xl">
            <Icon name="ic:sharp-keyboard-double-arrow-left" class="size-7" @click="" />
          </div>
        </div>
        <div class="flex items-center mx-2">
          <IconField class="mr-2 flex-1">
            <InputIcon>
              <Icon name="ic:baseline-search" />
            </InputIcon>
            <InputText placeholder="Suchen" v-model="props.searchInput" class="w-full" />
          </IconField>
          <Button class="!aspect-square !p-0 !" @click="emit('toggleFilters')">
            <template #default>
              <div class="flex items-center justify-center size-11">
                <Icon :name="!props.showFilters ? 'ic:baseline-filter-alt' : 'ic:baseline-close'"
                  :class="[props.showFilters ? 'size-6' : 'size-4']" />
              </div>
            </template>
          </Button>
        </div>
        <Panel class="m-2" v-if="props.showFilters">
          <div>
            <h3 class="text-lg font-bold">Filter</h3>
            <Divider />
          </div>
          <div class="flex flex-col mb-4">
            <label class="mb-1" for="sortSelect">Sortierrichtung:</label>
            <div class="flex">
              <Select class="w-32 flex-1 mr-1" id="sortSelect" size="small"
                v-model="props.filters.refs.sort.subject.value" :options="props.filters.options.sort" />
              <ToggleButton class="w-fit aspect-square !p-2" v-model="props.filters.refs.sort.direction.value">
                <template #default>
                  <Icon v-if="props.filters.refs.sort.direction.value" name="ic:baseline-arrow-upward" />
                  <Icon v-else name="ic:baseline-arrow-downward" />
                </template>
              </ToggleButton>
            </div>
          </div>
          <div class="flex flex-col items-start mb-4">
            <label class="text-md mb-1" for="finishedSelect">Abgeschlossen:</label>
            <Select class="w-full" id="finishedSelect" size="small" v-model="props.filters.refs.finished.value"
              :options="props.filters.options.finished" />
          </div>
          <div class="flex flex-col items-start">
            <label class="mb-1" for="datePicker">Erstellungsdatum:</label>
            <DatePicker class="w-full" id="datePicker" v-model="props.filters.refs.date.value" size="small"
              dateFormat="dd.mm.yy" showButtonBar selectionMode="range" :manualInput="false" />
          </div>
          <Message v-if="!props.isFilterApplied" class="mt-4">
            Die ausgewählten Filter wurden noch nicht angewandt.
          </Message>
          <Divider />
          <div class="flex justify-between">
            <Button label="Anwenden" size="small" @click="emit('applyFilters')" />
            <Button label="Zurücksetzen" size="small" variant="text" @click="emit('resetFilters')" />

          </div>
        </Panel>
        <Divider class="!w-full !mx-2" />
      </div>
      <div id="sidebar-content" class="flex flex-col">
        <EcoSpaceListEntry v-for="ecoSpace in props.searchedSpaces" :ecoSpace="ecoSpace" @delete="emit('openDeleteDialog')"
          v-model="spaceRefsById[ecoSpace.id].value" @click="emit('selectSpace', ecoSpace.id)" class="mx-2"/>
        <NuxtLink to="/configuration" class=" mx-2">
          <Button label="EcoSpace erstellen" rounded size="small" class="w-full">
            <template #icon>
              <Icon name="ic:baseline-add" class="size-5" />
            </template>
          </Button>
        </NuxtLink>
        <div v-if="searchedSpaces.length === 0" class="mt-2">
          <Message class="text-md flex justify-center items-center w-full">
            Es gibt noch keine EcoSpaces
          </Message>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { EcoSpace } from '~/types/EcoSpace';
import type { OverviewFilter } from '~/types/filter';

const props = defineProps<{
  searchedSpaces: EcoSpace[],
  searchInput: string,
  showFilters: boolean,
  filters: OverviewFilter,
  isFilterApplied: boolean,
  spaces: EcoSpace[] | null,
  selectedSpace: EcoSpace | undefined
}>();

const emit = defineEmits(['toggleFilters', 'applyFilters', 'resetFilters', 'selectSpace', 'openDeleteDialog']);

const spaceRefsById = props.spaces ? props.spaces.reduce((acc, space) => {
  acc[space.id] = ref(false);
  return acc;
}, {} as Record<string, Ref<boolean>>) : {};

</script>