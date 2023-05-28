<script setup lang="ts">
import VueDatePicker from "@vuepic/vue-datepicker";
import { ref, onMounted, computed, watch } from "vue";
import { authorized } from "@/main";
import { notNull } from "@/validation";

const hours = [
  "09:00",
  "09:30",
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
const date = ref<any>(new Date());
const unavailable_hours = ref<Array<string>>([]);
const unavailable_dates = ref<any>();
const current_month = ref<number>(date.value.getMonth());
const form = ref<any>();
// eslint-disable-next-line
defineExpose({
  date,
  select,
});

const getUnavailableHours = async () => {
  // let token = await localStorage.getItem("token");
  let res = await authorized.get(
    `/appointment/available-hours/${
      date.value.toISOString().split("T")[0]
    }/specialization/0`
  );
  unavailable_hours.value = res.data;
};

const getUnavailableDays = async (month: number) => {
  // let token = await localStorage.getItem("token");
  let res = await authorized.get(
    `http://localhost:7042/api/appointment/specialization/0/year/${date.value.getFullYear()}/month/${
      month + 1
    }`
  );
  unavailable_dates.value = res.data;
};

// eslint-disable-next-line
const emit = defineEmits(["page"]);
const change_page = async (arg: number) => {
  if (arg > 0) {
    const valid = ((await form.value.validate()) as any).valid;
    if (!valid) return;
  }
  emit("page", arg);
};

const available_hours = computed(() => {
  let total_hours = date.value
    ? date.value.getDay() == 6
      ? hours.slice(0, 10)
      : hours
    : null;
  return total_hours?.filter((hour) => {
    return !unavailable_hours.value.includes(hour + ":00");
  });
});

onMounted(async () => {
  // let token = await localStorage.getItem("token");
  let today = new Date();
  let res = await authorized.get(
    `http://localhost:7042/api/appointment/specialization/0/year/${today.getFullYear()}/month/${
      today.getMonth() + 1
    }`
  );
  unavailable_dates.value = res.data;
});

watch(date, (newDate, oldDate) => {
  console.log(newDate);
  if (oldDate && newDate && oldDate.getDate() != newDate.getDate())
    getUnavailableHours();
  select.value = undefined;
});
watch(current_month, (newMonth, oldMonth) => {
  if (newMonth != oldMonth) getUnavailableDays(newMonth);
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
                :hide-offset-dates="true"
                inline
                :highlight-disabled-days="true"
                :disabled-dates="unavailable_dates"
                auto-apply
                ref="picker"
                :disabled-week-days="[0]"
                position="center"
                class="justify-center"
                calendar-class-name="justify-center"
                calendar-cell-class-name="big-cell text-body-1 rounded-lg"
                @update-month-year="current_month = $event.month"
              />
            </v-col>
            <v-col cols="12" class="my-4 d-flex justify-center">
              <v-form class="w-100" ref="form" validate-on="input">
                <v-select
                  variant="solo"
                  label="Wybierz godzinę"
                  :items="available_hours"
                  v-model="select"
                  :rules="notNull"
                ></v-select>
              </v-form>
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
