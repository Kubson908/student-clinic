<script setup></script>
<template>
  <v-app-bar color="#597EDD" density="compact" theme="dark">
    <template v-slot:prepend>
      <v-app-bar-nav-icon icon="mdi-medical-bag" class="icon">
      </v-app-bar-nav-icon>
    </template>

    <v-menu>
      <template v-slot:activator="{ props }">
        <v-app-bar-nav-icon v-bind="props" class="icon"> </v-app-bar-nav-icon>
      </template>

      <v-list color="white" theme="light" nav>
        <v-list-item prepend-icon="mdi-login" link>
          <v-list-item-title>Zaloguj</v-list-item-title>
        </v-list-item>
        <v-list-item prepend-icon="mdi-logout" link>
          <v-list-item-title>Zarejestruj się</v-list-item-title>
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
