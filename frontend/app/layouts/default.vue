<script setup lang="ts">
const { smAndUp } = useDisplay();
const drawer = ref(smAndUp.value);

const route = useRoute();

const navItems = [
  { to: "/", icon: "mdi-view-dashboard", title: "Dashboard" },
  { to: "/categories", icon: "mdi-tag-multiple", title: "Categorias" },
  { to: "/products", icon: "mdi-package-variant", title: "Produtos" },
];

const pageTitle = computed(
  () => navItems.find((i) => i.to === route.path)?.title ?? "Inventario",
);
</script>

<template>
  <v-app>
    <v-navigation-drawer
      v-if="route.path !== '/'"
      v-model="drawer"
      :permanent="$vuetify.display.smAndUp"
      :temporary="$vuetify.display.xs"
    >
      <v-list-item
        title="Inventário Comercial"
        subtitle="ZDZCloud · 2026"
        prepend-icon="mdi-store"
        class="py-4"
      />

      <v-divider />
      <v-list nav>
        <v-list-item
          v-for="item in navItems"
          :key="item.to"
          :to="item.to"
          :prepend-icon="item.icon"
          :title="item.title"
          rounded="lg"
          color="primary"
        />
      </v-list>
    </v-navigation-drawer>

    <v-app-bar elevation="0" border="b">
      <v-app-bar-nav-icon
        v-if="route.path !== '/'"
        class="d-md-none"
        @click="drawer = !drawer"
      />
      <v-app-bar-title>{{ pageTitle }}</v-app-bar-title>
    </v-app-bar>

    <v-main>
      <v-container fluid class="pa-4 pa-md-6">
        <slot />
      </v-container>
    </v-main>
  </v-app>
</template>
