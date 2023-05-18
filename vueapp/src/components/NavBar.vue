<script setup lang="ts">
import { user } from "../main";

const logout = () => {
  localStorage.clear();
  user.name = "Niezalogowany";
  user.isLoggedIn = false;
};
</script>
<template>
  <v-app-bar color="#597EDD" density="compact" theme="dark">
    <template v-slot:prepend>
      <router-link to="/" custom v-slot="{ navigate }">
        <v-app-bar-nav-icon
          icon="mdi-hospital-building"
          class="icon"
          link
          @click="navigate"
        >
        </v-app-bar-nav-icon
      ></router-link>
    </template>
    {{ user.name }}
    <v-menu>
      <template v-slot:activator="{ props }">
        <v-app-bar-nav-icon v-bind="props" class="icon"> </v-app-bar-nav-icon>
      </template>

      <v-list v-if="!user.isLoggedIn" color="white" theme="light" nav>
        <router-link to="/login" custom v-slot="{ navigate }">
          <v-list-item prepend-icon="mdi-login" link @click="navigate">
            <v-list-item-title>Zaloguj</v-list-item-title>
          </v-list-item>
        </router-link>
        <router-link to="/signup" custom v-slot="{ navigate }">
          <v-list-item prepend-icon="mdi-logout" link @click="navigate">
            <v-list-item-title>Zarejestruj się</v-list-item-title>
          </v-list-item>
        </router-link>
      </v-list>
      <v-list v-if="user.isLoggedIn" color="white" theme="light" nav>
        <v-list-item prepend-icon="mdi-login" link @click="logout">
          <v-list-item-title>Wyloguj się</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </v-app-bar>
</template>
<style>
.test {
  display: block;
  width: 64px;
  height: 4.95vh;
}
.icon {
  color: var(--font-primary-color) !important;
}
</style>
