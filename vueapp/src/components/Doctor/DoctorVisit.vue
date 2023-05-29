<script setup lang="ts">
import { ref } from "vue";
import DoctorVisitProgress from "./DoctorVisitProgress.vue";
import DoctorVisitSummary from "./DoctorVisitSummary.vue";
const page = ref<number>(1);
</script>
<template>
  <v-card
    width="560px"
    location="center"
    elevation="5"
    class="rounded-lg mt-10 mb-10"
  >
    <v-window v-model="page" direction="vertical" reverse :touch="false">
      <v-window-item :value="1">
        <DoctorVisitProgress @page="(arg) => (page += arg)" />
      </v-window-item>
      <v-window-item :value="2">
        <DoctorVisitSummary @page="(arg) => (page += arg)" />
      </v-window-item>
    </v-window>
  </v-card>
</template>

<style>
.button {
  text-transform: unset !important;
}
</style>
