<script lang="ts" setup>
import { ref } from "vue";
import { specializations, router } from "@/main";
import { notNull } from "../../validation";
// eslint-disable-next-line
const emit = defineEmits(["page"]);
const change_page = async (arg: number) => {
  if (arg > 0) {
    const valid = ((await form.value.validate()) as any).valid;
    if (!valid) return;
  }
  emit("page", arg);
};
const symptoms = ref<string>("");
const medicine = ref<string>("");
const specialization = ref<number>();
const form = ref<any>();
// eslint-disable-next-line
defineExpose({
  symptoms,
  medicine,
  specialization,
});
</script>

<template>
  <v-card>
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
      <v-card-title font-size="56">Nowa wizyta</v-card-title>
      <v-card-subtitle>Wpisz szczegóły</v-card-subtitle>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent validate-on="input" ref="form">
        <v-select
          label="Wybierz specjalistów"
          :items="specializations"
          item-title="title"
          item-value="value"
          variant="solo"
          :rules="notNull"
          v-model="specialization"
        ></v-select>
        <v-textarea
          label="Objawy"
          variant="solo"
          v-model="symptoms"
        ></v-textarea>
        <v-textarea
          label="Stosowane leki"
          variant="solo"
          v-model="medicine"
        ></v-textarea>
        <v-row justify="center">
          <v-col xs="12" sm="6" md="3" align-self="center" class="text-left">
            <v-btn
              variant="outlined"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
              @click="router.push('/patient/appointments')"
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
</template>

<style>
.button {
  text-transform: unset !important;
}
</style>
