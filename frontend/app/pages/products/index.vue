<script setup lang="ts">
import ProductModal from "~/components/ProductModal.vue";

import type { CreateProductPayload, Product } from "~/types";

useSeoMeta({ title: "Produtos - Inventário Comensrcial" });

const route = useRoute();

const activeCategoryId = computed(() => {
  const cid = route.query.categoryId;
  return cid ? Number(cid) : null;
});

const {
  products,
  loading: loadingProducts,
  error: errorProducts,
  fetchProducts,
  createProduct,
  updateProduct,
  deleteProduct,
  clearError: clearProductError,
} = useProducts();

const {
  categories,
  loading: loadingCategories,
  fetchCategories,
} = useCategories();

const search = ref("");
const showModal = ref(false);
const showConfirm = ref(false);
const productEditing = ref<Product | null>(null);
const productToDelete = ref<Product | null>(null);

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
  { title: "Preço", key: "price", width: "140px" },
  { title: "Categoria", key: "category.name", width: "180px" },
  {
    title: "Ações",
    key: "actions",
    sortable: false,
    width: "120px",
    align: "center" as const,
  },
];

onMounted(() => {
  const cid = activeCategoryId.value;
  Promise.all([fetchProducts(cid || undefined), fetchCategories()]);
});

watch(activeCategoryId, (newCid) => {
  fetchProducts(newCid || undefined);
});

const openCreationModal = () => {
  productEditing.value = null;
  showModal.value = true;
};

const openEditionModal = (prod: Product) => {
  productEditing.value = prod;
  showModal.value = true;
};

const saveProduct = async (payload: CreateProductPayload): Promise<void> => {
  let success: boolean;

  if (productEditing.value) {
    success = await updateProduct(productEditing.value.id, payload);
  } else {
    const created = await createProduct(payload);
    success = created !== null;
  }

  if (success) {
    showModal.value = false;
    showFeedback("Produto Salvo com sucesso!", "success");
  } else if (errorProducts.value) {
    showFeedback(errorProducts.value, "error");
    clearProductError();
  }
};

const openDeleteConfirm = (prod: Product) => {
  productToDelete.value = prod;
  showConfirm.value = true;
};

const deleteConfirm = async () => {
  if (!productToDelete.value) return;
  const ok = await deleteProduct(productToDelete.value.id);
  showConfirm.value = false;
  if (ok) {
    showFeedback("Produto excluído com sucesso!", "success");
  } else if (errorProducts.value) {
    showFeedback(errorProducts.value, "error");
    clearProductError();
  }
};

const formatCurrency = (v: number) =>
  v.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
</script>

<template>
  <div>
    <h1 class="text-h4 font-weight-bold mb-6">Produtos</h1>
    <AppToast v-model="showToast" :message="toastMessage" :type="toastType" />

    <ConfirmDialog
      v-model="showConfirm"
      :message="`Deseja excluir o produto '${productToDelete}'?`"
      :loading="loadingProducts"
      @confirm="deleteConfirm"
    />

    <ProductModal
      v-model="showModal"
      :product="productEditing"
      :categories="categories"
      :loading-categories="loadingCategories"
      :loading="loadingProducts"
      @save="saveProduct"
    />

    <v-row class="mb-4" align="center">
      <v-col cols="12" sm="8" md="9">
        <v-text-field
          v-model="search"
          prepend-inner-icon="mdi-magnify"
          label="Buscar produto..."
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
          Novo Produto
        </v-btn>
      </v-col>
    </v-row>

    <v-card rounded="lg" style="overflow-x: auto">
      <v-data-table
        :headers="headers"
        :items="products"
        :search="search"
        :loading="loadingProducts"
        item-value="id"
        hover
        :mobileBreakpoint="0"
        class="text-no-wrap"
      >
        <template #item.name="{ item }">
          <NuxtLink
            :to="{ path: `/products/${item.id}` }"
            class="text-primary font-weight-bold text-decoration-none"
          >
            {{ item.name }}
          </NuxtLink>
        </template>

        <template #item.price="{ item }">
          {{ formatCurrency(item.price) }}
        </template>
        <template #item.category.name="{ item }">
          <v-chip color="primary" variant="tonal" size="small">
            {{ item.category?.name ?? "Sem Categoria" }}
          </v-chip>
        </template>

        <template #item.actions="{ item }">
          <div class="d-flex gap-2 justify-center">
            <v-btn
              incon="mdi-pencil"
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
