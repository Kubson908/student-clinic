<script setup lang="ts">
import DatePicker from "@vuepic/vue-datepicker";
import { reactive, ref, onBeforeMount, watch } from "vue";
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

const data = reactive({
  labels: specialization_labels,
  datasets: [{ data: [] }],
});
const options: any = {
  responsive: true,
};
const date = reactive({
  month: new Date().getMonth(),
  year: new Date().getFullYear(),
});
const picker = ref<any>(null);
const selected = ref<number>(0);
const loading = ref<boolean>(true);
const appointments = ref<Array<any>>([]);

const appointmentsBySpecialization = () => {
  const result: any[] = [];
  for (const specialization of specializations ) {
    result.push(() => {
      return appointments.value.reduce((acc: number, curr: number) => {

      })
    })
  }
  return result;
};

const appointmentsByDoctor = () => {};

const statistics_options: Array<{
  id: number;
  title: string;
  function: CallableFunction;
}> = [
  {
    id: 0,
    title: "Wizyty wg specjalności",
    function: appointmentsBySpecialization,
  },
  {
    id: 1,
    title: "Wizyty wg lekarza",
    function: appointmentsByDoctor,
  },
];

const renderStatistics = () => {
  statistics_options[selected.value].function();
};

const fetchData = async () => {
  try {
    loading.value = true;
    const res = await authorized.get(
      `/Appointment/statistics/${date.year}/${date.month+1}`
    );
    console.log(res.data);
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
  (newData, oldData) => {
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
            :active="loading"
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
              >
                <template #trigger>
                  <v-text-field
                    class="pa-0"
                    variant="solo"
                    type="text"
                    readonly
                    append-inner-icon="mdi-calendar-month"
                    label="Miesiąc i rok"
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
