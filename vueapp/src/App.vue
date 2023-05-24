<script setup lang="ts">
import { onBeforeMount, watch } from "vue";
import NavBar from "./components/NavBar.vue";
import { user, snackbar } from "./main";
// import FooterBar from "./components/FooterBar.vue";
let timeout: any = null;

onBeforeMount(() => {
  const date = Date.parse(localStorage.getItem("expireDate") as string);
  if (date < Date.now()) {
    localStorage.clear();
    user.name = "Niezalogowany";
    user.isLoggedIn = false;
  }
});
watch(() => snackbar.showing, (curr, prev) => {
  if (!curr && prev) {
    timeout = setTimeout(() => {
      snackbar.text = "";
      snackbar.error = false;
    }, 2000)
  }
});
</script>

<template>
  <v-app class="v-theme--light">
    <NavBar />
    <router-view v-slot="{ Component }">
      <v-fade-transition mode="out-in">
        <component :is="Component"
      /></v-fade-transition>
    </router-view>
    <!-- <FooterBar class="align-end"/> -->
    <v-snackbar
      location="top"
      class="mt-16"
      v-model="snackbar.showing"
      timeout="3000"
      :color="snackbar.error ? 'error' : 'success'"
    >
      {{ snackbar.text }}

      <template v-slot:actions>
        <v-btn variant="text" @click="snackbar.showing = false">
          Zamknij
        </v-btn>
      </template>
    </v-snackbar>
  </v-app>
</template>

<style>
:root {
  --secondary: #fff;
  --primary: rgb(89, 126, 221);
}

.body {
  min-height: 100vh !important;
}

#app {
  font-family: Souvenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  min-height: 100vh !important;
}
.details-enter-active {
  animation: popup 0.25s;
}
.details-leave-active {
  animation: popup 0.25s reverse;
}
.details-enter-active,
.details-leave-active {
  transition: opacity 0.25s ease;
}
.details-enter-from,
.details-leave-to {
  opacity: 0;
}
</style>
