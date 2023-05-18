<script setup lang="ts">
//import VueDatePicker from "@vuepic/vue-datepicker";
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
        Szczegóły wizyty
      </v-card-title>
      <v-card-subtitle>Dane wizyty</v-card-subtitle>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent>
        <v-container>
          <v-row>
            <p class="font-weight-bold">Informacje o wizycie</p>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Data wizyty
            </v-col>
            <v-col class="text-left"> 17.03.2023, 17:30 </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Lekarz
            </v-col>
            <v-col class="text-left"> Maciej Kowalczyk </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              Pacjent
            </v-col>
            <v-col class="text-left"> Jan Kowalski </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Podane objawy
            </v-col>
            <v-col class="text-left"> Kaszel, katar, gorączka </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Stosowane leki
            </v-col>
            <v-col class="text-left"> Riposton </v-col>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-row>
            <p class="font-weight-bold">Informacje od lekarza</p>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Diagnoza
            </v-col>
            <v-col class="text-left">
              Pacjent choruje na bakteryjne zapalenie zatok.
            </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Leki
            </v-col>
            <v-col class="text-left"> Augmentin 1000mg </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              Zalecenia
            </v-col>
            <v-col class="text-left">
              Augmenting - dawkowanie 2 tabletki/doba.
            </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Wizyta kontrolna
            </v-col>
            <v-col class="text-left"> 31.03.2023, 8:00 </v-col>
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
