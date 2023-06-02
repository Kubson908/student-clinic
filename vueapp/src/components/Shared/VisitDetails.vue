<script setup lang="ts">
import { router, snackbar, authorized, specializations } from "@/main";
import { onBeforeMount, ref } from "vue";

let appointment_id: string = "";
const appointment_data = ref<any>(null);
const waiting = ref<boolean>(true);

onBeforeMount(async () => {
  appointment_id = router.currentRoute.value.params["id"] as string;
  await getAppointmentData();
});

const getAppointmentData = async () => {
  try {
    const res = await authorized.get(`/appointment/${appointment_id}`);
    appointment_data.value = res.data;
  } catch (e: any) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd przy pobieraniu danych z serwera";
  } finally {
    waiting.value = false;
  }
};
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg">
    <template #loader>
      <v-progress-linear
        :active="waiting"
        color="deep-purple"
        height="4"
        indeterminate
      ></v-progress-linear>
    </template>
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
            <v-col class="text-left">
              {{
                appointment_data
                  ? `${new Date(
                      appointment_data.date
                    ).toLocaleDateString()}, ${new Date(
                      appointment_data.date
                    ).getHours()}:${(
                      "0" + new Date(appointment_data.date).getMinutes()
                    ).slice(-2)}`
                  : "Wczytywanie..."
              }}
            </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Lekarz
            </v-col>
            <v-col class="text-left">
              {{
                appointment_data
                  ? appointment_data.doctor
                    ? appointment_data.doctor.firstName +
                      " " +
                      appointment_data.doctor.lastName
                    : specializations.find(
                        (specialization) =>
                          specialization.value ===
                          appointment_data.specialization
                      )!.title
                  : "Wczytywanie..."
              }}
            </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              Pacjent
            </v-col>
            <v-col class="text-left">
              {{
                appointment_data
                  ? appointment_data.patient.firstName +
                    " " +
                    appointment_data.patient.lastName
                  : "Wczytywanie..."
              }}
            </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Podane objawy
            </v-col>
            <v-col class="text-left">
              {{
                appointment_data
                  ? appointment_data.symptoms !== ""
                    ? appointment_data.symptoms
                    : "Nie podano"
                  : "Wczytywanie..."
              }}
            </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Stosowane leki
            </v-col>
            <v-col class="text-left">
              {{
                appointment_data
                  ? appointment_data.medicines !== ""
                    ? appointment_data.medicines
                    : "Nie podano"
                  : "Wczytywanie..."
              }}
            </v-col>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-container v-if="appointment_data && appointment_data.finished">
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
                {{
                  appointment_data.diagnosis !== ""
                    ? appointment_data.diagnosis
                    : "Nie podano"
                }}
              </v-col>
            </v-row>
            <v-row>
              <v-col
                class="font-weight-bold text-blue-darken-1 text-left"
                cols="4"
              >
                Leki
              </v-col>
              <v-col class="text-left">
                {{
                  appointment_data.medicines !== ""
                    ? appointment_data.medicines
                    : "Nie podano"
                }}
              </v-col>
            </v-row>
            <v-row>
              <v-col
                cols="4"
                class="font-weight-bold text-blue-darken-1 text-left"
              >
                Zalecenia
              </v-col>
              <v-col class="text-left">
                {{
                  appointment_data.recommendations !== ""
                    ? appointment_data.recommendations
                    : "Nie podano"
                }}
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
          </v-container>
          <v-row><v-divider></v-divider></v-row>
        </v-container>

        <v-row justify="start">
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
