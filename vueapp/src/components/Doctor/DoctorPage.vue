<script setup lang="ts">
import DoctorCard from "./DoctorCard.vue";
import { ref } from "vue";
import { onBeforeMount } from "vue";
import { authorized } from "@/main";

const doctors = ref<any[]>();
const loading = ref<boolean>(true);
onBeforeMount(async () => {
  try {
    const card = await authorized.get("/employee/");
    doctors.value = card.data.filter((emp: any) => emp.specialization !== null);
    loading.value = false;
  } catch (e) {
  } finally {
  }
});
</script>
<template>
  <v-row no-gutters justify="center">
    <v-col cols="12" sm="10" md="8" class="py-16" align-self="center">
      <v-card elevation="5" class="rounded-lg">
        <v-card-item>
          <v-container class="d-flex justify-center align-center">
            <h1>Lekarze</h1>
          </v-container>
        </v-card-item>
        <v-card-text>
          <v-row class="pa-8">
            <v-col
              v-for="doctor in doctors"
              :key="doctor.id"
              cols="12"
              sm="6"
              md="4"
              class="d-flex justify-center"
            >
              <DoctorCard
                img="https://img.favpng.com/24/11/9/physician-medicine-stock-photography-health-care-clinic-png-favpng-U8PcYt9GTDcuyNQMGgUhAhivX.jpg"
                :doctor="doctor"
                :disabled="false"
              >
              </DoctorCard>
            </v-col>
            <v-col
              v-if="loading"
              v-for="elem in Array(6)"
              cols="12"
              sm="6"
              md="4"
              class="d-flex justify-center"
            >
              <v-skeleton-loader type="card-avatar" width="288">
              </v-skeleton-loader>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>
<style></style>
