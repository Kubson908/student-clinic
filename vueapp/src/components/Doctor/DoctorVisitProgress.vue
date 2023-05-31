<script setup lang="ts">
import VueDatePicker from "@vuepic/vue-datepicker";
import { router, authorized, snackbar, specializations } from "@/main";
import { ref, onBeforeMount, onMounted, computed, watch } from "vue";
import { notNull } from "@/validation";
// eslint-disable-next-line
const emit = defineEmits(["page"]);
const change_page = (arg: number) => {
  emit("page", arg);
};

const medicines = ref<string>();
const symptoms = ref<string>();
const appointmentDate = ref<string>();
const patientName = ref<string>();

const form = ref<any>();
const meds = ref<string>();
const diagnose = ref<string>();
const recomendations = ref<string>();

const doctorSpecialization = ref<number>();

onBeforeMount(async () => {
  // const patientId = router.currentRoute.value.params["id"] as string;
  let appointmentId = router.currentRoute.value.params["id"] as string;
  let res = await authorized.get(`Appointment/${appointmentId}`);
  const appointmentData = res.data;
  appointmentDate.value = appointmentData.date;
  patientName.value = appointmentData.patient.firstName + " " + appointmentData.patient.lastName;
  symptoms.value = appointmentData.symptoms;
  medicines.value = appointmentData.medicines;
  doctorSpecialization.value = appointmentData.specialization;
});
// const submit = async (data: SubmitEvent) => {
//   const valid = ((await data) as any).valid;
//   if (!valid) return;

// }
const select = ref<any>();
const picker = ref<any>();
const date = ref<any>(new Date());
const unavailable_hours = ref<Array<string>>([]);
const unavailable_dates = ref<any>();
const current_month = ref<number>(date.value.getMonth());
const hours_ready = ref<boolean>(false);
const days_ready = ref<boolean>(false);
// eslint-disable-next-line
defineExpose({
  appointmentDate,
  patientName,
  symptoms,
  medicines,
  meds,
  diagnose,
  recomendations,
  date,
  select,
});
const hours = [
  "09:00",
  "09:30",
  "10:00",
  "10:30",
  "11:00",
  "11:30",
  "12:00",
  "12:30",
  "13:00",
  "13:30",
  "14:00",
  "14:30",
  "15:00",
  "15:30",
  "16:00",
  "16:30",
];
// const getSpecialization = () => {
//   const specialization = specializations.find(
//     (elem: any) => elem.value === doctorSpecialization.value
//   );
//   if (specialization) return specialization.title;
//   else return "Błąd";
// };
const getUnavailableHours = async () => {
  try {
    hours_ready.value = false;
    let res = await authorized.get(
      `/appointment/available-hours/${
        date.value.toISOString().split("T")[0]
      }/specialization/${doctorSpecialization.value}` //specjalizacja tutaj
    );
    unavailable_hours.value = res.data;
  } catch (e) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd przy wczytywaniu dostępnych godzin";
    snackbar.showing = true;
  } finally {
      // if selected date is today
    if (
      date.value.getTime() - Date.now() < 86400000 &&
      date.value.getDate() === new Date().getDate()
    ) {
      const to_slice =
        (new Date().getHours() - 9) * 2 +
        5 +
        (new Date().getMinutes() >= 30 ? 1 : 0);
      unavailable_hours.value = unavailable_hours.value.concat(
        hours.slice(0, to_slice).map((hour) => hour + ":00")
      );
    }
    hours_ready.value = true;
  }
};

const getUnavailableDays = async (month: number) => {
  try {
    days_ready.value = false;
    let res = await authorized.get(
      `http://localhost:7042/api/appointment/specialization/${
        doctorSpecialization.value //specjalizacja tutal
      }/year/${date.value.getFullYear()}/month/${month + 1}`
    );
    unavailable_dates.value = res.data;
  } catch (e) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd przy wczytywaniu dostępnych godzin";
    snackbar.showing = true;
  } finally {
    days_ready.value = true;
  }
};
const available_hours = computed(() => {
  let total_hours = date.value
    ? date.value.getDay() == 6
      ? hours.slice(0, 10)
      : hours
    : null;
  return total_hours?.filter((hour) => {
    return !unavailable_hours.value.includes(hour + ":00");
  });
});

onMounted(async () => {
  getUnavailableHours();
  getUnavailableDays(new Date().getMonth() + 1);
});

watch(date, (newDate, oldDate) => {
  if (oldDate && newDate && oldDate.getDate() != newDate.getDate())
    getUnavailableHours();
  select.value = undefined;
});
watch(current_month, (newMonth, oldMonth) => {
  if (newMonth != oldMonth) getUnavailableDays(newMonth);
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
              {{ new Date(appointmentDate as string).toLocaleDateString() }},
              {{
                new Date(appointmentDate as string).toLocaleTimeString().substring(0, 5)
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
            <v-col class="text-left"> {{ patientName }} </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              Podane objawy
            </v-col>
            <v-col class="text-left"> {{ symptoms }} </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Przyjmowane leki
            </v-col>
            <v-col class="text-left"> {{ medicines }} </v-col>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-spacer></v-spacer>
        </v-container>
        <v-container>
          <v-row>
            <v-col cols="12" sm="6">
              <v-textarea
                v-model="diagnose"
                variant="filled"
                auto-grow
                label="Diagnoza"
                rows="5"
                row-height="20"
              ></v-textarea>
            </v-col>
            <v-col cols="12" sm="6">
              <v-textarea
              v-model="meds"
                variant="filled"
                auto-grow
                label="Leki"
                rows="5"
                row-height="20"
              ></v-textarea>
            </v-col>
          </v-row>
          <v-textarea label="Zalecenia" v-model="recomendations"></v-textarea>

          <v-row>
            <p class="font-weight-bold">Termin wizyty kontrolnej</p>
            <v-divider></v-divider>
          </v-row>
          <!-- <v-row>
            <v-col>
              <VueDatePicker auto-apply> 

              </VueDatePicker>
            </v-col>
            <v-col>
              <v-checkbox label=""></v-checkbox>
            </v-col>
          </v-row> -->
          <v-row no-gutters>
            <v-col cols="12" class="pa-4">
              <!-- 2592000000ms = 30 days -->
              <VueDatePicker
                v-model="date"
                locale="pl"
                :six-weeks="true"
                :enable-time-picker="false"
                :min-date="new Date()"
                :max-date="new Date(Date.now() + 2592000000)"
                no-today
                :hide-offset-dates="true"
                inline
                :highlight-disabled-days="true"
                :disabled-dates="unavailable_dates"
                auto-apply
                :disabled="!days_ready"
                ref="picker"
                :disabled-week-days="[0]"
                position="center"
                class="justify-center"
                calendar-class-name="justify-center"
                calendar-cell-class-name="big-cell text-body-1 rounded-lg"
                @update-month-year="current_month = $event.month"
              />
            </v-col>
            <v-col cols="12" class="my-4 d-flex justify-center">
              <v-form class="w-100" ref="form" validate-on="input">
                <v-select
                  variant="solo"
                  :label="
                    (available_hours as string[]).length !== 0
                      ? 'Wybierz godzinę'
                      : 'Dzień niedostępny'
                  "
                  :disabled="!hours_ready"
                  :items="available_hours"
                  no-data-text="Brak godzin w danym dniu"
                  v-model="select"
                  :rules="notNull"
                ></v-select>
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
