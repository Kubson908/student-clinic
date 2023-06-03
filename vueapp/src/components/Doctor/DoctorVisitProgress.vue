<script setup lang="ts">
import { router, authorized, snackbar } from "@/main";
import { ref, onBeforeMount } from "vue";
// eslint-disable-next-line
const emit = defineEmits(["page", "loaded", "save"]);
const change_page = async (arg: number) => {
  if (arg > 0) {
    const valid = ((await form.value.validate()) as any).valid;
    if (!valid) return;
  }
  emit("page", arg);
};

const medicines = ref<string>();
const symptoms = ref<string>();
const appointmentDate = ref<string>();
const patientName = ref<string>();

const form = ref<any>();
const meds = ref<string>();
const diagnose = ref<string>();
const recommendations = ref<string>();
const control_visit = ref<boolean>(true);
const loading = ref<boolean>(true);

const doctorSpecialization = ref<number>();

const onSaveAndExitClick = () => {
  emit("save");
};

onBeforeMount(async () => {
  try {
    let appointmentId = router.currentRoute.value.params["id"] as string;
    let res = await authorized.get(`Appointment/${appointmentId}`);
    const appointmentData = res.data;
    appointmentDate.value = appointmentData.date;
    patientName.value =
      appointmentData.patient.firstName +
      " " +
      appointmentData.patient.lastName;
    symptoms.value = appointmentData.symptoms;
    medicines.value = appointmentData.medicines;
    doctorSpecialization.value = appointmentData.specialization;
  } catch (error) {
    console.log(error);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd przy pobieraniu danych wizyty";
    snackbar.showing = true;
  } finally {
    loading.value = false;
    emit("loaded");
  }
});

const select = ref<any>();
const date = ref<any>(new Date());
// eslint-disable-next-line
defineExpose({
  appointmentDate,
  patientName,
  symptoms,
  medicines,
  meds,
  diagnose,
  recommendations,
  date,
  select,
  doctorSpecialization,
  control_visit,
});
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
      <v-form @submit.prevent ref="form">
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
            <v-col class="text-left">
              {{
                loading
                  ? "Wczytywanie..."
                  : new Date(appointmentDate as string).toLocaleDateString()
              }},
              {{
                loading
                  ? ""
                  : new Date(appointmentDate as string)
                      .toLocaleTimeString()
                      .substring(0, 5)
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
              {{ loading ? "Wczytywanie" : patientName }}
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
                loading ? "Wczytywanie..." : symptoms ? symptoms : "Nie podano"
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
                loading
                  ? "Wczytywanie..."
                  : medicines
                  ? medicines
                  : "Nie podano"
              }}
            </v-col>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-spacer></v-spacer>
        </v-container>
        <v-container>
          <v-row>
            <v-col cols="12" sm="6">
              <v-textarea
                v-model="diagnose"
                variant="solo"
                auto-grow
                label="Diagnoza"
                rows="5"
                row-height="20"
              ></v-textarea>
            </v-col>
            <v-col cols="12" sm="6">
              <v-textarea
                v-model="meds"
                variant="solo"
                auto-grow
                label="Leki"
                rows="5"
                row-height="20"
              ></v-textarea>
            </v-col>
          </v-row>
          <v-textarea
            variant="solo"
            label="Zalecenia"
            v-model="recommendations"
          ></v-textarea>
          <v-checkbox
            v-model="control_visit"
            label="Wizyta kontrolna"
          ></v-checkbox>
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
              @click="change_page(control_visit ? 1 : 2)"
            >
              {{ control_visit ? "Dalej" : "Zakończ" }}
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
              @click="onSaveAndExitClick()"
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
