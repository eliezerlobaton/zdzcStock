<script setup lang="ts">
interface Props {
  modelValue: boolean;
  message?: string;
  loading?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  message: "Esta ação não pode ser desfeita. deseja continuar?",
  loading: false,
});

const emit = defineEmits<{
  "update:modelValue": [boolean];
  confirm: [];
  cancel: [];
}>();

const visible = computed({
  get: () => props.modelValue,
  set: (v) => emit("update:modelValue", v),
});

const confirm = () => emit("confirm");
const cancel = () => {
  emit("cancel");
  visible.value = false;
};
</script>

<template>
  <v-dialog>
    <v-card>
      <v-card-title class="d-flex align-center gap-2 pt-4 text-wrap">
        <v-icon icon="mdi-alert-circle-outline" color="error" size="28" />
        Confirmar Exclusão
      </v-card-title>

      <v-card-text>
        {{ message }}
      </v-card-text>

      <v-card-actions class="pb-4 px-4 flex-wrap">
        <v-spacer class="d-none d-sm-block" />
        <v-btn variant="text" class="mb-2 mb-sm-0" v-click="cancel">
          Cancelar
        </v-btn>
        <v-btn
          color="error"
          variant="elevated"
          :loading="loading"
          @click="confirm"
        >
          Excluir
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
