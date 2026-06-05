<script setup lang="ts">
import type { Category, CreateCategoryPayload } from "~/types";

useSeoMeta({ title: "Categorias - Inventário Comercial" });

const {
  categories,
  loading,
  error,
  fetchCategories,
  createCategory,
  updateCategory,
  deleteCategory,
  clearError,
} = useCategories();

const search = ref("");
const showModal = ref(false);
const showConfirm = ref(false);
const categoryEditing = ref<Category | null>(null);
const categoryToDelete = ref<Category | null>(null);

const showToast = ref(false);
const toastMessage = ref("");
const toastType = ref<"success" | "error">("success");

const showFeedback = (msg: string, type: "success" | "error") => {
  toastMessage.value = msg;
  toastType.value = type;
  showToast.value = true;
};

const headers = [
  { title: "ID", key: "id", width: "80px" },
  { title: "Nome", key: "name" },
  { title: "Descrição", key: "description" },
  { title: "Criado em", key: "createdAt" },
  {
    title: "Ações",
    key: "actions",
    sortable: false,
    width: "120px",
    align: "center" as const,
  },
];

onMounted(fetchCategories);

const openCreationModal = () => {
  categoryEditing.value = null;
  showModal.value = true;
};

const openEditionModal = (cat: Category) => {
  categoryEditing.value = cat;
  showModal.value = true;
};

const saveCategory = async (payload: CreateCategoryPayload): Promise<void> => {
  let success: boolean;

  if (categoryEditing.value) {
    success = await updateCategory(categoryEditing.value.id, payload);
  } else {
    const created = await createCategory(payload);
    success = created !== null;
  }

  if (success) {
    showModal.value = false;
    showFeedback("Categoria salva com sucesso!", "success");
  } else if (error.value) {
    showFeedback(error.value, "error");
    clearError();
  }
};

const openDeleteConfirm = (cat: Category) => {
  categoryToDelete.value = cat;
  showConfirm.value = true;
};

const deleteConfirm = async () => {
  if (!categoryToDelete.value) return;

  const ok = await deleteCategory(categoryToDelete.value.id);
  showConfirm.value = false;

  if (ok) {
    showFeedback("Categoria excluída com sucesso!", "success");
  } else if (error.value) {
    showFeedback(error.value, "error");
    clearError();
  }
};

const formatDate = (iso: string) =>
  new Date(iso).toLocaleDateString("pt-BR", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
  });
</script>

<template>
  <div>
    <h1 class="tex-h4 font-weight-bold mb-6">Categorias</h1>
    <AppToast v-model="showToast" :message="toastMessage" :type="toastType" />

    <ConfirmDialog
      v-model="showConfirm"
      :message="`Deseja excluir a categoria '${categoryToDelete?.name}'?`"
      :loading="loading"
      @confirm="deleteConfirm"
    />

    <CategoryModal
      v-model="showModal"
      :category="categoryEditing"
      :loading="loading"
      @save="saveCategory"
    />

    <v-row class="mb-4" align="center">
      <v-col cols="12" sm="8" md="9">
        <v-text-field
          v-model="search"
          prepend-inner-icon="mdi-magnify"
          label="Buscar categoria..."
          variant="outlined"
          density="compact"
          hide-details
          clearable
        />
      </v-col>
      <v-col cols="12" sm="4" md="3" class="text-sm-right">
        <v-btn
          color="primary"
          prepend-icon="mdi-plus"
          @click="openCreationModal"
          block
        >
          Nova Categoria
        </v-btn>
      </v-col>
    </v-row>

    <v-card rounded="lg">
      <v-data-table
        :headers="headers"
        :items="categories"
        :search="search"
        :loading="loading"
        item-value="id"
        hover
        :mobile-breakpoint="0"
        class="text-no=wrap"
      >
        <template #item.name="{ item }">
          <NuxtLink
            :to="`/categories/${item.id}`"
            class="text-primary font-weight-bold text-decoration-none"
          >
            {{ item.name }}
          </NuxtLink>
        </template>
        <template #item.createdAt="{ item }">
          {{ formatDate(item.createdAt) }}
        </template>
        <template #item.actions="{ item }">
          <div class="d-flex gap-2 justify-center">
            <v-btn
              icon="mdi-pencil"
              variant="text"
              color="primary"
              size="small"
              @click="openEditionModal(item)"
            />
            <v-btn
              icon="mdi-delete"
              variant="text"
              color="error"
              size="small"
              @click="openDeleteConfirm(item)"
            />
          </div>
        </template>
      </v-data-table>
    </v-card>
  </div>
</template>
