<script setup lang="ts">
import { ref } from "vue";
import DoctorVisitProgress from "./DoctorVisitProgress.vue";
import DoctorVisitSummary from "./DoctorVisitSummary.vue";
import DoctorVisitControl from "./DoctorVisitControl.vue";
import { router, snackbar, authorized } from "@/main";
const loading = ref<boolean>(true);

const progress = ref<any>();
const control = ref<any>();
let appointment_id = router.currentRoute.value.params["id"] as string;

const getData = () => {
  return {
    medicines: progress.value.medicines,
    symptoms: progress.value.symptoms,
    diagnose: progress.value.diagnose,
    patientName: progress.value.patientName,
    recomendations: progress.value.recomendations,
    appointmentDate: progress.value.appointmentDate,
    meds: progress.value.meds,
    date: progress.value.date,
    select: progress.value.select,
    specialization: progress.value.doctorSpecialization,
    controlVisit: progress.value.control_visit,
    dateDay: control.value ? control.value.date : undefined,
    dateHour: control.value ? control.value.hour : undefined,
  };
};
const onSubmit = async () => {
  try {
    loading.value = true;
    const req_data = getData();
    const res = await authorized.patch(`Appointment/finish/${appointment_id}`, {
      finished: true,
      diagnosis: req_data.diagnose,
      recommmendations: req_data.recomendations,
      medicines: req_data.meds,
      date:
        req_data.dateDay && req_data.dateHour
          ? `${("0" + req_data.dateDay.toLocaleDateString())
              .slice(-10)
              .split(".")
              .reverse()
              .join("-")}T${req_data.dateHour}:00.000Z`
          : null,
    });
    if (res.status === 200) {
      snackbar.text = "Pomyślnie ukończono wizytę";
      snackbar.error = false;
    }
  } catch (e: any) {
    console.log(e);
    if (e.response && e.response.status === 406) {
      snackbar.text = "Niedostępny termin wizyty kontrolnej"
    } else {
      snackbar.text = "Wystąpił błąd przy kończeniu wizyty";
    }
    snackbar.error = true;
  } finally {
    loading.value = false;
    snackbar.showing = true;
  }
};
const page = ref<number>(1);
</script>
<template>
  <v-card
    width="560px"
    location="center"
    elevation="5"
    class="rounded-lg mt-10 mb-10"
  >
    <template #loader>
      <v-progress-linear
        :active="loading"
        color="deep-purple"
        height="4"
        indeterminate
      ></v-progress-linear>
    </template>
    <v-window v-model="page" direction="vertical" reverse :touch="false">
      <v-window-item :value="1">
        <DoctorVisitProgress
          @page="(arg) => (page += arg)"
          ref="progress"
          @loaded="loading = false"
        />
      </v-window-item>
      <v-window-item :value="2">
        <DoctorVisitControl
          @page="(arg) => (page += arg)"
          ref="control"
          :specialization="progress.doctorSpecialization"
        />
      </v-window-item>
      <v-window-item :value="3">
        <DoctorVisitSummary
          @page="(arg) => (page += arg)"
          :data="getData()"
          @submit="onSubmit()"
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
