<script setup lang="ts">
import axios from "axios";
import { ref } from "vue";
import { VForm } from "vuetify/lib/components/index";
import { prefix } from "../config";
import { user } from "../main";

const visible = ref(false);
const form_login = ref<typeof VForm | null>(null);
const form_reset = ref<typeof VForm | null>(null);

const email = ref<string>("");
const pass = ref<string>("");
const email_reset = ref<string>("");
const remember_me = ref<boolean>(false);

const page = ref<number>(1);

const submit = async (data: SubmitEvent) => {
  await data;
  const login = await form_login.value?.validate();
  const reset = await form_reset.value?.validate();
  if (login && login.valid) {
    try {
      const res = await axios.post(`${prefix}/api/auth/login`, {
        email: email.value,
        password: pass.value,
      });
      localStorage.setItem("token", res.data.accessToken);
      localStorage.setItem("expireDate", res.data.expireDate);
      localStorage.setItem("user", res.data.user);
      user.name = res.data.user;
      user.loggedIn = true;
      console.log(user);
    } catch (error) {
      console.log(error);
    }

    alert(
      `E-mail: ${email.value}\nHasło: ${pass.value}\nZapamiętaj mnie: ${
        remember_me.value ? "Tak" : "Nie"
      }`
    );
    form_reset.value?.reset();
    form_login.value?.reset();
  }
  if (reset && reset.valid) {
    alert(`E-mail: ${email_reset.value}`);
    form_reset.value?.reset();
    form_login.value?.reset();
  }
};

const emailRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) =>
    /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(value) ||
    "Nieprawidłowy e-mail",
];

const passwordRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (value.length >= 8) return true;
    else return "Hasło musi zawierać przynajmniej 8 znaków";
  },
];
</script>

<template>
  <v-row justify="center" class="mx-2">
    <v-col xs="12" sm="6" md="3" align-self="center">
      <v-card elevation="5" class="rounded-lg">
        <v-window v-model="page" direction="vertical" reverse>
          <v-window-item :value="1">
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
              <v-card-title>Zaloguj się</v-card-title>
              <v-card-subtitle>Podaj dane logowania</v-card-subtitle>
            </v-card-item>
            <v-spacer></v-spacer>
            <v-card-text>
              <v-form ref="form_login" @submit.prevent="submit">
                <v-text-field
                  v-model="email"
                  type="email"
                  label="E-mail"
                  variant="solo"
                  :rules="emailRules"
                  class="py-1"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
                <v-text-field
                  v-model="pass"
                  label="Hasło"
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
                <v-checkbox
                  v-model="remember_me"
                  label="Zapamiętaj mnie"
                ></v-checkbox>
                <v-row justify="center">
                  <v-btn type="submit" color="blue-darken-2" class="mt-2"
                    >Zaloguj się</v-btn
                  >
                </v-row>
                <v-row justify="center">
                  <v-btn
                    variant="text"
                    size="small"
                    class="mt-2"
                    @click="page++"
                    >Zapomniałem hasła</v-btn
                  >
                </v-row>
              </v-form>
            </v-card-text>
          </v-window-item>
          <v-window-item :value="2">
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
              <v-card-title>Przypomnij hasło</v-card-title>
              <v-card-subtitle>Podaj e-mail</v-card-subtitle>
            </v-card-item>
            <v-spacer></v-spacer>
            <v-card-text>
              <v-form ref="form_reset" @submit.prevent="submit">
                <v-text-field
                  v-model="email_reset"
                  type="email"
                  label="E-mail"
                  variant="solo"
                  :rules="emailRules"
                  class="py-1"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
                <v-row>
                  <v-col>
                    <v-btn
                      variant="outlined"
                      color="blue-darken-2"
                      class="mt-2"
                      @click="page--"
                      >Wstecz</v-btn
                    >
                  </v-col>
                  <v-col>
                    <v-btn type="submit" color="blue-darken-2" class="mt-2"
                      >Potwierdź</v-btn
                    >
                  </v-col>
                </v-row>
              </v-form>
            </v-card-text>
          </v-window-item>
        </v-window>
      </v-card>
    </v-col>
  </v-row>
</template>

<style></style>
