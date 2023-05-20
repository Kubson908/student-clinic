<script setup lang="ts">
<<<<<<< Updated upstream

=======
//Wojtek
// const appointments = [
//   {
//     id: 1,
//     patient: "Tomasz Problem",
//     date: "05/18/2023",
//     symptom: "Ból głowy",
//     medicines: "Rutinoskorbin"
//   },
//   {
//     id: 2,
//     patient: "Andrew Tate",
//     date: "05/20/2023",
//     symptom: "Ból głowy, kaszel",
//     medicines: "Gripeks"
//   },
//   {
//     id: 3,
//     patient: "Greta Thunberg",
//     date: "05/21/2023",
//     symptom: "Ból głowy, katar",
//     medicines: "Rutinoskorbin"
//   }
// ];
// const doctor = [
//   {
//     id: 1,
//     patient: "Adam Jaki",
//   },
//   {
//     id: 2,
//     patient: "Wiktor Chuj",
//   }
// ];

//import VueDatePicker from "@vuepic/vue-datepicker";
// let date = new Date(Date.now() - new Date().getTimezoneOffset() * 60000)
//   .toISOString()
//   .substr(0, 10);
// let menu = false;
// let modal = false;
// let menu2 = false;
>>>>>>> Stashed changes
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
      <v-card-title font-size="56">Przydziel wizyt�</v-card-title>
      <v-card-subtitle>Wybierz lekarza</v-card-subtitle>
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
              Pacjent
            </v-col>
            <v-col class="text-left"> Jan ambroziak </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              Specjalizacja
            </v-col>
            <v-col class="text-left"> Punmonolog </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              Podane objawy
            </v-col>
            <v-col class="text-left"> Kaszel, katar, gor�czka </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Przyjmowane leki
            </v-col>
            <v-col class="text-left"> Riposton </v-col>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-spacer></v-spacer>
        </v-container>
    
        <v-row justify="center">
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
          <v-col justify="center" class="text-right">
            <v-btn
              xs="12"
              sm="6"
              md="3"
              align-self="center"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
            >
              Przydziel
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