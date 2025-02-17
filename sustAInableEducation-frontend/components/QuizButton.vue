<template>
  <div class="text-white py-2 px-3 flex items-center min-h-9 justify-center rounded-md cursor-pointer" :class="getConditionalClasses()" @click="model = !model">
    <span class="text-lg">{{ props.label }} - {{ model }}</span>
  </div>
</template>

<script lang="ts" setup>
  const model = defineModel<boolean>({ required: true })
  const props = defineProps<{label: string, disabled: boolean }>()

  function getConditionalClasses() {
    if(props.disabled) {
      return 'bg-primary-700 opacity-60 cursor-default'
    } else {
      if(model.value) {
        return 'bg-primary-500'
      } else {
        return 'bg-primary-700'
      }
    }
  }
</script>