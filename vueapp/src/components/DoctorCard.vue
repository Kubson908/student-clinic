<script setup lang="ts">
import { defineProps } from "vue";
defineProps({
  img: String,
});
</script>

<template>
  <v-card width="288">
    <v-row no-gutters>
      <v-col cols="6">
        <v-img
          cover
          src="https://img.favpng.com/24/11/9/physician-medicine-stock-photography-health-care-clinic-png-favpng-U8PcYt9GTDcuyNQMGgUhAhivX.jpg"
        ></v-img>
      </v-col>
      <v-col cols="6"> </v-col>
    </v-row>
  </v-card>
</template>

<style></style>
