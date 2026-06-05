<script setup lang="ts">
import type { Category, CreateCategoryPayload } from "~/types";

interface Props {
  modelValue: boolean;
  category?: Category | null;
  loading?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  category: null,
  loading: false,
});

const emit = defineEmits<{
  "update:modelValue": [boolean];
  save: [CreateCategoryPayload];
}>();

const visible = computed({
  get: () => props.modelValue,
  set: (v) => emit("update:modelValue", v),
});

const isEditing = computed(() => props.category !== null);

const form = reactive({
  name: "",
  description: "",
});

watch(
  () => props.modelValue,
  (open) => {
    if (open) {
      form.name = props.category?.name ?? "";
      form.description = props.category?.description ?? "";
    }
  },
);

const nameRules = [
  (v: string) => !!v || "Nome é obrigatório.",
  (v: string) => v.length >= 5 || "Nome deve ter no mínimo 5 caracteres.",
];

const descriptionRules = [
  (v: string) =>
    !v || v.length <= 500 || "Descrição deve ter no máximo 500 caracteres.",
];

const isFormValid = computed(() => form.name.trim().length >= 5);

const submit = () => {
  if (!isFormValid.value) return;
  emit("save", {
    name: form.name.trim(),
    description: form.description || undefined,
  });
};

const close = () => {
  visible.value = false;
};
</script>

<template>
  <v-dialog v-model="visible" max-width="560" persistent>
    <v-card rounded="lg">
      <v-card-title class="pt-4 text-wrap">
        {{ isEditing ? "Editar Categoria" : "Nova Categoria" }}
      </v-card-title>

      <v-card-text>
        <v-form ref="formRef" @submit.prevent="submit">
          <v-row>
            <v-col cols="12">
              <v-text-field
                v-model="form.name"
                label="Nome *"
                :rules="nameRules"
                variant="outlined"
                autofocus
                hide-details="auto"
              />
            </v-col>
            <v-col cols="12">
              <v-textarea
                v-model="form.description"
                label="Descrição"
                variant="outlined"
                rows="3"
                counter="500"
                :rules="descriptionRules"
                hide-details="auto"
              />
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>

      <v-card-actions class="pb-4 px-4 flex-wrap">
        <v-spacer class="d-none d-sm-block" />
        <v-btn variant="text" class="mb-2 mb-sm-0" @click="close"
          >Cancelar</v-btn
        >
        <v-btn
          color="primary"
          variant="elevated"
          :disabled="!isFormValid"
          :loading="loading"
          @click="submit"
        >
          Salvar
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
