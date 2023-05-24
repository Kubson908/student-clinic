<script setup lang="ts">
import DatePicker from "@vuepic/vue-datepicker";
import { ref } from "vue";

const date = ref<any>();
const awaiting = [
  {
    id: 1,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 2,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 3,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 4,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 5,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 6,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 7,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 8,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 9,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 10,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 11,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
  {
    id: 12,
    doctor: "Internista",
    date: "17.03.2023",
    hour: "8:00",
  },
];
</script>
<template>
  <v-row justify="center" no-gutters>
    <v-col cols="12" sm="6" md="6" align-self="center">
      <v-card elevation="5" class="rounded-lg">
        <v-card-item>
          <v-container class="d-flex justify-center align-center">
            <h1>Oczekujące wizyty</h1>
          </v-container>
          <v-row no-gutters class="justify-center text-center"
            ><DatePicker
              auto-apply
              no-today
              v-model="date"
              range
              :teleport="true"
              :enable-time-picker="false"
              locale="pl"
              class="w-50 m-auto"
            ></DatePicker
          ></v-row>
        </v-card-item>
        <v-card-text>
          <v-table height="50vh" class="px-8" fixed-header>
            <thead>
              <tr>
                <th class="text-center">Data</th>
                <th class="text-center">Godzina</th>
                <th class="text-center">Specjalizacja</th>
                <th class="text-center">Wybór lekarza</th>
                <th class="text-center">Szczegóły</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="visit in awaiting" :key="visit.id">
                <td>
                  {{ visit.date }}
                </td>
                <td>
                  {{ visit.hour }}
                </td>
                <td>
                  {{ visit.doctor }}
                </td>
                <td>
                  <v-btn class="mt-2 mx-2 button hp-dark" size="small">
                    Wybór lekarza
                  </v-btn>
                </td>
                <td>
                  <v-btn
                    variant="text"
                    class="mt-2 mx-2 button hp-bright"
                    size="small"
                  >
                    Szczegóły
                  </v-btn>
                </td>
              </tr>
            </tbody>
          </v-table>
        </v-card-text>
        <v-card-actions>
          <v-container class="d-flex justify-center bottom pa-0 white">
            <div width="90%" class="space-between pa-4">
              <router-link to="/doctor/doctorpage" custom v-slot="{ navigate }">
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
