<script lang="ts" setup>
import { ref } from "vue";
import { authorized, snackbar, router } from "@/main";
import { passwordRules } from "@/validation";
const form = ref<any>();
const oldPass = ref<string>("");
const newPass = ref<string>("");
const newPassRepeat = ref<string>("");
const loading = ref<boolean>(false);
const submit = async (data: SubmitEvent) => {
  const valid = ((await data) as any).valid;
  if (!valid) return;

  form.value?.reset();
};
const repeatPasswordRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (value === newPassRepeat.value) return true;
    else return "Hasła muszą się zgadzać";
  },
];
const submitData = async () => {
  const valid = await form.value.validate();
  if (!valid) return;
  // sprawdzic czy stare hasło jest poprawne !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  // if (oldPass.value != doctorPass.value){
  //   snackbar.error = true;
  //   snackbar.text = "Podałeś złe stare hasło";
  // }
  try {
    loading.value = true;
    const res = await authorized.patch(
      `http://localhost:7042/api/auth/employee-reset-password`,

      {
        id: "52e97c43-3a30-49b3-ba28-9b761da64680",
        password: newPass.value,
      }
    );
    if (res.status === 200) {
      snackbar.error = false;
      snackbar.text = "Pomyślnie zmieniono hasło";
    }
  } catch (e) {
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd podczas zmiany hasła";
  } finally {
    loading.value = false;
    snackbar.showing = true;
  }
};
</script>

<template>
  <v-card width="440px" location="center" elevation="5" class="rounded-lg">
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
        <v-card
          height="64"
          width="64"
          color="#36BFF1"
          class="d-flex justify-center align-center"
        >
          <v-icon icon="mdi-hospital-building" size="48" color="white"></v-icon>
        </v-card>
      </v-container>
      <v-card-title>Zmień hasło</v-card-title>
      <v-card-subtitle>Zmień swoje hasło</v-card-subtitle>
    </v-card-item>
    <v-card-text>
      <v-form @submit.prevent="submit" ref="form">
        <v-row class="mx-4">
          <v-text-field
            type="input"
            label="Stare hasło"
            v-model="oldPass"
            variant="solo"
            :rules="passwordRules"
            color="blue-darken-2"
            required
          >
          </v-text-field>
        </v-row>
        <v-row class="mx-4">
          <v-text-field
            type="input"
            label="Nowe hasło"
            v-model="newPass"
            variant="solo"
            :rules="passwordRules"
            color="blue-darken-2"
            required
          >
          </v-text-field>
        </v-row>
        <v-row class="mx-4">
          <v-text-field
            type="input"
            label="Powtórz nowe hasło"
            v-model="newPassRepeat"
            variant="solo"
            :rules="repeatPasswordRules"
            color="blue-darken-2"
            required
          >
          </v-text-field>
        </v-row>
        <v-row>
          <v-col cols="auto" class="me-auto">
            <v-sheet class="pa-2 ma-2">
              <v-btn
                type="submit"
                variant="outlined"
                text="blue-darken"
                color="blue-darken-2"
                class="mt-2 button"
                @click="router.back()"
                >Wstecz</v-btn
              >
            </v-sheet>
          </v-col>
          <v-col cols="auto">
            <v-sheet class="pa-2 ma-2">
              <v-btn
                type="submit"
                color="blue-darken-2"
                class="mt-2 button"
                @click="submitData"
                >Zapisz</v-btn
              >
            </v-sheet>
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
