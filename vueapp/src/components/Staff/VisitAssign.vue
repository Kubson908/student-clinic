Truskawa nie śpij
<script setup lang="ts">
import { router, snackbar, authorized, specializations } from "@/main";
import { onBeforeMount, ref } from "vue";
import { notNull } from "@/validation";

let appointment_id: string = "";
const appointment_data = ref<any>(null);
const waiting = ref<boolean>(true);
const available_doctors = ref<Array<any>>([]);
const selected_doctor = ref<string | null>(null);
const loading = ref<boolean>(false);
const form = ref<any>();

onBeforeMount(async () => {
  appointment_id = router.currentRoute.value.params["id"] as string;
  await getAppointmentData();
  console.log(appointment_data.value);
  await getAvailableDoctors();
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
const getAvailableDoctors = async () => {
  try {
    const res = await authorized.get("/employee/available-doctors", {
      params: {
        date: appointment_data.value.date,
        specialization: appointment_data.value.specialization,
      },
    });
    available_doctors.value = res.data;
    if (available_doctors.value.length === 1) selected_doctor.value = available_doctors.value[0].id;
  } catch (e: any) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd przy pobieraniu danych z serwera";
  } finally {
    waiting.value = false;
  }
};
const assign = async () => {
  form.value.valida;
  try {
    loading.value = true;
    if (available_doctors.value.length == 1)
      selected_doctor.value = available_doctors.value[0].id;
    await authorized.patch(
      "appointment/assign-appointment/" + appointment_data.value.id,
      {
        doctorId: selected_doctor.value,
      }
    );
    router.push("/staff/appointments/awaiting");
    snackbar.error = false;
    snackbar.text = "Pomyślnie przypisano wizytę";
  } catch (e) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Nie udało się przypisać tej wizyty";
  } finally {
    snackbar.showing = true;
    loading.value = false;
  }
};
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg">
    <template #loader>
      <v-progress-linear
        :active="loading"
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
            <v-col class="text-left">
              {{
                appointment_data
                  ? `${new Date(appointment_data.date).toLocaleDateString(
                      "pl-PL"
                    )}, ${new Date(appointment_data.date).getHours()}:${(
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
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              Specjalizacja
            </v-col>
            <v-col class="text-left">
              {{
                appointment_data
                  ? specializations.find(
                      (s) => s.value === appointment_data.specialization
                    )?.title
                  : "Wczytywanie..."
              }}
            </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
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
              Przyjmowane leki
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

          <!--<v-spacer></v-spacer>-->
          <v-row>
            <p class="font-weight-bold">Lekarz</p>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-row>
            <v-col>
              <v-form ref="form">
                <v-select
                  label="Wybierz lekarza"
                  :items="available_doctors"
                  variant="solo"
                  item-value="id"
                  :rules="notNull"
                  :disabled="available_doctors.length <= 1"
                  :item-title="(item) => item.firstName + ' ' + item.lastName"
                  v-model="selected_doctor"
                >
                </v-select>
              </v-form>
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
              value="/staff/awaitingappointments"
              @click="assign"
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
