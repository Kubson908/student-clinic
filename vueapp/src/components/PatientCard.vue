<script setup lang="ts">
//import VueDatePicker from "@vuepic/vue-datepicker";
import "@vuepic/vue-datepicker/dist/main.css";
// let date = new Date(Date.now() - new Date().getTimezoneOffset() * 60000)
//   .toISOString()
//   .substr(0, 10);
// let menu = false;
// let modal = false;
// let menu2 = false;
</script>

<template>
  <v-card width="888px" location="center" elevation="5" class="rounded-lg">
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
      <v-card-title font-size="56">Karta pacjenta</v-card-title>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent>
        <v-container>
          <v-row>
            <p class="font-weight-bold">Dane osobowe</p>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Imię i Nazwisko
            </v-col>
            <v-col class="text-left"> Jan Ambroziak </v-col>
            <v-col class="font-weight-bold text-blue-darken-1 text-left"
              >telefon</v-col
            >
            <v-col>574987248</v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Data urodzenia
            </v-col>
            <v-col class="text-left"> 15.07.1980 </v-col>
            <v-col class="font-weight-bold text-blue-darken-1 text-left"
              >Adres e-mail</v-col
            >
            <v-col> jan.am1@poczta.pl</v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              PESEL
            </v-col>
            <v-col class="text-left"> 80071545579 </v-col>
          </v-row>
        </v-container>

        <v-container>
          <v-row>
            <p class="font-weight-bold">Lista Leków</p>
          </v-row>
          <v-row><v-divider></v-divider></v-row>

          <v-row>
            <v-col class="font-weight-bold text-blue-darken-1 text-left">
              Quinilaril Azagel
            </v-col>
          </v-row>
          <v-row>
            <v-col class="font-weight-bold text-blue-darken-1 text-left">
              Silvalozin
            </v-col>
          </v-row>
        </v-container>
        <v-container>
          <v-row>
            <p class="font-weight-bold">Historia Leczenia</p>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Pulmonolog
            </v-col>
            <v-col class="text-left"> 13.02.2023 </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Internista
            </v-col>
            <v-col class="text-left"> 17.03.2023</v-col>
          </v-row>
        </v-container>
        <v-row justify="center">
          <v-col class="text-left">
            <v-btn
              variant="outlined"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
            >
              Wstecz
            </v-btn>
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
  </v-card>
</template>

<style>
.button {
  text-transform: unset !important;
}
</style>
