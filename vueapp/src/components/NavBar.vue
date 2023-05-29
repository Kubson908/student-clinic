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
  const roles = user.roles!
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
          <router-link to="/patient/patientcard" custom v-slot="{ navigate }">
            <v-tab value="/patient/patientcard" @click="navigate"
              >Pacjenci</v-tab
            >
          </router-link>
        </v-tabs>
      </div>
      <div v-else-if="checkRole('Staff')">
        <v-tabs
          :mandatory="false"
          :model-value="router.currentRoute.value.path"
        >
          <router-link to="/doctor/visit_harmonogram" custom v-slot="{ navigate }">
            <v-tab value="/doctor/visit_harmonogram" @click="navigate"
              >Wizyty</v-tab
            >
          </router-link>
          <router-link to="/staff/patientlist" custom v-slot="{ navigate }">
            <v-tab value="/staff/patientlist" @click="navigate"
              >Pacjenci</v-tab
            >
          </router-link>
          <router-link to="/doctor/doctorpage" custom v-slot="{ navigate }">
            <v-tab value="/doctor/doctorpage" @click="navigate"
              >Lekarze</v-tab
            >
          </router-link>
          <router-link
            to="/staff/appointmentstatistics"
            custom
            v-slot="{ navigate }"
          >
            <v-tab value="/staff/appointmentstatistics" @click="navigate"
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
          <router-link to="/doctor/visit_harmonogram" custom v-slot="{ navigate }">
            <v-tab value="/doctor/visit_harmonogram" @click="navigate"
              >Wizyty</v-tab
            >
          </router-link>
          <router-link to="/staff/patientlist" custom v-slot="{ navigate }">
            <v-tab value="/staff/patientlist" @click="navigate"
              >Pacjenci</v-tab
            >
          </router-link>
        </v-tabs>
      </div>
      
    </template>
    
    <span class="d-inline-block link" >
      {{ user.name }}
      <router-link to="/patient/patientcard" custom v-slot="{ navigate }" >
      <v-app-bar-nav-icon v-if="checkRole('Patient')"
        size="50"
        icon="mdi-account-circle-outline"
        class="icon"
        link
        @click="navigate"
      >
      </v-app-bar-nav-icon>
      <v-app-bar-nav-icon v-else-if="checkRole('Staff')"
        size="50"
        icon="mdi-hospital-building"
        class="icon"
        link
        
      >
      </v-app-bar-nav-icon>
      <v-app-bar-nav-icon v-else-if="checkRole('Employee')"
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
