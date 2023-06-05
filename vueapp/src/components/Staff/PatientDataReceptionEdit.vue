<script setup lang="ts">
import { VForm } from "vuetify/lib/components/index";
import { authorized, router, snackbar } from "@/main";
import { ref, onBeforeMount } from "vue";
import {
  dateRules,
  getPeselRules,
  phoneRules,
  surnameRules,
  nameRules,
  emailRules,
} from "@/validation";

const name = ref<string>("");
const lastName = ref<string>("");
const email = ref<string>("");
const pesel = ref<string>("");
const phone = ref<string>("");
const birthDate = ref<string>("");
const terms = ref<boolean>(false);
const form = ref<typeof VForm | null>(null);
const id = ref<string>("");

onBeforeMount(async () => {
  id.value = router.currentRoute.value.params["id"] as string;
  let card = await authorized.get("/patient/patient-card/" + id.value);
  const data = card.data;
  name.value = data.firstName;
  lastName.value = data.lastName;
  phone.value = data.phoneNumber;
  email.value = data.email;
  pesel.value = data.pesel;
  birthDate.value = data.dateOfBirth;
  terms.value = data.verified;
  console.log(card.data);
});
const update = async () => {
  await authorized.patch("/patient/update/" + id.value, {
    firstName: name.value,
    lastName: lastName.value,
    phoneNumber: phone.value,
    email: email.value,
    pesel: pesel.value,
    dateOfBirth: birthDate.value,
    verified: terms.value,
  });
};
const submit = async (data: SubmitEvent) => {
  await data;
  const valid = (await form.value?.validate()).valid;
  if (!valid) return;
  try {
    await update();
    router.push("/staff/patients");
    snackbar.text = "Zaktualizowano dane";
    snackbar.error = false;
    snackbar.showing = true;
  } catch (error: any) {
    console.log(error);
    snackbar.text = "Wystąpił błąd";
    snackbar.error = true;
    snackbar.showing = true;
  }
};
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg mt-6">
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
      <!-- <v-card-title font-size="32">Twoje dane</v-card-title> -->
      <p class="font-weight-bold text-h4">Dane pacjenta</p>
      <v-card-subtitle>Edytuj dane pacjenta</v-card-subtitle>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent="submit" ref="form">
        <v-container>
          <v-card-text>
            <v-row>
              <v-col>
                <v-text-field
                  type="input"
                  label="Imie"
                  v-model="name"
                  :rules="nameRules"
                  variant="solo"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  type="input"
                  label="Nazwisko"
                  v-model="lastName"
                  :rules="surnameRules"
                  variant="solo"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  type="input"
                  label="Pesel"
                  v-model="pesel"
                  :rules="getPeselRules(new Date(birthDate))"
                  variant="solo"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  type="input"
                  label="Email"
                  v-model="email"
                  :rules="emailRules"
                  variant="solo"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  type="input"
                  label="Telefon"
                  v-model="phone"
                  :rules="phoneRules"
                  variant="solo"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  type="date"
                  label="Data urodzenia"
                  v-model="birthDate"
                  :rules="dateRules"
                  variant="solo"
                  color="blue-darken-2"
                  required
                >
                </v-text-field>
              </v-col>
            </v-row>
          </v-card-text>
          <v-row>
            <v-col>
              <v-checkbox
                v-model="terms"
                color="blue-darken-2"
                label="Zweryfikuj"
              ></v-checkbox>
            </v-col>
          </v-row>
        </v-container>

        <v-row justify="start">
          <v-col cols="auto" class="me-auto">
            <v-sheet class="pa-2 ma-2">
              <v-btn
                variant="outlined"
                text="blue-darken"
                color="blue-darken-2"
                @click="router.back()"
                class="mt-2 button"
              >
                Wstecz
              </v-btn>
            </v-sheet>
          </v-col>
          <v-col cols="auto">
            <v-sheet class="pa-2 ma-2">
              <v-btn type="submit" color="blue-darken-2" class="mt-2 button">
                Zapisz
              </v-btn>
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
