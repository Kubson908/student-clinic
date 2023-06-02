<script setup lang="ts">
import { ref } from "vue";
import { passwordRules } from "@/validation";
import { router, snackbar, unauthorized } from "@/main";
const pass = ref<string>("");
const pass_repeat = ref<string>("");
const visible = ref<boolean>(false);
const visible_repeat = ref<boolean>(false);
const token = router.currentRoute.value.query
  .token!.toString()
  .replace(" ", "+");
const id = router.currentRoute.value.query.id;
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
const submit = async () => {
  console.log(token);
  try {
    await unauthorized.patch("/auth/reset-password", {
      id: id,
      token: token,
      password: pass.value,
    });
    snackbar.text = "Pomyślnie zmieniono hasło";
    snackbar.error = false;
    snackbar.showing = true;
    router.push("/login");
  } catch (error: any) {
    console.log(error);
    snackbar.text =
      error.response && error.response.status == 400
        ? "Link do zmiany hasła wygasł"
        : "Wystąpił nieznany błąd";
    snackbar.error = true;
  } finally {
    snackbar.showing = true;
  }
};
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg">
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
        Zresetuj hasło
      </v-card-title>
      <v-card-subtitle>Podaj nowe hasło do konta</v-card-subtitle>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent="submit">
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
            v-model="pass_repeat"
            label="Powtórz hasło"
            variant="solo"
            :type="visible_repeat ? 'text' : 'password'"
            :append-inner-icon="visible_repeat ? 'mdi-eye-off' : 'mdi-eye'"
            @click:append-inner="() => (visible_repeat = !visible_repeat)"
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
              type="submit"
              xs="12"
              sm="6"
              md="3"
              align-self="center"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
            >
              Potwierdź
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
