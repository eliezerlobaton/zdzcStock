<script setup lang="ts">
useSeoMeta({
  title: "Dashboard - Inventário Comercial",
  description: "Visão geral do catálogo de produtos e categorias",
});

const { categories, fetchCategories } = useCategories();
const { products, fetchProducts } = useProducts();

onMounted(async () => {
  await Promise.all([fetchCategories(), fetchProducts()]);
});

const totalCategories = computed(() => categories.value.length);
const totalProducts = computed(() => products.value.length);
const formatedTotalValue = computed(() => {
  const total = products.value.reduce((acc, p) => acc + p.price, 0);
  return total.toLocaleString("pr-BR", { style: "currency", currency: "BRL" });
});
</script>

<template>
  <div>
    <h1 class="text-h4 font-weight-bold mb-6">Dashboard</h1>

    <v-row>
      <v-col cols="12" sm="4">
        <v-card rounded="lg" variant="outlined" class="border">
          <v-card-text class="d-flex justify-space-between align-start">
            <div>
              <span
                class="text-caption text-medium-emphasis uppercase font- weight-bold"
                >Categorias</span
              >
              <div class="text-h4 font-weight-bold mt-1">
                {{ totalCategories }}
              </div>
            </div>
            <v-avatar color="primary" variant="tonal" rounded="lg">
              <v-icon icon="mdi-tag-multiple" />
            </v-avatar>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" sm="4">
        <v-card rounded="lg" variant="outlined" class="border">
          <v-card-text class="d-flex justify-space-between align-start">
            <div>
              <span
                class="text-caption text-medium-emphasis uppercase font- weight-bold"
                >Produtos</span
              >
              <div class="text-h4 font-weight-bold mt-1">
                {{ totalProducts }}
              </div>
            </div>
            <v-avatar color="secondary" variant="tonal" rounded="lg">
              <v-icon icon="mdi-package-variant" />
            </v-avatar>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" sm="4">
        <v-card rounded="lg" variant="outlined" class="border">
          <v-card-text class="d-flex justify-space-between align-start">
            <div>
              <span
                class="text-caption text-medium-emphasis uppercase font- weight-bold"
                >Valor em Estoque</span
              >
              <div class="text-h4 font-weight-bold mt-1">
                {{ formatedTotalValue }}
              </div>
            </div>
            <v-avatar color="success" variant="tonal" rounded="lg">
              <v-icon icon="mdi-currency-brl" />
            </v-avatar>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-row class="mt-16">
      <v-col cols="12" md="6">
        <v-card rounded="lg" hover @click="navigateTo('/categories')">
          <v-card-title class="d-flex align-center gap-2 text-wrap">
            <v-icon icon="mdi-tag-multiple" color="primary" class="me-3" />
            Gerenciar Categorias
          </v-card-title>
          <v-card-text>
            Cadastre, edite e organize as categorias do seu catálogo.
          </v-card-text>
          <v-card-actions>
            <v-btn color="primary" variant="text" append-icon="mdi-arrow-right">
              Acessar
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" md="6">
        <v-card rounded="lg" hover @click="navigateTo('/products')">
          <v-card-title class="d-flex align-center gap-2 text-wrap">
            <v-icon icon="mdi-package-variant" color="secondary" class="me-3" />
            Gerenciar Produtos
          </v-card-title>
          <v-card-text>
            Gerencie seu catálogo completo de produtos com preços e categorias.
          </v-card-text>
          <v-card-actions>
            <v-btn
              color="secondary"
              variant="text"
              append-icon="mdi-arrow-right"
            >
              Acessar
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>
