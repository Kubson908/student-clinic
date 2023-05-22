<script setup lang="ts">
import VueDatePicker from "@vuepic/vue-datepicker";
import { ref, onMounted, computed, watch } from "vue";
import axios from "axios";

//Do poprawy:
//przełączanie miesiąca nie zawsze prawidłowo odświeża listę dostępnych dni

const hours = [
  "9:00",
  "9:30",
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

const select = ref<any>();
const picker = ref<any>();
const date = ref<any>();
const unavailable_hours = ref<Array<string>>([]);
const unavailable_dates = ref<any>();

const getUnavailableHours = async () => {
  let token = await localStorage.getItem("token");
  let res = await axios.get(
    `http://localhost:7042/api/appointment/available-hours/${
      date.value.toISOString().split("T")[0]
    }/specialization/0`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );
  unavailable_hours.value = res.data;
};

const getUnavailableDays = async (event: Event) => {
  console.log(event);
  let token = await localStorage.getItem("token");
  let res = await axios.get(
    `http://localhost:7042/api/appointment/specialization/0/year/${date.value.getFullYear()}/month/${
      date.value.getMonth() + 1
    }`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );
  console.log(res.data);
  unavailable_dates.value = res.data;
};

// eslint-disable-next-line
const emit = defineEmits(["page"]);
const change_page = (arg: number) => {
  emit("page", arg);
};

const available_hours = computed(() => {
  let total_hours = date.value
    ? date.value.getDay() == 6
      ? hours.slice(0, 10)
      : hours
    : null;
  console.log(total_hours, unavailable_hours.value);
  return total_hours?.filter((hour) => {
    return !unavailable_hours.value.includes(hour + ":00")
  });
});

onMounted(async () => {
  let token = await localStorage.getItem("token");
  let today = new Date();
  let res = await axios.get(
    `http://localhost:7042/api/appointment/specialization/0/year/${today.getFullYear()}/month/${
      today.getMonth() + 1
    }`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );
  unavailable_dates.value = res.data;
});

watch(date, (newDate, oldDate) => {
  if (oldDate && oldDate.getDate() != newDate.getDate()) getUnavailableHours();
  select.value = undefined;
});
</script>

<template>
  <!-- <v-card location="center" elevation="5" class="rounded-lg w-50 my-16"> -->
  <v-card justify-center>
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
      <v-card-title class="font-weight-bold text-h5" font-size="56"
        >Nowa wizyta</v-card-title
      >
      <v-card-subtitle>Wybierz datę i godzinę</v-card-subtitle>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent>
        <v-container>
          <v-row no-gutters>
            <v-col cols="12" class="pa-4">
              <VueDatePicker
                v-model="date"
                locale="pl"
                :six-weeks="true"
                :enable-time-picker="false"
                :min-date="new Date()"
                no-today
                inline
                :disabled-dates="unavailable_dates"
                auto-apply
                ref="picker"
                :disabled-week-days="[0]"
                position="center"
                class="justify-center"
                calendar-class-name="justify-center"
                calendar-cell-class-name="big-cell text-body-1 rounded-lg"
                @update-month-year="getUnavailableDays($event)"
              />
            </v-col>
            <v-col cols="12" class="my-4 d-flex justify-center">
              <!-- <VueDatePicker ref="minutesPicker" v-model="time" time-picker :min-time="{ hours: 8, minutes: 59 }" :max-time="{ hours: 16, minutes: 59 }" :start-time="{hours: 0, minutes: 0}" minutes-increment="15" minutes-grid-increment="16" inline auto-apply/> -->
              <v-select
                class="w-75"
                variant="solo"
                label="Wybierz godzinę"
                :items="available_hours"
                v-model="select"
              ></v-select>
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
              @click="change_page(-1)"
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
              Dalej
            </v-btn>
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
  </v-card>

  <!-- </v-card> -->
</template>

<style>
.button {
  text-transform: unset !important;
}
.big-cell {
  width: 64px;
  height: 48px;
}
</style>
