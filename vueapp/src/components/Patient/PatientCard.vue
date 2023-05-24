<script setup lang="ts">
//import VueDatePicker from "@vuepic/vue-datepicker";
import { authorized, specializations } from "@/main";
import { onBeforeMount } from "vue";
import { ref } from "vue";
// let test = {};

const name = ref<string>("");
const lastName = ref<string>("");
const phoneNumber = ref<string>("");
const email = ref<string>("");
const pesel = ref<string>("");
const dateOfBirth = ref<string>("");
const treatmentHistory = ref<
  Array<{ id: number; date: string; specialization: number }>
>([]);
// const lastName = ref<string>("");
onBeforeMount(async () => {
  // const res = await authorized.get("/appointment");
  const card = await authorized.get("/patient/patient-card");
  console.log(card.data);
  const data = card.data;
  name.value = data.firstName;
  lastName.value = data.lastName;
  phoneNumber.value = data.phoneNumber;
  email.value = data.email;
  pesel.value = data.pesel;
  dateOfBirth.value = data.dateOfBirth;
  treatmentHistory.value = data.treatmentHistory;
});
</script>

<template>
  <v-card width="888px" location="center" elevation="5" class="rounded-lg px-8">
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
            <v-col class="text-left"> {{ name }} {{ lastName }} </v-col>
            <v-col class="font-weight-bold text-blue-darken-1 text-left"
              >Telefon</v-col
            >
            <v-col>{{ phoneNumber }}</v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Data urodzenia
            </v-col>
            <v-col class="text-left"> {{ dateOfBirth }} </v-col>
            <v-col class="font-weight-bold text-blue-darken-1 text-left"
              >Adres e-mail</v-col
            >
            <v-col> {{ email }}</v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              PESEL
            </v-col>
            <v-col class="text-left"> {{ pesel }} </v-col>
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
          <v-row
            v-for="appointment in treatmentHistory"
            :key="appointment.specialization"
          >
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              {{
                specializations.find(
                  (elem: any) => elem.value === appointment.specialization
                )?.title
              }}
            </v-col>
            <v-col class="text-left" cols="4">
              {{ new Date(appointment.date).toLocaleString() }}
            </v-col>
          </v-row>
        </v-container>
        <v-row justify="center" class="mt-4">
          <v-col class="text-left">
            <v-btn variant="outlined" class="mt-2 button" color="blue-darken-2">
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
