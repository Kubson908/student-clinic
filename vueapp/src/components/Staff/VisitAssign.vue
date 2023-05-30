<script setup lang="ts">
import {router} from "@/main";
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
      <v-card-title font-size="56">Przydziel wizytę</v-card-title>
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
            <v-col class="text-left"> Pulmonolog </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
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
              Przyjmowane leki
            </v-col>
            <v-col class="text-left"> Riposton </v-col>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <!--<v-spacer></v-spacer>-->
          <v-row>
            <p class="font-weight-bold">Lekarz</p>
          </v-row>
          <v-row>
            <v-col>
              <v-select
                label="Wybierz lekarza"
                :items="['Jan Chomar', 'Grzegorz Brzęczyszczykiewicz']"
              >
              </v-select>
            </v-col>
          </v-row>
        </v-container>

        <v-row justify="center">
          <v-col xs="12" sm="6" md="3" align-self="center" class="text-left">
            <v-btn
              variant="outlined"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
              @click="router.back()"
            >
              Wstecz
            </v-btn>
          </v-col>
          <v-col justify="center" class="text-right">
            <router-link to="/staff/awaitingappointments" custom v-slot="{ navigate }">
              <v-btn
              xs="12"
              sm="6"
              md="3"
              align-self="center"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
              value="/staff/awaitingappointments"
              @click="navigate"
            >
              Przydziel
            </v-btn>
            </router-link>
            
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
