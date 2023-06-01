<script lang="ts" setup>
import SignedUp from "./SignedUp.vue";
import { ref } from "vue";
import { VForm } from "vuetify/lib/components/index";
import {
  dateRules,
  nameRules,
  surnameRules,
  peselRules,
  phoneRules,
  passwordRules,
  emailRules,
} from "@/validation";
import { snackbar, unauthorized } from "@/main";
const visible = ref(false);
const visible2 = ref(false);

const form = ref<typeof VForm | null>(null);
const name = ref<string>("");
const surname = ref<string>("");
const birth_date = ref<string>("");
const pesel = ref<string>("");
const email = ref<string>("");
const phone = ref<string>("");
const pass = ref<string>("");
const rpass = ref<string>("");
const page = ref<number>(1);

const email_to_confirm = ref<string>("");
const submit = async (data: SubmitEvent) => {
  const valid = ((await data) as any).valid;
  if (!valid) return;
  try {
    await unauthorized.post("/auth/register", {
      firstName: name.value,
      lastName: surname.value,
      dateOfBirth: birth_date.value,
      pesel: pesel.value,
      email: email.value,
      password: pass.value,
      confirmPassword: rpass.value,
      phoneNumber: phone.value,
    });
    email_to_confirm.value = email.value;
    page.value = 1;
    snackbar.text = "Na e-mail podany w rejestracji wysłano link weryfikacyjny";
    snackbar.error = false;
  } catch (e) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Błąd podczas rejestracji";
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
    if (value === pass.value) return true;
    else return "Hasła muszą się zgadzać";
  },
];
</script>

<template>
  <v-row no-gutters>
    <v-col>
      <v-card
        width="40vw"
        location="center"
        elevation="5"
        class="rounded-lg signup"
      >
        <v-window v-model="page" direction="vertical" reverse>
          <v-window-item :value="0">
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
              <v-card-title>Zarejestruj się</v-card-title>
              <v-card-subtitle>Podaj dane rejestracji</v-card-subtitle>
            </v-card-item>
            <v-spacer></v-spacer>
            <v-card-text>
              <v-form @submit.prevent="submit" ref="form">
                <div class="cont">
                  <v-row>
                    <v-col class="py-1">
                      <v-text-field
                        type="input"
                        v-model="name"
                        label="Imię"
                        variant="solo"
                        :rules="nameRules"
                        color="blue-darken-2"
                        required
                      >
                      </v-text-field>
                    </v-col>
                    <v-col class="py-1">
                      <v-text-field
                        type="input"
                        v-model="surname"
                        label="Nazwisko"
                        variant="solo"
                        :rules="surnameRules"
                        color="blue-darken-2"
                        required
                      >
                      </v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="py-1">
                      <v-text-field
                        type="Date"
                        label="Data urodzenia"
                        v-model="birth_date"
                        variant="solo"
                        :rules="dateRules"
                        color="blue-darken-2"
                        required
                      >
                      </v-text-field>
                    </v-col>
                    <v-col class="py-1">
                      <v-text-field
                        type="input"
                        label="Pesel"
                        v-model="pesel"
                        variant="solo"
                        :rules="peselRules"
                        color="blue-darken-2"
                        required
                      >
                      </v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="py-1">
                      <v-text-field
                        type="email"
                        label="Email"
                        v-model="email"
                        variant="solo"
                        :rules="emailRules"
                        color="blue-darken-2"
                        required
                      >
                      </v-text-field>
                    </v-col>
                    <v-col class="py-1">
                      <v-text-field
                        type="input"
                        label="Nr telefonu"
                        v-model="phone"
                        variant="solo"
                        :rules="phoneRules"
                        color="blue-darken-2"
                        required
                      >
                      </v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-text-field
                      label="Hasło"
                      v-model="pass"
                      variant="solo"
                      :type="visible ? 'text' : 'password'"
                      :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
                      @click:append-inner="() => (visible = !visible)"
                      :rules="passwordRules"
                      class="px-3 py-1"
                      color="blue-darken-2"
                      required
                    >
                    </v-text-field>
                  </v-row>
                  <v-row>
                    <v-text-field
                      label="Powtórz hasło"
                      v-model="rpass"
                      variant="solo"
                      :type="visible2 ? 'text' : 'password'"
                      :append-inner-icon="visible2 ? 'mdi-eye-off' : 'mdi-eye'"
                      @click:append-inner="() => (visible2 = !visible2)"
                      :rules="repeatPasswordRules"
                      class="px-3 py-1"
                      height="10px"
                      color="blue-darken-2"
                      required
                    >
                    </v-text-field>
                  </v-row>
                </div>
                <v-row justify="center">
                  <v-btn type="submit" color="blue-darken-2" class="mt-2 button"
                    >Zarejestruj się</v-btn
                  >
                </v-row>
                <v-row justify="center">
                  <router-link to="/login" custom v-slot="{ navigate }">
                    <v-btn
                      variant="text"
                      size="small"
                      class="mt-2 button"
                      @click="navigate"
                      role="link"
                    >
                      Mam już konto
                    </v-btn>
                  </router-link>
                </v-row>
              </v-form>
            </v-card-text>
          </v-window-item>
          <v-window-item :value="1">
            <SignedUp :email_to_confirm="email_to_confirm" />
          </v-window-item>
        </v-window>
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
