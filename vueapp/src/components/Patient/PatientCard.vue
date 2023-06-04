<script setup lang="ts">
//import VueDatePicker from "@vuepic/vue-datepicker";
import { authorized, specializations, router, snackbar } from "@/main";
import { onBeforeMount } from "vue";
import { ref } from "vue";
// let test = {};

const name = ref<string>("");
const lastName = ref<string>("");
const medicines = ref<string>("");
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
  const patientId = router.currentRoute.value.params["id"] as string;
  try {
    let card = null;
    if (patientId == undefined)
      card = await authorized.get("/patient/patient-card");
    else card = await authorized.get("/patient/patient-card/" + patientId);
    const data = card.data;
    name.value = data.firstName;
    lastName.value = data.lastName;
    medicines.value = data.medicines;
    phoneNumber.value = data.phoneNumber;
    email.value = data.email;
    pesel.value = data.pesel;
    dateOfBirth.value = data.dateOfBirth;
    treatmentHistory.value = data.treatmentHistory;
  } catch (error: any) {
    console.log(error);
    snackbar.error = true;
    snackbar.text = "Błąd przy pobieraniu danych karty pacjenta";
    snackbar.showing = true;
  }
});
</script>

<template>
  <v-row no-gutters>
    <v-col cols="12" class="my-auto">
      <v-card width="888px" elevation="5" class="rounded-lg px-8 ma-auto">
        <v-card-item>
          <v-container class="d-flex justify-center align-center">
            <v-card
              height="64"
              width="64"
              color="#36BFF1"
              class="d-flex justify-center align-center"
            >
              <v-icon
                icon="mdi-hospital-building"
                size="48"
                color="white"
              ></v-icon>
            </v-card>
          </v-container>
          <v-card-title font-size="56">Karta Pacjenta</v-card-title>
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
                <v-col
                  v-if="medicines == null"
                  class="text-blue-darken-1 font-weight-bold text-left"
                >
                  Brak leków
                </v-col>
                <v-col class="font-weight-bold text-blue-darken-1 text-left">
                  {{ medicines }}
                </v-col>
              </v-row>
            </v-container>
            <v-container>
              <v-row>
                <p class="font-weight-bold">Historia Leczenia</p>
              </v-row>
              <v-row><v-divider></v-divider></v-row>
              <div class="scrollable mt-3">
                <v-row no-gutters>
                  <v-col
                    class="text-blue-darken-1 font-weight-bold text-left pt-2"
                    v-if="treatmentHistory.length === 0"
                  >
                    Brak Historii
                  </v-col>
                </v-row>
                <v-row
                  no-gutters
                  class="history my-4"
                  v-for="appointment in treatmentHistory"
                  :key="appointment.specialization"
                >
                  <v-col
                    class="font-weight-bold text-blue-darken-1 text-left d-flex align-center"
                    cols="4"
                  >
                    {{
                      specializations.find(
                        (elem: any) => elem.value === appointment.specialization
                      )?.title
                    }}
                  </v-col>
                  <v-col class="text-left d-flex align-center" cols="4">
                    {{
                      (
                        "0" + new Date(appointment.date).toLocaleString("pl-PL")
                      ).slice(-20)
                    }}
                  </v-col>
                  <v-col class="text-left d-flex align-center" cols="4">
                    <router-link
                      :to="'/doctor/appointment/' + appointment.id"
                      custom
                      v-slot="{ navigate }"
                    >
                      <v-btn
                        color="blue-darken-2"
                        class="button"
                        variant="text"
                        @click="navigate"
                      >
                        <u>Szczegóły</u>
                      </v-btn>
                    </router-link>
                  </v-col>
                </v-row>
              </div>
            </v-container>
            <v-row justify="center" class="mt-4">
              <v-col class="text-left">
                <v-btn
                  variant="outlined"
                  class="mt-2 button"
                  color="blue-darken-2"
                  @click="router.back()"
                >
                  Wstecz
                </v-btn>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>
    </v-col></v-row
  >
</template>

<style>
.button {
  text-transform: unset !important;
}
.scrollable {
  overflow-y: auto !important;
  height: 20vh;
}
</style>
