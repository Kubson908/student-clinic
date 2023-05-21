<script setup lang="ts">
import { user, router } from "../main";
const logout = () => {
  localStorage.clear();
  user.name = "Niezalogowany";
  user.isLoggedIn = false;
  user.role = undefined;
};
// console.log(router.currentRoute.value)
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

      <div v-if="user.role == 'Patient'">
        <v-tabs mandatory="false" :model-value="router.currentRoute.value.path">
          <router-link to="/patient/appointments" custom v-slot="{ navigate }">
            <v-tab value="/patient/appointments" @click="navigate">Wizyty</v-tab>
          </router-link>
          <router-link to="/patient/patientcard" custom v-slot="{ navigate }">
            <v-tab value="/patient/patientcard" @click="navigate">Pacjenci</v-tab>
          </router-link>
        </v-tabs>
      </div>
      <div v-else-if="user.role == 'Employee'">
        <v-tabs mandatory="false" :model-value="router.currentRoute.value.path">
          <router-link to="/patient/appointments" custom v-slot="{ navigate }">
            <v-tab value="/patient/appointments" @click="navigate">Wizyty</v-tab>
          </router-link>
          <router-link to="/patient/patientcard" custom v-slot="{ navigate }">
            <v-tab value="/patient/patientcard" @click="navigate">Pacjenci</v-tab>
          </router-link>
        </v-tabs>
      </div>
    </template>
    <span class="d-inline-block link">
      {{ user.name }}
    </span>
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
        <v-list-item prepend-icon="mdi-logout" link @click="logout">
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
.link {
  width: 150px;
  color: white;
}
</style>
