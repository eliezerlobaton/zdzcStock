<script setup lang="ts">
interface Props {
  message: string;
  type?: "success" | "error" | "warning" | "info";
  modelValue: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  type: "info",
});
const emit = defineEmits<{ "update:modelValue": [boolean] }>();

const visible = computed({
  get: () => props.modelValue,
  set: (v) => emit("update:modelValue", v),
});

const color = computed(
  () =>
    ({
      success: "success",
      error: "error",
      warning: "warning",
      info: "info",
    })[props.type],
);

const icon = computed(
  () =>
    ({
      success: "mdi-check-circle",
      error: "mdi-alert-circle",
      warning: "mdi-alert",
      info: "mdi-information",
    })[props.type],
);
</script>

<template>
  <v-snackbar
    v-model="visible"
    :color="color"
    :timeout="4000"
    location="top right"
    rounded="lg"
  >
    <v-icon :icon="icon" class="mr-2" />
    {{ message }}

    <template #actions>
      <v-btn variant="text" @click="visible = false">Fechar</v-btn>
    </template>
  </v-snackbar>
</template>
