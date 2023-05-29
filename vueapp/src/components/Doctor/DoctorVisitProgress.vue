<script setup lang="ts">
import VueDatePicker from "@vuepic/vue-datepicker";
import { router } from "@/main";
const emit = defineEmits(["page"]);
const change_page = (arg: number) => {
  emit("page", arg);
};
</script>

<template>
  <v-card>
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
      <v-card-title font-size="56">Wizyta</v-card-title>
      <v-card-subtitle>Wypełnij dane o wizycie</v-card-subtitle>
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
              Informacje o wizycie
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
          <v-spacer></v-spacer>
        </v-container>
        <v-container>
          <v-row>
            <v-col cols="12" sm="6">
              <v-textarea
                variant="filled"
                auto-grow
                label="Diagnoza"
                rows="5"
                row-height="20"
              ></v-textarea>
            </v-col>
            <v-col cols="12" sm="6">
              <v-textarea
                variant="filled"
                auto-grow
                label="Leki"
                rows="5"
                row-height="20"
              ></v-textarea>
            </v-col>
          </v-row>
          <v-textarea label="Zalecenia"></v-textarea>

          <v-row>
            <p class="font-weight-bold">Termin wizyty kontrolnej</p>
            <v-divider></v-divider>
          </v-row>
          <v-row>
            <v-col>
              <VueDatePicker auto-apply> </VueDatePicker>
            </v-col>
            <v-col>
              <v-checkbox label=""></v-checkbox>
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
            <v-btn
              xs="12"
              sm="6"
              md="3"
              align-self="center"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
              @click="change_page(1)"
            >
              Zakończ wizytę
            </v-btn>
          </v-col>
        </v-row>
        <v-row justify="center">
          <v-col justify="center" class="text-right">
            <v-btn
              variant="text"
              align-self="center"
              size="small"
              class="mt-2 button"
            >
              Zapisz i wyjdź
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
