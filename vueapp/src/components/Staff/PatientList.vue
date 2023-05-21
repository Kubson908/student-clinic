<script setup lang="ts">
import { ref } from "vue";

const search = ref<string>("");

const patients = [
  {
    id: 1,
    name: "Jan Ambroziak",
  },
  {
    id: 2,
    name: "Jan Ambroziak",
  },
  {
    id: 3,
    name: "Jan Ambroziak",
  },
  {
    id: 4,
    name: "Jan Ambroziak",
  },
  {
    id: 5,
    name: "Jan Ambroziak",
  },
];
</script>
<template>
  <v-row justify="center" no-gutters>
    <v-col cols="12" sm="6" md="6" align-self="center">
      <v-card elevation="5" class="rounded-lg">
        <v-card-item>
          <v-container class="d-flex justify-center align-center">
            <h1>Pacjenci</h1>
          </v-container>
          <v-row no-gutters class="justify-center text-center"
            ><v-col cols="9" class="d-flex justify-center"
              ><v-text-field
                label="Wyszukiwarka pacjentów"
                v-model="search"
                class="w-75"
                variant="solo"
                append-inner-icon="mdi-account-search"
                placeholder="Imię Nazwisko"
              ></v-text-field></v-col
          ></v-row>
        </v-card-item>
        <v-card-text>
          <v-divider></v-divider>
          <v-list height="50vh">
            <v-list-item
              v-for="patient in patients"
              :key="patient.id"
              class="mx-8 my-4 pa-4 rounded-lg"
              elevation="3"
            >
              <template #prepend>
                <v-list-item-title class="mx-4">{{
                  patient.name
                }}</v-list-item-title>
              </template>
              <template #append>
                <v-btn class="ma-2 button hp-dark">Dane pacjenta</v-btn>
                <v-btn variant="outlined" class="ma-2 button hp-bright"
                  >Karta pacjenta</v-btn
                >
              </template>
            </v-list-item>
          </v-list>
          <v-divider></v-divider>
        </v-card-text>
        <v-card-actions>
          <v-btn variant="outlined" color="blue-darken-2" class="mx-8 my-2"
            >Wstecz</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>
<style></style>
