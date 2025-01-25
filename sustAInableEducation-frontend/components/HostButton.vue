<template>
  <Button class="sm:!text-2xl w-full h-full" :label="props.label" @click="emit('click')"
    :disabled="props.disabled">
    <template #default>
      <div class="w-full h-full relative p-4 sm:p-0" v-if="props.voting">
        <div class="bg h-full w-full flex flex-col absolute top-0 left-0">
          <div class="w-full flex-1"></div>
          <div class="w-full relative h-4 mb-1">
            <div class="w-full h-5 bg-primary-200 rounded-md absolute hidden sm:block"></div>
            <div class="h-5 bg-primary-500 rounded-md absolute hidden sm:block" :style="{width: props.percentage + '%'}"></div>
            <div class="w-full h-5 absolute text-sm justify-center items-center hidden sm:flex">{{ props.percentage }}%</div>
          </div>
        </div>
        <div class="w-full h-full flex justify-between sm:justify-center items-center absolute top-0 left-0">
          <span class="ml-2 sm:mr-0">{{ props.label }}</span>
          <span class="sm:hidden mr-2 sm:mr-0">{{ props.percentage }}% </span>
        </div>
      </div>
    </template>
  </Button>
</template>

<script lang="ts" setup>
  const props = defineProps<{
    disabled: boolean,
    voting: boolean,
    label: string,
    percentage: number
  }>()

  const emit = defineEmits(['click'])
</script>