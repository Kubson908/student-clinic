<script setup lang="ts">
import DoctorCard from "./DoctorCard.vue";
const doctors: Array<any> = [{
    id: 1,
    name: "Tomasz Problem",
},
{
    id: 2,
    name: "Jan Testowy"
}];
</script>
<template>
  <v-row no-gutters justify="center">
    <v-col cols="12" sm="6" md="6" align-self="center">
      <v-card elevation="5" class="rounded-lg" height="70vh">
        <v-card-item>
            <v-container class="d-flex justify-center align-center">
            <h1>Lekarze</h1>
          </v-container>
        </v-card-item>
        <v-card-text>
            <DoctorCard v-for="doctor in doctors" :key="doctor.id">
            </DoctorCard>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>
<style></style>
