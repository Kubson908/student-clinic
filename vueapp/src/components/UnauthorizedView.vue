<script setup lang="ts"></script>

<template>
  <v-card width="300px" location="center" elevation="5" class="rounded-lg">
      <v-card-item>
        <v-card-title>Odmowa dostępu</v-card-title>
        <v-icon
          icon="mdi-shield-lock-outline"
          size="150"
          color="black"
        ></v-icon>
      </v-card-item>
      <v-spacer></v-spacer>

      <v-spacer></v-spacer>
      <v-card-text>
        <v-row justify="center">
          <router-link to="/login">
            <v-btn type="submit" color="blue-darken-2" class="mt-2 mb-2">
              Zaloguj się
            </v-btn>
          </router-link>
        </v-row>
        <v-row justify="center">
          <router-link to="/">
            <v-btn type="text" variant="outlined"  color="blue-darken-2" class="mt-2 mb-2">
              Strona główna
            </v-btn>
          </router-link>
        </v-row>
      </v-card-text>
    
  </v-card>
</template>

<style></style>
