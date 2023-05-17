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
  <v-card width="560px" location="center" elevation="5" class="rounded-lg">
    <v-card-item>
      <!-- <v-card-title font-size="32">Twoje dane</v-card-title> -->
      <p class="font-weight-bold text-h4">Twoje dane</p>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent>
        <v-container>
          <v-row>
            <v-col align="center">
              <v-img
                :width="100"
                aspect-ratio="1/1"
                cover
                src="./img/doctor.PNG"
              ></v-img>
            </v-col>
          </v-row>
          <v-row>
            <p class="font-weight-bold">Dane osobowe</p>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Imię i nazwisko
            </v-col>
            <v-col class="text-left"> Maciej Kowalczyk </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              PESEL
            </v-col>
            <v-col class="text-left"> 48322349428 </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              Data urodzenia
            </v-col>
            <v-col class="text-left"> 12.05.1979r. </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Email
            </v-col>
            <v-col class="text-left"> m.kowalczyk@poczta.pl </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Specjalizacja
            </v-col>
            <v-col class="text-left"> Pulmunolog </v-col>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
        </v-container>

        <v-row justify="start">
          <v-col xs="12" sm="6" md="3" align-self="center" class="text-left">
            <v-btn
              variant="outlined"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
            >
              Wstecz
            </v-btn>
            <v-btn
              variant="text"
              align-self="center"
              size="small"
              class="mt-2 button"
            >
              Zmień hasło
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
