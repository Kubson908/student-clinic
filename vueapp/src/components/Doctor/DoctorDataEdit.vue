<script setup lang="ts">
//import { ref } from "vue";
//const name = ref<string>("");
//const lastName = ref<string>("");
//const email = ref<string>("");
//const pesel = ref<string>("");
//const phone = ref<string>("");
//const birthDate = ref<string>("");
//const specialization = ref<string>("");
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg">
    <v-card-item>
      <!-- <v-card-title font-size="32">Twoje dane</v-card-title> -->
      <p class="font-weight-bold text-h4">Dane lekarza</p>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent>
        <v-container>
          <v-row>
            <v-col align="center">
              <v-img height="200" src="https://st2.depositphotos.com/1010683/5848/i/950/depositphotos_58482379-stock-photo-male-asian-doctor.jpg">
            </v-img>
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
            <v-col class="text-left"> Pulmonolog </v-col>
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
