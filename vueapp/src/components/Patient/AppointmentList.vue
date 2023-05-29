<script setup lang="ts">
import { authorized, specializations, router } from "@/main";
import { onBeforeMount, reactive, computed } from "vue";
import { Appointments } from "../../typings";

let test = reactive<Appointments>({
  appointments: [],
});
onBeforeMount(async () => {
  const res = await authorized.get("/Appointment/patient");
  test.appointments = res.data.appointments[0];
});
let getDateFromString = (string: any) => {
  let date = new Date(string);
  return `${date.toLocaleDateString("pl-PL")} ${date.toLocaleTimeString(
    "pl-PL"
  )}`;
};

const disable = (dateString: string) => {
  let pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
  let date = new Date(dateString.replace(pattern, "$3-$2-$1"));
  let today = new Date();
  if (today > date) return true;
  return false;
};
const sortedByDate = computed(() => {
  return Array.from(test.appointments).sort(
    (a: any, b: any) => new Date(b.date).getTime() - new Date(a.date).getTime()
  );
});
</script>

<template>
  <v-row justify="center" no-gutters>
    <v-col xs="12" sm="6" md="6" align-self="center">
      <v-card elevation="5" class="rounded-lg" height="70vh">
        <v-card-item>
          <v-container class="d-flex justify-center align-center">
            <h1>Lista wizyt</h1>
          </v-container>
        </v-card-item>
        <div class="card">
          <v-list class="d-flex flex-column justify-center align-center">
            <v-list-item
              elevation="3"
              class="rounded-lg my-2"
              v-for="(appointment, idx) in sortedByDate"
              :key="idx"
              width="90%"
            >
              <v-fade-transition>
                <v-row>
                  <v-col xs="2" md="4">
                    <v-container class="d-flex flex-1-0 flex-column left">
                      <strong>{{
                        specializations.find(
                          (specialization) =>
                            appointment.specialization === specialization.value
                        )?.title
                      }}</strong>
                      {{ getDateFromString(appointment.date) }}
                    </v-container>
                  </v-col>
                  <v-col xs="10" md="8">
                    <v-container class="right">
                      <router-link :to="'/patient/appointments/' + appointment.id">
                        <v-btn color="blue-darken-2" class="mt-2 mx-2 button"
                          >Szczegóły</v-btn
                        >
                      </router-link>
                      <router-link
                        v-if="!disable(appointment.date)"
                        :to="
                          '/patient/appointments/' + appointment.id + '/cancel'
                        "
                      >
                        <v-btn
                          variant="outlined"
                          color="red-darken-2"
                          class="mt-2 mx-2 button"
                        >
                          Anuluj wizytę
                        </v-btn>
                      </router-link>
                      <v-btn
                        v-else
                        disabled
                        variant="outlined"
                        color="red-darken-2"
                        class="mt-2 mx-2 button"
                      >
                        Anuluj wizytę
                      </v-btn>
                    </v-container>
                  </v-col>
                </v-row>
              </v-fade-transition>
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
              @click="router.push('/')"
              >Wstecz</v-btn
            >
            <router-link to="/patient/new_visit">
              <v-btn color="blue-darken-2" class="mt-2 button"
                >Nowa wizyta</v-btn
              >
            </router-link>
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
