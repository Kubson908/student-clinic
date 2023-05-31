<script setup lang="ts">
import Datepicker from "@vuepic/vue-datepicker";
import { ref } from "vue";
import { computed } from "vue";
import { onBeforeMount } from "vue";
import { authorized, user, router } from "@/main";
import { prefix } from "@/config";

const checkRole = (role: string) => {
  const roles = user.roles!;
  return roles.includes(role);
};

// const date = ref(new Date());
// date.value.setHours(0, 0, 0, 0);
const date = ref(new Date());

interface IApppointment {
  id: number;
  finished: boolean;
  patient: string;
  date: Date;
}

var appointments: IApppointment[] = [];

onBeforeMount(async () => {
  if (checkRole("Staff")) {
    const res = await authorized.get(`${prefix}/api/Appointment/everySchedule`);
    appointments = res.data;
  } else if (checkRole("Employee")) {
    const res = await authorized.get(`${prefix}/api/Appointment/schedule`);
    appointments = res.data;
  }

  //console.log(appointments);
});

const format = (date: Date) => {
  const day = date.getDate();
  const month = date.getMonth() + 1;
  const year = date.getFullYear();
  

  return `${day}/${month}/${year}`;
};
const formatTime = (date: Date) => {
  const hours = date.getHours();
  const minutes = date.getMinutes();
  

  return `${hours}:${minutes}`;
};

const filteredAppointments = computed(() => {
  const d = date.value; //Don't question it
  //console.log(date.value.toDateString());
  return appointments.filter(
    (it) => new Date(it.date).toDateString() == d.toDateString() // TODO: filter appointments at startup
  );
});
</script>

<template>
  <v-row justify="center" no-gutters>
    <v-col xs="12" sm="6" md="6" align-self="center">
      <v-card elevation="5" class="rounded-lg" height="70vh">
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
                :format="format"
              />
            </v-col>
          </v-row>
        </v-card-item>

        <div class="card">
          <v-list class="d-flex flex-column justify-center align-center">
            <v-list-item
              elevation="3"
              class="rounded-lg my-2"
              v-for="appointment in filteredAppointments"
              :key="appointment.id"
              width="90%"
            >
              <v-row>
                <v-col xs="2" md="4">
                  <v-container class="d-flex  flex-column left">
                    
                    {{ formatTime(new Date(appointment.date)) }}
                    {{ appointment.patient }}
                    {{ format(new Date(appointment.date)) }}
                  </v-container>
                </v-col>
                <v-col xs="10" md="8">
                  <v-container class="right">
                    <router-link
                      to="/doctor/visit"
                      custom
                      v-slot="{ navigate }"
                    >
                      <v-btn
                        color="blue-darken-2"
                        class="mt-2 mx-2 button"
                        :disabled="appointment.finished"
                        @click="navigate"
                        >Rozpocznij</v-btn
                      >
                    </router-link>
                    <router-link
                      :to="'/doctor/appointments/' + appointment.id"
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
          </v-list>
        </div>

        <v-container class="d-flex justify-center align-center bottom">
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
.card {
  overflow: auto !important;
  max-height: 60%;
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

/* width */
.card::-webkit-scrollbar {
  width: 0px;
}
::-webkit-scrollbar {
  width: 0px;
}
.card::-webkit-scrollbar {
  width: 0px;
}

/* Track */
.card::-webkit-scrollbar-track {
  background: #f1f1f1;
  display: none;
}

/* Handle */
.background::-webkit-scrollbar-thumb {
  background: #888;
  display: none;
}

/* Handle on hover */
.background::-webkit-scrollbar-thumb:hover {
  background: #555;
}
</style>
