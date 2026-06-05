<script setup lang="ts">
import type { Category, Product } from "~/types";

const route = useRoute();
const router = useRouter();
const categoryId = Number(route.params.id);

const category = ref<Category | null>(null);
const products = ref<Product[]>([]);
const loading = ref(true);
const loadinngProducts = ref(true);

const headers = [
  { title: "ID", key: "id", width: "80" },
  { title: "Nome do produto", key: "name" },
  { title: "Preço", key: "price", width: "150px" },
];

onMounted(async () => {
  loading.value = true;
  const config = useRuntimeConfig();
  const base = config.public.apiBase;

  try {
    category.value = await $fetch<Category>(
      `${base}/api/categories/${categoryId}`,
    );

    loadinngProducts.value = true;
    products.value = await $fetch<Product[]>(
      `${base}/api/products?categoryId=${categoryId}`,
    );
  } catch (err) {
    console.error("Erro ao carregar detalhes", err);
  } finally {
    loading.value = false;
    loadinngProducts.value = false;
  }
});

const formatCurrency = (v: number) =>
  v.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
</script>

<template>
  <div>
    <v-btn
      prepend-icon="mdi-arrow-left"
      variant="text"
      class="mb-4"
      @click="router.push('/categories')"
      >Voltar</v-btn
    >
    <div v-if="loading" class="text-center py-12">
      <v-progress-circular indeterminate color="primary" size="64" />
    </div>

    <div v-else-if="category">
      <v-card rounded="lg" variant="outlined" class="border mb-6">
        <v-card-text
          class="d-flex justify-space-between align-center flex-wrap gap-4"
        >
          <div>
            <div
              class="text-caption text-medium-emphasis uppercase font-weight-bold"
            >
              Detalhes da Categoria
            </div>
            <h1 class="text-h4 font-weight-bold mt-1 text-primary">
              {{ category.name }}
            </h1>
            <p class="text-body-1 mt-2 text-medium-emphasis">
              {{ category.description || "Sem descrição cadastrada" }}
            </p>
          </div>

          <v-card
            rounded="lg"
            color="primary"
            variant="tonal"
            class="pa-4 text-container"
            min-width="160"
          >
            <div class="text-h3 font-weight-bold">{{ products.length }}</div>
            <div class="text-caption">Produtos Vinculados</div>
          </v-card>
        </v-card-text>
      </v-card>

      <h2 class="text-h5 font-weight-bold mb-4">Produtos nesta Categoria</h2>

      <v-card rounded="lg">
        <v-data-table
          :headers="headers"
          :items="products"
          :loading="loadinngProducts"
          item-value="id"
          class="text-no-wrap"
        >
          <template #item.price="{ item }">{{
            formatCurrency(item.price)
          }}</template>
        </v-data-table>
      </v-card>
    </div>
  </div>
</template>
