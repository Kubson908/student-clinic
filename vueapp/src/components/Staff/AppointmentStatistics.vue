<script setup lang="ts">
import DatePicker from "@vuepic/vue-datepicker";
import { ref, computed, onBeforeMount, watch } from "vue";
import { specializations, authorized, snackbar } from "@/main";
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
} from "chart.js";

import { Bar } from "vue-chartjs";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

const specialization_labels: Array<string> = specializations.map(
  (specialization) => specialization.title
);

const doctors = ref<Array<any>>([]);

const surnames = computed(() => doctors.value.map((doctor) => doctor.name));

const backgroundColors: Array<string> = [
  "rgba(255, 99, 132, 0.2)",
  "rgba(255, 159, 64, 0.2)",
  "rgba(255, 205, 86, 0.2)",
  "rgba(75, 192, 192, 0.2)",
  "rgba(54, 162, 235, 0.2)",
  "rgba(153, 102, 255, 0.2)",
  "rgba(201, 203, 207, 0.2)",
];

const borderColors: Array<string> = [
  "rgb(255, 99, 132)",
  "rgb(255, 159, 64)",
  "rgb(255, 205, 86)",
  "rgb(75, 192, 192)",
  "rgb(54, 162, 235)",
  "rgb(153, 102, 255)",
  "rgb(201, 203, 207)",
];

const data = ref({
  labels: specialization_labels,
  datasets: [
    {
      label: "",
      data: [],
      backgroundColor: backgroundColors,
      borderColor: borderColors,
      borderWidth: 1,
    },
  ],
});
const options: any = {
  responsive: true,
};
const date = ref({
  month: new Date().getMonth(),
  year: new Date().getFullYear(),
});
const picker = ref<any>(null);
const selected = ref<number>(0);
const loading = ref<boolean>(true);
const appointments = ref<Array<any>>([]);
const doctors_loaded = ref<boolean>(false);

const appointmentsBySpecialization = () => {
  const result: any[] = [];
  if (appointments.value.length > 0) {
    for (const specialization of specializations) {
      result.push(
        appointments.value.reduce((acc: number, curr: any) => {
          return curr.specialization === specialization.value ? acc + 1 : acc;
        }, 0)
      );
    }
  }
  return result;
};

const appointmentsByDoctor = () => {
  const result: any[] = [];
  if (appointments.value.length > 0) {
    for (const doctor of doctors.value) {
      result.push(
        appointments.value.reduce((acc: number, curr: any) => {
          return curr.doctorId === doctor.id ? acc + 1 : acc;
        }, 0)
      );
    }
  }
  return result;
};

const statistics_options: Array<{
  id: number;
  title: string;
  function: CallableFunction;
  labels: Array<string>;
}> = [
  {
    id: 0,
    title: "Ilość wizyt wg specjalności",
    function: appointmentsBySpecialization,
    labels: specialization_labels,
  },
  {
    id: 1,
    title: "Ilość wizyt wg lekarza",
    function: appointmentsByDoctor,
    labels: surnames as any,
  },
];

const renderStatistics = () => {
  data.value = {
    labels: statistics_options[selected.value].labels,
    datasets: [
      {
        label: statistics_options[selected.value].title,
        data: statistics_options[selected.value].function(),
        backgroundColor: backgroundColors,
        borderColor: borderColors,
        borderWidth: 1,
      },
    ],
  };
};

const fetchData = async () => {
  try {
    loading.value = true;
    const res = await authorized.get(
      `/Appointment/statistics/${date.value.year}/${date.value.month + 1}`
    );
    if (res.status === 204) {
      snackbar.error = false;
      snackbar.text = "Brak danych z tego miesiąca";
      snackbar.showing = true;
    }
    appointments.value = res.data;
  } catch (e: any) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd przy pobieraniu danych statystyki";
    snackbar.showing = true;
  } finally {
    loading.value = false;
  }
};

const fetchDoctors = async () => {
  try {
    const res = await authorized.get("/Employee");
    const result = res.data
      .filter((emp: any) => emp.specialization !== null)
      .map((doctor: any) => {
        return {
          name: doctor.firstName + " " + doctor.lastName,
          id: doctor.id,
        };
      });
    result.push({ name: "Nieprzypisane", id: null });
    doctors.value = result;
  } catch (e: any) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd przy wczytywaniu lekarzy";
    snackbar.showing = true;
  } finally {
    doctors_loaded.value = true;
  }
};

onBeforeMount(() => {
  fetchDoctors();
});
watch(
  date,
  (newDate, oldDate: any) => {
    if (!oldDate || newDate !== oldDate || newDate !== oldDate) {
      fetchData();
    }
  },
  { immediate: true, deep: true }
);
watch(
  [appointments, selected],
  () => {
    renderStatistics();
  },
  { deep: true }
);
</script>
<template>
  <v-row justify="center" no-gutters>
    <v-col cols="12" sm="6" md="6" align-self="center">
      <v-card elevation="5" class="rounded-lg">
        <template #loader>
          <v-progress-linear
            :active="loading || !doctors_loaded"
            color="deep-purple"
            height="4"
            indeterminate
          ></v-progress-linear>
        </template>
        <v-card-item>
          <v-container class="d-flex justify-center align-center">
            <h1>Statystyka</h1>
          </v-container>
          <v-row no-gutters class="justify-center text-center">
            <v-col cols="12" sm="6">
              <v-select
                label="Wybierz wykres"
                v-model="selected"
                class="px-8"
                variant="solo"
                :items="statistics_options"
                item-title="title"
                item-value="id"
                :disabled="!doctors_loaded || loading"
              >
              </v-select>
            </v-col>
            <v-col cols="12" sm="6">
              <DatePicker
                auto-apply
                month-picker
                no-today
                v-model="date"
                :teleport="true"
                :enable-time-picker="false"
                locale="pl"
                class="px-8"
                :clearable="false"
                ref="picker"
                :disabled="!doctors_loaded || loading"
              >
                <template #trigger>
                  <v-text-field
                    class="pa-0"
                    variant="solo"
                    type="text"
                    readonly
                    append-inner-icon="mdi-calendar-month"
                    label="Miesiąc i rok"
                    :disabled="!doctors_loaded || loading"
                    :model-value="
                      date
                        ? `${
                            date.month < 9
                              ? `0${date.month + 1}`
                              : date.month + 1
                          }.${date.year}`
                        : 'Brak'
                    "
                  ></v-text-field>
                </template>
              </DatePicker>
            </v-col>
          </v-row>
        </v-card-item>
        <v-card-text>
          <Bar :data="data" :options="options" />
        </v-card-text>
        <v-card-actions>
          <v-container class="d-flex justify-center bottom pa-0 white">
            <div width="90%" class="space-between pa-4">
              <router-link to="/staff/doctors" custom v-slot="{ navigate }">
                <v-btn
                  width="20%"
                  variant="outlined"
                  color="blue-darken-2"
                  class="mt-2 button"
                  @click="navigate"
                  >Wstecz</v-btn
                >
              </router-link>
            </div>
          </v-container>
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>
<style></style>
