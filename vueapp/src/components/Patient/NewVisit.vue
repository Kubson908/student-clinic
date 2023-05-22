<script lang="ts" setup>
import { ref } from "vue";
import NewVisitSymptoms from "./NewVisitSymptoms.vue";
import NewVisitDate from "./NewVisitDate.vue";
import NewVisitSummary from "./NewVisitSummary.vue";
const page = ref<number>(1);
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg mt-10">
    <v-window v-model="page" direction="vertical" reverse :touch="false">
      <v-window-item :value="1">
        <NewVisitSymptoms @page="(arg) => (page += arg)" />
      </v-window-item>

      <v-window-item :value="2">
        <NewVisitDate @page="(arg) => (page += arg)" />
      </v-window-item>
      <v-window-item :value="3">
        <NewVisitSummary @page="(arg) => (page += arg)" />
      </v-window-item>
    </v-window>
  </v-card>
</template>

<style>
.button {
  text-transform: unset !important;
}
</style>
