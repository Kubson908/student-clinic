<script setup lang="ts"></script>

<template>
  <v-card max-width="288" class="rounded-lg">
    <v-card-item class="text-left">
      <slot name="header"></slot>
    </v-card-item>
    <v-card-text class="text-justify">
      <slot> </slot>
    </v-card-text>
  </v-card>
</template>
<style></style>
