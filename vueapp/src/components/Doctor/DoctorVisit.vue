<script setup lang="ts">
import { ref } from "vue";
import DoctorVisitProgress from "./DoctorVisitProgress.vue";
import DoctorVisitSummary from "./DoctorVisitSummary.vue";
import DoctorVisitControl from "./DoctorVisitControl.vue";
import { router } from "@/main";
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
    dateDay: control.value.date,
    dateHour: control.value.hour,
  };
};
const onSubmit = () => {};
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
          :specialization="getData().specialization"
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
