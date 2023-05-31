<script setup lang="ts">
import { ref } from 'vue';
const code = ref<string>("");
</script>

<template>
  <v-card>
    <v-card-item>
      <v-container class="d-flex justify-center align-center">
        <v-card
          height="64"
          width="64"
          color="#36BFF1"
          class="d-flex justify-center align-center"
        >
          <v-icon icon="mdi-hospital-building" size="48" color="white"></v-icon>
        </v-card>
      </v-container>
      <v-card-title class="font-weight-bold text-h5" font-size="56">
        Na podany e-mail przesłano kod
      </v-card-title>
    </v-card-item>
    <v-card-text>
      <v-text-field
        type="input"
        v-model="code"
        label="Kod"
        variant="solo"
        color="blue-darken-2"
        required
      >
      </v-text-field>
    </v-card-text>
  </v-card>
</template>

<style>
.button {
  text-transform: unset !important;
}
</style>
