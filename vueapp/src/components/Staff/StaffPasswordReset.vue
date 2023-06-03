<script setup lang="ts">
import { ref } from "vue";
import { onBeforeMount } from "vue";
import { authorized } from "@/main";
import { passwordRules } from "@/validation";
import { snackbar, router } from "@/main";

const name = ref<string>("");
const lastName = ref<string>("");
const pass = ref<string>("");
const passRepeat = ref<string>("");
const form = ref<any>();
const visible = ref<boolean>(false);
const visibleRepeat = ref<boolean>(false);
const loading = ref<boolean>(false);
// const props = defineProps({
// id: Number,
// });
onBeforeMount(async () => {
  const doc = await authorized.get(
    `/Employee/${"52e97c43-3a30-49b3-ba28-9b761da64680"}`
  );
  const data = doc.data;
  name.value = data.firstName;
  lastName.value = data.lastName;
});

const repeatPasswordRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (value === pass.value) return true;
    else return "Hasła muszą się zgadzać";
  },
];

const submitData = async () => {
  const valid = await form.value.validate();
  if (!valid) return;
  try {
    loading.value = true;
    const res = await authorized.patch(
      `http://localhost:7042/api/auth/employee-reset-password`,

      {
        id: "52e97c43-3a30-49b3-ba28-9b761da64680",
        password: pass.value,
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
  <v-card width="560px" location="center" elevation="5" class="rounded-lg">
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
      <v-card-title class="font-weight-bold text-h5" font-size="56">
        Zmień hasło
      </v-card-title>
      <v-card-subtitle
        >Zmień hasło lekarza: {{ name }} {{ lastName }}</v-card-subtitle
      >
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form ref="form">
        <v-container>
          <v-text-field
            v-model="pass"
            label="Nowe hasło"
            variant="solo"
            :type="visible ? 'text' : 'password'"
            :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
            @click:append-inner="() => (visible = !visible)"
            :rules="passwordRules"
            class="py-1"
            color="blue-darken-2"
            required
          >
          </v-text-field>
          <v-text-field
            v-model="passRepeat"
            label="Powtórz hasło"
            variant="solo"
            :type="visibleRepeat ? 'text' : 'password'"
            :append-inner-icon="visibleRepeat ? 'mdi-eye-off' : 'mdi-eye'"
            @click:append-inner="() => (visibleRepeat = !visibleRepeat)"
            :rules="repeatPasswordRules"
            class="py-1"
            color="blue-darken-2"
            required
          >
          </v-text-field>
        </v-container>

        <v-row justify="center">
          <v-col xs="12" sm="6" md="3" align-self="center" class="text-left">
            <v-btn
              variant="outlined"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
              @click="router.back()"
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
              @click="submitData"
            >
              Zapisz
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
