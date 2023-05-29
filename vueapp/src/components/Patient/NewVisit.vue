<script lang="ts" setup>
import { ref } from "vue";
import { authorized, specializations, snackbar } from "../../main";
import NewVisitSymptoms from "./NewVisitSymptoms.vue";
import NewVisitDate from "./NewVisitDate.vue";
import NewVisitSummary from "./NewVisitSummary.vue";

const page = ref<number>(1);
const symptoms = ref<any>();
const date = ref<any>();
const waiting = ref<boolean>(false);

const getSpecialization = () => {
  const specialization = specializations.find(
    (elem: any) => elem.value === symptoms.value.specialization
  );
  if (specialization) return specialization.title;
  else return "Błąd";
};
const getData = () => {
  return {
    symptoms: symptoms.value.symptoms,
    date: `${("0" + new Date(date.value.date).toLocaleDateString()).slice(-11)}`,
    hour: date.value.select,
    specialization: getSpecialization(),
    medicine: symptoms.value.medicine,
  };
};

const submit = async () => {
  const data = getData();
  const datestring = `${data.date.split(".").reverse().join("-")}T${
    data.hour.split(":")[0]
  }:${data.hour.split(":")[1]}:00.000Z`;
  try {
    waiting.value = true;
    const response = await authorized.post("/Appointment/create", {
      date: datestring,
      specialization: symptoms.value.specialization,
      symptoms: data.symptoms,
      medicines: data.medicine,
    });
    if (response.status === 201) {
      snackbar.error = false;
      snackbar.text = "Pomyślnie zarezerowano wizytę";
    }
  } catch (e: any) {
    console.log(e);
    snackbar.error = true;
    snackbar.text =
      e.response && e.response.status === 406
        ? "Nie można zarezerwować wizyty - termin zajęty"
        : e.response && e.response.status === 409
        ? "Zarezerwowałeś już wizytę w tym terminie"
        : "Wystąpił nieznany błąd";
  } finally {
    waiting.value = false;
    snackbar.showing = true;
  }
};
</script>

<template>
  <v-card
    width="560px"
    location="center"
    elevation="5"
    class="rounded-lg mt-10"
  >
    <template #loader>
      <v-progress-linear
        :active="waiting"
        color="deep-purple"
        height="4"
        indeterminate
      ></v-progress-linear>
    </template>
    <v-window v-model="page" direction="vertical" reverse :touch="false">
      <v-window-item :value="1">
        <NewVisitSymptoms @page="(arg) => (page += arg)" ref="symptoms" />
      </v-window-item>

      <v-window-item :value="2">
        <NewVisitDate @page="(arg) => (page += arg)" ref="date" />
      </v-window-item>
      <v-window-item :value="3">
        <NewVisitSummary
          @page="(arg) => (page += arg)"
          :data="getData()"
          @submit="submit"
        />
      </v-window-item>
    </v-window>
  </v-card>
</template>

<style>
.button {
  text-transform: unset !important;
}
</style>
