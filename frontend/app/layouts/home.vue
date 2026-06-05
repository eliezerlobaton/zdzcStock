<script setup lang="ts">
const navItems = [
  { to: "/", icon: "mdi-view-dashboard", title: "Dashboard" },
  { to: "/categories", icon: "mdi-tag-multiple", title: "Categorias" },
  { to: "/products", icon: "mdi-package-variant", title: "Produtos" },
];

const route = useRoute();
const pageTitle = computed(
  () => navItems.find((i) => i.to === route.path)?.title ?? "Inventario",
);
</script>

<template>
  <v-app>
    <v-app-bar elevation="0" border="b">
      <v-app-bar-title>{{ pageTitle }}</v-app-bar-title>
    </v-app-bar>

    <v-main>
      <v-container fluid class="pa-4 pa-md-6">
        <slot />
      </v-container>
    </v-main>
  </v-app>
</template>
