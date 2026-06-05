<script setup lang="ts">
import type { Category, CreateProductPayload, Product } from "~/types";

interface Props {
  modelValue: boolean;
  product?: Product | null;
  categories: Category[];
  loadingCategories?: boolean;
  loading?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  product: null,
  loadingCategories: false,
  loading: false,
});

const emit = defineEmits<{
  "update:modelValue": [boolean];
  save: [CreateProductPayload];
}>();

const visible = computed({
  get: () => props.modelValue,
  set: (v) => emit("update:modelValue", v),
});

const isEditing = computed(() => props.product !== null);

const form = reactive({
  name: "",
  description: "",
  price: 0,
  categoryId: null as number | null,
});

watch(
  () => props.modelValue,
  (open) => {
    if (open) {
      form.name = props.product?.name ?? "";
      form.description = props.product?.description ?? "";
      form.price = props.product?.price ?? 0;
      form.categoryId = props.product?.categoryId ?? null;
    }
  },
);

const nameRules = [
  (v: string) => !!v || "Nome é Obrigatorio.",
  (v: string) => v.length >= 5 || "Nome deve ter no mínimo 5 caracteres.",
];

const priceRules = [(v: number) => v > 0 || "Preço deve ser maior que zero"];

const categoryRules = [(v: number | null) => !!v || "Seleccione uma categoria"];

const isFormValid = computed(
  () =>
    form.name.trim().length >= 5 && form.price > 0 && form.categoryId !== null,
);

const submit = () => {
  if (!isFormValid.value) return;
  emit("save", {
    name: form.name.trim(),
    description: form.description || undefined,
    price: form.price,
    categoryId: form.categoryId!,
  });
};

const close = () => {
  visible.value = false;
};
</script>

<template>
  <v-dialog v-model="visible" maxWidth="600" persistent>
    <v-card rounded="lg">
      <v-card-title class="pt-4 text-wrap">
        {{ isEditing ? "Editar Produto" : "Novo Produto" }}
      </v-card-title>
      <v-card-text>
        <v-form>
          <v-row>
            <v-col cols="12" sm="8">
              <v-text-field
                v-model="form.name"
                label="Nome *"
                :rules="nameRules"
                variant="outlined"
                autofocus
                hide-details="auto"
              />
            </v-col>
            <v-col cols="12" sm="4">
              <v-text-field
                v-model.number="form.price"
                label="preço *"
                type="number"
                prefix="R$"
                :rules="priceRules"
                variant="outlined"
                hide-details="auto"
              />
            </v-col>
            <v-col cols="12">
              <v-select
                v-model="form.categoryId"
                :items="categories"
                item-title="name"
                item-value="id"
                :rules="categoryRules"
                variant="outlined"
                :loading="loadingCategories"
                hide-details="auto"
              />
            </v-col>
            <v-col cols="12">
              <v-textarea
                v-model="form.description"
                label="Descrição"
                variant="outlined"
                rows="3"
                hide-details="auto"
              />
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>

      <v-card-actions class="pb-4 px-4 flex-wrap">
        <v-spacer class="d-none d-sm-block" />
        <v-btn variant="text" class="mb-2 mb-sm-0" @click="close">
          Cancelar
        </v-btn>
        <v-btn
          color="primary"
          variant="elevated"
          :disbled="!isFormValid"
          :loading="loading"
          @click="submit"
          >Salvar</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
