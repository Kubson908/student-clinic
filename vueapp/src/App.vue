<script setup lang="ts">
import NavBar from "./components/NavBar.vue";
// import FooterBar from "./components/FooterBar.vue";


</script>

<template>
  <v-app class="v-theme--light">
    <NavBar />
    <router-view v-slot="{ Component }">
        <Transition name="details" mode="out-in">
            <component :is="Component" class="test2"/>
        </Transition>
    </router-view>
    <!-- <FooterBar class="align-end"/> -->
  </v-app>
</template>

<style>
:root {
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
.details-enter-active .test2 {
  animation: popup 0.25s;
}
.details-leave-active .test2 {
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
