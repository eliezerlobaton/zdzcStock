<script setup lang="ts">
import type { Product } from "~/types";

const route = useRoute();
const router = useRouter();
const productId = Number(route.params.id);

const product = ref<Product | null>(null);
const loading = ref(true);

onMounted(async () => {
  const config = useRuntimeConfig();
  const base = config.public.apiBase;

  try {
    product.value = await $fetch<Product>(`${base}/api/products/${productId}`);
  } catch (err) {
    console.log("Erro ao carrgear produto", err);
  } finally {
    loading.value = false;
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
      @click="router.push('/products')"
    >
      Voltar
    </v-btn>

    <div v-if="loading" class="text-center py-12">
      <v-progress-circular
        indeterminate
        color="primary"
        variant="outlined"
        size="64"
      />
    </div>

    <v-card
      v-else-if="product"
      rounded="lg"
      variant="outlined"
      class="border max-width-600 mx-auto"
    >
      <v-card-title class="pt-6 px-6 pb-2">
        <span
          class="text-caption text-medium-emphasis uppercase font-weight-bold"
          >Ficha do Produto</span
        >
        <h1 class="text-h4 font-weight-bold mt-1 text-primary">
          {{ product.name }}
        </h1>
      </v-card-title>

      <v-card-text class="px-6 pb-6">
        <v-list lines="two" class="bg-transparent pa-0">
          <v-list-item
            title="ID do Produto"
            :subtitle="String(product.id)"
            class="px-0"
          />

          <v-list-item
            title="Preço"
            :subtitle="formatCurrency(product.price)"
            class="px-0"
          >
            <template #prepend>
              <v-icon icon="mdi-currency-brl" class="mr-4 text-success" />
            </template>
          </v-list-item>

          <v-list-item
            title="Categoria Vinculada"
            :subtitle="product.category?.name || 'Sem categoria'"
            class="px-0"
          >
            <template #prepend>
              <v-icon icon="mdi-tag-multiple" class="mr-4 text-primary" />
            </template>
          </v-list-item>

          <v-list-item
            title="Descrição do Produto"
            :subtitle="product.description || 'Sem descrição cadastrada.'"
            class="px-0"
          >
            <template #prepend>
              <v-icon icon="mdi-information-outline" class="mr-4" />
            </template>
          </v-list-item>
        </v-list>
      </v-card-text>
    </v-card>
  </div>
</template>
