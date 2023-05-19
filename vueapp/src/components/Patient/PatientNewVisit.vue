<script setup lang="ts">
import VueDatePicker from "@vuepic/vue-datepicker";
import { ref } from "vue";

// In case of a range picker, you'll receive [Date, Date]
const format = (date) => {
  const day = date.getDate();
  const month = date.getMonth() + 1;
  const year = date.getFullYear();

  return `${day}/${month}/${year}`;
};

// const time = ref({
//   hours: new Date().getHours(),
//   minutes: new Date().getMinutes()
// });

// let minutes = new Date().getMinutes();
// let hours = new Date().getHours();
// let m = (parseInt((minutes + 7.5)/15) * 15) % 60;
// var h = minutes > 52 ? (hours === 23 ? 0 : ++hours) : hours;
const date = ref();
</script>

<template>
  <v-card location="center" elevation="5" class="rounded-lg w-50">
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
            <v-col>
              <p class="text-h6 text-left">Data</p>
            </v-col>
            <v-col>
              <p class="text-h6 text-left">Godzina</p>
            </v-col>
          </v-row>
          <v-row no-gutters>
            <v-col cols="12" sm="6">
              <VueDatePicker
                v-model="date"
                locale="pl"
                :six-weeks="true"
                :enable-time-picker="false"
                :format="format"
                :min-date="new Date()"
                inline
                auto-apply
                :disabled-week-days="[0]"
                position="center"
                calendar-class-name="justify-center"
                calendar-cell-class-name="big-cell text-body-1 rounded-lg"
              />
            </v-col>
            <v-col cols="12" sm="6">
              <!-- <VueDatePicker ref="minutesPicker" v-model="time" time-picker :min-time="{ hours: 8, minutes: 59 }" :max-time="{ hours: 16, minutes: 59 }" :start-time="{hours: 0, minutes: 0}" minutes-increment="15" minutes-grid-increment="16" inline auto-apply/> -->
              <v-select
                variant="solo"
                label="Wybierz godzinę"
                :items="['9:30','10:00', '10:30', '11:00','11:30','12:00','12:30','13:00','13:30', '16:00', '16:30']"
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
            >
              Dalej
            </v-btn>
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
  </v-card>
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
