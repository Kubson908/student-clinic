<script setup lang="ts">
import DatePicker from "@vuepic/vue-datepicker";
import { ref } from "vue";
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
const data: any = {
  labels: ["January", "February", "March"],
  datasets: [{ data: [40, 20, 12] }],
};
const options: any = {
  responsive: true,
};
const date = ref<any>({month: new Date().getMonth(), year: new Date().getFullYear()});
const picker = ref<any>(null);
const selected = ref<string>("Wizyty wg specjalności");
console.log(date);
</script>
<template>
  <v-row justify="center" no-gutters>
    <v-col cols="12" sm="6" md="6" align-self="center">
      <v-card elevation="5" class="rounded-lg">
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
                :items="['Wizyty wg specjalności', 'Wizyty wg lekarza']"
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
              <v-btn
                width="20%"
                variant="outlined"
                color="blue-darken-2"
                class="mt-2 button"
                >Wstecz</v-btn
              >
            </div>
          </v-container>
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>
<style></style>
