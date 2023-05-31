<script setup lang="ts">
import { user, router } from "../main";
import { socket } from "@/socket";
const logout = () => {
  localStorage.clear();
  user.name = "Niezalogowany";
  user.isLoggedIn = false;
  user.roles = [];
  router.push("/");
  socket.value?.close();
  // console.log(user.roles);
};
const checkRole = (role: string) => {
  const roles = user.roles!;
  return roles.includes(role);
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

      <div v-if="checkRole('Patient')">
        <v-tabs
          :mandatory="false"
          :model-value="router.currentRoute.value.path"
        >
          <router-link to="/patient/appointments" custom v-slot="{ navigate }">
            <v-tab value="/patient/appointments" @click="navigate"
              >Wizyty</v-tab
            >
          </router-link>
          <router-link to="/patient/card" custom v-slot="{ navigate }">
            <v-tab value="/patient/card" @click="navigate">Pacjenci</v-tab>
          </router-link>
        </v-tabs>
      </div>
      <div v-else-if="checkRole('Staff')">
        <v-tabs
          :mandatory="false"
          :model-value="router.currentRoute.value.path"
        >
          <router-link
            to="/staff/appointments/awaiting"
            custom
            v-slot="{ navigate }"
          >
            <v-tab value="/staff/appointments/awaiting" @click="navigate"
              >Wizyty</v-tab
            >
          </router-link>
          <router-link to="/staff/patients" custom v-slot="{ navigate }">
            <v-tab value="/staff/patients" @click="navigate">Pacjenci</v-tab>
          </router-link>
          <router-link to="/staff/doctors" custom v-slot="{ navigate }">
            <v-tab value="/staff/doctors" @click="navigate">Lekarze</v-tab>
          </router-link>
          <router-link to="/staff/statistics" custom v-slot="{ navigate }">
            <v-tab value="/staff/statistics" @click="navigate"
              >Statystyka</v-tab
            >
          </router-link>
        </v-tabs>
      </div>
      <div v-else-if="checkRole('Employee')">
        <v-tabs
          :mandatory="false"
          :model-value="router.currentRoute.value.path"
        >
          <router-link to="/doctor/harmonogram" custom v-slot="{ navigate }">
            <v-tab value="/doctor/harmonogram" @click="navigate">Wizyty</v-tab>
          </router-link>
          <router-link to="/doctor/patients" custom v-slot="{ navigate }">
            <v-tab value="/doctor/patients" @click="navigate">Pacjenci</v-tab>
          </router-link>
        </v-tabs>
      </div>
    </template>

    <span class="d-inline-block link">
      {{ user.name }}
      <router-link
        v-if="checkRole('Patient')"
        to="/patient/profile"
        custom
        v-slot="{ navigate }"
      >
        <v-app-bar-nav-icon
          size="50"
          icon="mdi-account-circle-outline"
          class="icon"
          link
          @click="navigate"
        >
        </v-app-bar-nav-icon>
      </router-link>
      <router-link
        v-else-if="checkRole('Staff')"
        to="/staff/profile"
        custom
        v-slot="{ navigate }"
      >
        <v-app-bar-nav-icon
          size="50"
          icon="mdi-hospital-building"
          class="icon"
          link
          @click="navigate"
        >
        </v-app-bar-nav-icon>
      </router-link>
      <router-link
        v-else-if="checkRole('Employee')"
        to="/doctor/profile"
        custom
        v-slot="{ navigate }"
      >
        <v-app-bar-nav-icon
          size="50"
          icon="mdi-doctor"
          class="icon"
          link
          @click="navigate"
        >
        </v-app-bar-nav-icon>
      </router-link>
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
  width: 300px;
  color: white;
  text-align: right;
}
</style>
