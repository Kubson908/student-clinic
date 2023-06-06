<script setup lang="ts">
import Datepicker from "@vuepic/vue-datepicker";
import { ref } from "vue";
import { watch, onBeforeMount, onBeforeUnmount, computed } from "vue";
import { authorized, router, snackbar, user } from "@/main";
import { getSocket } from "@/socket";

const date = ref(new Date());

interface IApppointment {
  id: number;
  finished: boolean;
  patient: string;
  date: Date;
}

const appointments = ref<IApppointment[]>([]);
const loading = ref<boolean>(true);
const socket = ref<WebSocket | null>(null);
const message = ref<any>(null);

const fetchData = async () => {
  const id = router.currentRoute.value.params["id"] ?? null;
  if (id !== null) {
    try {
      loading.value = true;
      const res = await authorized.get(
        `/Appointment/doctor/${id}/schedule/${date.value.getFullYear()}/${
          date.value.getMonth() + 1
        }/${date.value.getDate()}`
      );
      appointments.value = res.data;
    } catch (e) {
      console.log(e);
      snackbar.error = true;
      snackbar.text =
        "Wystąpił błąd przy sprawdzaniu harmonogramu na dany dzień";
      snackbar.showing = true;
    } finally {
      loading.value = false;
    }
  } else {
    try {
      loading.value = true;
      const res = await authorized.get(
        `/Appointment/schedule/${date.value.getFullYear()}/${
          date.value.getMonth() + 1
        }/${date.value.getDate()}`
      );
      appointments.value = res.data;
    } catch (e) {
      console.log(e);
      snackbar.error = true;
      snackbar.text =
        "Wystąpił błąd przy sprawdzaniu harmonogramu na dany dzień";
      snackbar.showing = true;
    } finally {
      loading.value = false;
    }
  }
};

const sorted = computed(() => {
  return Array.from(appointments.value).sort(
    (a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()
  );
});

/*const format = (date: Date) => {
  const day = date.getDate();
  const month = date.getMonth() + 1;
  const year = date.getFullYear();

  return `${day}/${month}/${year}`;
};*/
const formatTime = (date: Date) => {
  const hours = date.getHours();
  const minutes = date.getMinutes();
  if (minutes == 0) {
    return `${hours}:00`;
  }
  return `${hours}:${minutes}`;
};

onBeforeMount(async () => {
  try {
    if (user.isLoggedIn) socket.value = getSocket(message);
  } catch (e) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Błąd pobierania danych";
    snackbar.showing = true;
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

watch(
  date,
  () => {
    fetchData();
  },
  { immediate: true }
);

watch(message, (newMessage) => {
  const parsed = JSON.parse(newMessage.data);
  if (parsed["EventName"] === "assignedAppointment") {
    try {
      const a_date = new Date(parsed.Data.date.slice(0, -4) + "Z");
      if (
        !(
          a_date.getDate() === date.value.getDate() &&
          a_date.getMonth() === date.value.getMonth() &&
          a_date.getFullYear() === date.value.getFullYear()
        )
      )
        return;
      parsed.Data.date = a_date.toISOString();
      parsed.Data.patient = `${parsed.Data.patient.firstName} ${parsed.Data.patient.lastName}`;
      appointments.value.push(parsed.Data);
      snackbar.error = false;
      snackbar.text = `Przypisano nową wizytę [ID: ${parsed.Data.id}]`;
      snackbar.showing = true;
    } catch (error) {
      console.log(error);
      window.location.reload();
    }
  } else if (parsed["EventName"] === "deletedAppointment") {
    try {
      appointments.value = appointments.value.filter(
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
    <v-col xs="12" sm="6" md="6" align-self="center">
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
            <h1>Harmonogram wizyt</h1>
          </v-container>
          <v-row no-gutters justify="center">
            <v-col cols="12" sm="6" md="6">
              <Datepicker
                v-model="date"
                :teleport="true"
                :enable-time-picker="false"
                auto-apply
                locale="pl-PL"
                :disabled="loading"
                calendar-cell-class-name="big-cell text-body-1 rounded-lg"
                :disabled-week-days="[0]"
              >
                <template #trigger>
                  <v-text-field
                    class="pa-0"
                    variant="solo"
                    type="text"
                    readonly
                    append-inner-icon="mdi-calendar-month"
                    label="Miesiąc i rok"
                    :disabled="loading"
                    :model-value="
                      date ? `${date.toLocaleDateString('pl-PL')}` : 'Brak'
                    "
                  ></v-text-field></template
              ></Datepicker>
            </v-col>
          </v-row>
        </v-card-item>
        <v-divider class="mx-4"> </v-divider>
        <div class="visit-card mx-4">
          <v-list class="d-flex flex-column justify-center align-center">
            <v-row no-gutters v-if="appointments.length === 0">
              <v-col class="text-grey"> Brak wizyt w tym dniu </v-col>
            </v-row>
            <TransitionGroup name="list">
              <v-list-item
                elevation="3"
                class="rounded-lg my-2"
                v-for="appointment in sorted"
                :key="appointment.id"
                width="90%"
              >
                <v-row>
                  <v-col xs="2" md="4">
                      <v-container class="d-flex flex-column left">
                          {{ formatTime(new Date(appointment.date)) }}
                          {{ appointment.patient }}

                      </v-container>
                  </v-col>
                  <v-col xs="10" md="8">
                    <v-container class="right">
                      <router-link
                        :to="
                          '/doctor/appointment/' + appointment.id + '/finish'
                        "
                        custom
                        v-slot="{ navigate }"
                      >
                        <v-btn
                          color="blue-darken-2"
                          class="mt-2 mx-2 button"
                          :disabled="
                            appointment.finished ||
                            !(
                              new Date(appointment.date).getDate() ===
                                new Date().getDate() &&
                              new Date(appointment.date).getMonth() ===
                                new Date().getMonth() &&
                              new Date(appointment.date).getFullYear() ===
                                new Date().getFullYear()
                            )
                          "
                          @click="navigate"
                          >Rozpocznij</v-btn
                        >
                      </router-link>
                      <router-link
                        :to="'/doctor/appointment/' + appointment.id"
                        custom
                        v-slot="{ navigate }"
                      >
                        <v-btn
                          color="blue-darken-2"
                          class="mt-2 mx-2 button"
                          variant="text"
                          @click="navigate"
                        >
                          <u>Szczegóły</u>
                        </v-btn>
                      </router-link>
                    </v-container>
                  </v-col>
                </v-row>
              </v-list-item>
            </TransitionGroup>
          </v-list>
        </div>
        <v-divider class="mx-4"></v-divider>
        <v-container class="d-flex justify-center align-center">
          <div width="90%" class="space-between">
            <v-btn
              width="20%"
              variant="outlined"
              color="blue-darken-2"
              class="mt-2 button"
              @click="router.back()"
              >Wstecz</v-btn
            >
          </div>
        </v-container>
      </v-card>
    </v-col>
  </v-row>
</template>

<style>
.visit-card {
  overflow: auto !important;
  height: 50vh;
}
.button {
  max-width: fit-content !important;
  text-transform: unset !important;
}
.space-between {
  display: flex;
  justify-content: space-between;
  width: 90%;
  padding: 0;
}
.left {
  text-align: left;
  margin-left: 15% !important;
}
.right {
  text-align: right;
  margin-right: 10% !important;
}
.bottom {
  position: absolute;
  bottom: 0;
}
</style>
