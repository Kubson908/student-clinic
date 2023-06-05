<script setup lang="ts">
import { authorized, snackbar, specializations, user } from "@/main";
import { ref, onBeforeMount, onBeforeUnmount, watch, computed } from "vue";
import { getSocket } from "@/socket";

const awaiting = ref<Array<any>>([]);
const socket = ref<WebSocket | null>(null);
const message = ref<any>(null);
let loaded = false;
const sorted = computed(() =>
  Array.from(awaiting.value).sort(
    (a: any, b: any) => new Date(a.date).getTime() - new Date(b.date).getTime()
  )
);
onBeforeMount(async () => {
  try {
    if (user.isLoggedIn) socket.value = getSocket(message);
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

onBeforeUnmount(() => {
  try {
    socket.value?.close();
    socket.value = null;
  } catch (e: any) {
    console.log(e);
  }
});

watch(message, (newMessage) => {
  const parsed = JSON.parse(newMessage.data);
  if (parsed["EventName"] === "newAppointment") {
    try {
      parsed.Data.date = parsed.Data.date.slice(0, -4);
      awaiting.value.push(parsed.Data);
      snackbar.error = false;
      snackbar.text = `Dodano nową wizytę [ID: ${parsed.Data.id}]`;
      snackbar.showing = true;
    } catch (error) {
      console.log(error);
      window.location.reload();
    }
  } else if (parsed["EventName"] === "assignedAppointment") {
    try {
      awaiting.value = awaiting.value.filter(
        (appointment: any) => appointment.id !== parsed.Data.id
      );
      snackbar.error = true;
      snackbar.text = `Wizyta o ID ${parsed.Data.id} została przypisana.`;
      snackbar.showing = true;
    } catch (error) {
      console.log(error);
    }
  } else if (parsed["EventName"] === "deletedAppointment") {
    try {
      awaiting.value = awaiting.value.filter(
        (appointment: any) => appointment.id !== parsed.Data.id
      );
      snackbar.error = true;
      snackbar.text = `Wizyta o ID ${parsed.Data.id} została anulowana.`;
      snackbar.showing = true;
    } catch (error) {
      console.log(error);
    }
  }
});
</script>
<template>
  <v-row justify="center" no-gutters>
    <v-col cols="12" sm="8" xl="6" xxl="4" align-self="center">
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
                <th class="text-center">Odrzuć</th>
              </tr>
            </thead>
            <tbody>
              <TransitionGroup name="list">
                <tr v-for="visit in sorted" :key="visit.id">
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
                        v-if="new Date(visit.date) > new Date()"
                        class="mx-2 button"
                        color="blue-darken-2"
                        size="small"
                        :value="'/staff/appointments/' + visit.id + '/assign'"
                        @click="navigate"
                      >
                        Wybór lekarza
                      </v-btn>
                      <v-btn
                        size="small"
                        color="grey-darken-2"
                        v-else
                        class="mx-2 button"
                        disabled
                      >
                        &nbsp;Termin minął&nbsp;
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
                        class="mx-2 button hp-bright"
                        size="small"
                        :value="'/staff/appointment/' + visit.id"
                        @click="navigate"
                      >
                        Szczegóły
                      </v-btn>
                    </router-link>
                  </td>
                  <td>
                    <router-link
                      :to="'/staff/appointments/' + visit.id + '/cancel'"
                    >
                      <v-btn
                        class="mx-2 button"
                        color="red-darken-2"
                        size="small"
                      >
                        Odrzuć
                      </v-btn>
                    </router-link>
                  </td>
                </tr>
                <tr v-if="awaiting.length === 0">
                  <td colspan="6">
                    <v-row no-gutters>
                      <v-col
                        align-self="center"
                        class="text-grey text-center pa-4"
                        >{{
                          loaded ? "Brak wizyt do przypisania" : "Wczytywanie"
                        }}</v-col
                      ></v-row
                    >
                  </td>
                </tr>
              </TransitionGroup>
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
<style>
.list-enter-active,
.list-leave-active {
  transition: all 0.5s ease;
}
.list-enter-from,
.list-leave-to {
  opacity: 0;
  transform: translateY(30px);
}
</style>
