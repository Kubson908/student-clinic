<script lang="ts" setup>
import { ref } from "vue";
import { VForm } from "vuetify/lib/components/index";
// const visible = ref(false);
// const visible2 = ref(false);

const form = ref<typeof VForm | null>(null);
const name = ref<string>("");
const surname = ref<string>("");
const birth_date = ref<string>("");
const pesel = ref<string>("");
const email = ref<string>("");
const phone = ref<string>("");
const pass = ref<string>("");
const rpass = ref<string>("");

const submit = async (data: SubmitEvent) => {
  const valid = ((await data) as any).valid;
  if (!valid) return;
  alert(
    `Imię: ${name.value}\nNazwisko: ${surname.value}\nData urodzenia: ${birth_date.value}\nPesel: ${pesel.value}\nHasło: ${pass.value}\nPowtórzone hasło: ${rpass.value}\n`
  );
  form.value?.reset();
};

// const passwordRules = [
//   (value: string) => {
//     if (value) return true;
//     else return "Pole jest wymagane";
//   },
//   (value: string) => {
//     if (value.toLowerCase() !== value && /\d/.test(value) && value.length >= 8)
//       return true;
//     else
//       return "Hasło musi zawierać przynajmniej 8 znaków, małą literę, dużą literę i cyfrę";
//   },
// ];
const nameRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (
      value[0].toLowerCase() !== value[0] &&
      value.slice(1).toLowerCase() === value.slice(1)
    )
      return true;
    else return "Imię powinno zaczynać się od wielkiej litery";
  },
  (value: string) => {
    if (
      !/\d/.test(value) &&
      !/[^a-zA-Z0-9\u0118\u0119\u00F3\u00D3\u0141\u0142\u0104\u0105\u017B\u017C\u017A\u0179\u0106\u0107\u0143\u0144\u015A\u015B]/.test(
        value
      ) &&
      !/_/.test(value)
    )
      return true;
    else return "Imię może zawierać tylko litery";
  },
];
const surnameRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (
      value[0].toLowerCase() !== value[0] &&
      value.slice(1).toLowerCase() === value.slice(1)
    )
      return true;
    else return "Nazwisko powinno zaczynać się od wielkiej litery";
  },
  (value: string) => {
    if (
      !/\d/.test(value) &&
      !/[^a-zA-Z0-9\u0118\u0119\u00F3\u00D3\u0141\u0142\u0104\u0105\u017B\u017C\u017A\u0179\u0106\u0107\u0143\u0144\u015A\u015B]/.test(
        value
      ) &&
      !/_/.test(value)
    )
      return true;
    else return "Nazwisko może zawierać tylko litery";
  },
];
const peselRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (!/\D/.test(value) && value.length === 11) return true;
    else return "Pesel musi składać się z 11 cyfr";
  },
];
// const repeatPasswordRules = [
//   (value: string) => {
//     if (value) return true;
//     else return "Pole jest wymagane";
//   },
//   (value: string) => {
//     if (value === pass.value) return true;
//     else return "Hasła muszą się zgadzać";
//   },
// ];
const emailRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) =>
    /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(value) ||
    "Nieprawidłowy e-mail",
];
const phoneRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (!/\D/.test(value) && value.length === 9) return true;
    else return "Numer telefonu musi składać się z cyfr";
  },
];
const dateRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    const dateFromValue: Date = new Date(value);
    if (dateFromValue < new Date()) return true;
    else return "Podaj prawidłową datę";
  },
];
</script>

<template>
  <v-card
    width="40vw"
    location="center"
    elevation="5"
    class="rounded-lg signup"
  >
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
            <v-textarea
                variant="filled"
                auto-grow
                label="Przyjmowane Leki"
                rows="2"
                row-height="20"
                
              ></v-textarea>
          </v-row>
          <v-row>
            <v-textarea
                variant="filled"
                auto-grow
                label="alergia"
                rows="2"
                row-height="20"
              ></v-textarea>
          </v-row>
        </div>
        <v-row justify="center">
          <v-btn type="submit" color="blue-darken-2" class="mt-2 button"
            >Zarejestruj się</v-btn
          >
        </v-row>
        <v-row justify="center">
          <v-btn variant="text" size="small" class="mt-2 button"
            >Mam już konto</v-btn
          >
        </v-row>
      </v-form>
    </v-card-text>
  </v-card>
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
