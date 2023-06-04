<script setup lang="ts">
import { authorized, snackbar, specializations } from "@/main";
import { ref, onBeforeMount } from "vue";

const awaiting = ref<Array<any>>([]);
let loaded = false;
onBeforeMount(async () => {
  try {
    const res = await authorized.get("/appointment/awaiting-appointments");
    awaiting.value = res.data;
  } catch (e) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Błąd pobierania danych";
    snackbar.showing = true;
  } finally {
    //snackbar.showing = true;
    loaded = true;
  }
});
</script>
<template>
  <v-row justify="center" no-gutters>
    <v-col cols="12" sm="6" md="6" align-self="center">
      <v-card elevation="5" class="rounded-lg">
        <v-card-item>
          <v-container class="d-flex justify-center align-center">
            <h1>Oczekujące wizyty</h1>
          </v-container>
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
                  {{ new Date(visit.date).toLocaleDateString("pl-PL") }}
                </td>
                <td>
                  {{
                    new Date(visit.date)
                      .toLocaleTimeString("pl-PL")
                      .substring(0, 5)
                  }}
                </td>
                <td>
                  {{
                    specializations.find(
                      (s) => s.value === visit.specialization
                    )?.title
                  }}
                </td>
                <td>
                  <router-link
                    :to="'/staff/appointments/' + visit.id + '/assign'"
                    custom
                    v-slot="{ navigate }"
                  >
                    <v-btn
                      class="mt-2 mx-2 button"
                      color="blue-darken-2"
                      size="small"
                      :value="'/staff/appointments/' + visit.id + '/assign'"
                      @click="navigate"
                    >
                      Wybór lekarza
                    </v-btn>
                  </router-link>
                </td>
                <td>
                  <router-link
                    :to="'/staff/appointments/' + visit.id"
                    custom
                    v-slot="{ navigate }"
                  >
                    <v-btn
                      variant="text"
                      class="mt-2 mx-2 button hp-bright"
                      size="small"
                      :value="'/staff/appointment/' + visit.id"
                      @click="navigate"
                    >
                      Szczegóły
                    </v-btn>
                  </router-link>
                </td>
              </tr>
              <tr v-if="awaiting.length === 0">
                <td colspan="5">
                  <v-row no-gutters>
                    <v-col
                      align-self="center"
                      class="text-grey text-center pa-4"
                      >{{ loaded ? "Brak wizyt do przypisania" : "Wczytywanie" }}</v-col
                    ></v-row
                  >
                </td>
              </tr>
            </tbody>
          </v-table>
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
