<script setup lang="ts">
import { onBeforeMount, watch } from "vue";
import NavBar from "./components/NavBar.vue";
import { user, snackbar } from "./main";
import { connect } from "./socket";
// import FooterBar from "./components/FooterBar.vue";
let timeout: any = null;

onBeforeMount(() => {
  const date = Date.parse(localStorage.getItem("expireDate") as string);
  if (date < Date.now()) {
    localStorage.clear();
    user.name = "Niezalogowany";
    user.isLoggedIn = false;
  }
  if (user.isLoggedIn) connect();
});
watch(
  () => snackbar.showing,
  (curr, prev) => {
    if (!curr && prev) {
      timeout = setTimeout(() => {
        snackbar.text = "";
        snackbar.error = false;
      }, 2000);
    }
    if (curr && !prev) {
      clearTimeout(timeout);
    }
  }
);
</script>

<template>
  <v-app class="v-theme--light">
    <NavBar />
    <router-view v-slot="{ Component }">
      <v-fade-transition mode="out-in">
        <component class="mt-12" :is="Component"
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
  scrollbar-width: thin;
  scrollbar-color: #888;
  scrollbar-track-color: #f1f1f1;
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

.list-move,
.list-enter-active,
.list-leave-active {
  transition: all 0.5s ease;
}

.list-enter-from,
.list-leave-to {
  opacity: 0;
  transform: translateX(30px);
}

.list-leave-active {
  position: absolute;
}

/* width */
::-webkit-scrollbar {
  width: 8px;
  display: none;
}

div::-webkit-scrollbar {
  width: 8px;
  height: 8px;
  display: block;
}

/* Track */
::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 4px;
}

/* Handle */
::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 4px;
}

/* Handle on hover */
::-webkit-scrollbar-thumb:hover {
  background: #777;
}
</style>
