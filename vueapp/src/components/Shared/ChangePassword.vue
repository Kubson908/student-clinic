<script lang="ts" setup>
import { ref } from "vue";
import { VForm } from "vuetify/lib/components/index";
import { passwordRules } from "@/validation";
import { authorized, router, snackbar, user } from "@/main";

const form = ref<typeof VForm | null>(null);
const oldPassword = ref<string>("");
const newPassword = ref<string>("");
const newPasswordRepeat = ref<string>("");
const visible = ref(false);
const visibleNew = ref(false);
const visibleNewRepeat = ref(false);
const submit = async (data: SubmitEvent) => {
  const valid = ((await data) as any).valid;
  if (!valid) return;
  try {
    await authorized.patch("/auth/change-password", {
      CurrentPassword: oldPassword.value,
      NewPassword: newPassword.value,
    });
    snackbar.text = "Hasło zostało zmienione";
    snackbar.error = false;
    localStorage.clear();
    user.name = "Niezalogowany";
    user.isLoggedIn = false;
    user.roles = [];
  } catch (error: any) {
    console.log(error);
    snackbar.text =
      error.response && error.response.status == 409
        ? "Nowe hasło nie może być takie samo jak stare hasło"
        : "Wystąpił błąd";
    snackbar.error = true;
  } finally {
    snackbar.showing = true;
  }
  form.value?.reset();
};

const repeatPasswordRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (value === newPassword.value) return true;
    else return "Hasła muszą się zgadzać";
  },
];
</script>

<template>
  <v-row no-gutters justify="center" class="mx-2">
    <v-col cols="12" sm="6" md="3" align-self="center">
      <v-card elevation="5" class="rounded-lg">
        <v-card-item>
          <v-container class="d-flex justify-center align-center">
            <v-card
              height="64"
              width="64"
              color="#36BFF1"
              class="d-flex justify-center align-center"
            >
              <v-icon
                icon="mdi-hospital-building"
                size="48"
                color="white"
              ></v-icon>
            </v-card>
          </v-container>
          <v-card-title>Zmień hasło</v-card-title>
          <v-card-subtitle>Zmień swoje hasło</v-card-subtitle>
        </v-card-item>
        <v-spacer></v-spacer>
        <v-card-text>
          <v-form @submit.prevent="submit" ref="form">
            <div class="cont">
              <v-row no-gutters>
                <v-text-field
                  :type="visible ? 'text' : 'password'"
                  :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
                  @click:append-inner="() => (visible = !visible)"
                  v-model="oldPassword"
                  label="Stare hasło"
                  variant="solo"
                  :rules="passwordRules"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
              </v-row>
              <v-row no-gutters>
                <v-text-field
                  :type="visibleNew ? 'text' : 'password'"
                  :append-inner-icon="visibleNew ? 'mdi-eye-off' : 'mdi-eye'"
                  @click:append-inner="() => (visibleNew = !visibleNew)"
                  v-model="newPassword"
                  label="Nowe hasło"
                  variant="solo"
                  :rules="passwordRules"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
              </v-row>
              <v-row no-gutters>
                <v-text-field
                  :type="visibleNewRepeat ? 'text' : 'password'"
                  :append-inner-icon="
                    visibleNewRepeat ? 'mdi-eye-off' : 'mdi-eye'
                  "
                  @click:append-inner="
                    () => (visibleNewRepeat = !visibleNewRepeat)
                  "
                  v-model="newPasswordRepeat"
                  label="Powtórz nowe hasło"
                  variant="solo"
                  :rules="repeatPasswordRules"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
              </v-row>
            </div>
            <v-row no-gutters class="mt-2">
              <v-col cols="12" class="d-flex justify-space-between">
                <v-btn
                  variant="outlined"
                  color="blue-darken-2"
                  class="button"
                  @click="router.back()"
                  >Wstecz</v-btn
                >
                <v-btn type="submit" color="blue-darken-2" class="button"
                  >Zapisz</v-btn
                >
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<style>
.button {
  text-transform: unset !important;
}

/* .cont{
    height:40vh;
    overflow: scroll;
} */
.v-field__field .v-input__control .v-input__slot {
  min-height: auto !important;
  display: flex !important;
  align-items: center !important;
}
.signup {
  margin-bottom: 150px;
}
</style>
