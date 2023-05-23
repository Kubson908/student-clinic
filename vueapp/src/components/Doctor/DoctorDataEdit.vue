<script setup lang="ts">
// Wojtek
import { ref } from "vue";
import axios from "axios";
import { onBeforeMount, computed } from "vue";
import {
  nameRules,
  surnameRules,
  emailRules,
  phoneRules,
  dateRules,
  peselRules,
  notNull,
} from "@/validation";
const name = ref<string>("");
const lastName = ref<string>("");
const email = ref<string>("");
const pesel = ref<string>("");
const phone = ref<string>("");
const birthDate = ref<string>("");
const specialization = ref<number>(0);
const snackbar = ref<boolean>(false);
const error = ref<boolean>(false);
const snackbarText = computed(() => {
  return error.value
    ? "Wystąpił błąd podczas edycji"
    : "Pomyślnie zaktualizowano dane";
});

const form = ref<any>();
// eslint-disable-next-line
const props = defineProps({
  id: Number,
});
//const specialization = ref<string>("");
onBeforeMount(async () => {
  const res = await axios.get(
    `http://localhost:7042/api/Employee/${"52e97c43-3a30-49b3-ba28-9b761da64680"}`,
    {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    }
  );
  const data = res.data;
  name.value = data.firstName;
  lastName.value = data.lastName;
  pesel.value = data.pesel;
  email.value = data.email;
  phone.value = data.phoneNumber;
  birthDate.value = data.dateOfBirth;
  specialization.value = data.specialization;
});

const submitData = async () => {
  const valid = await form.value.validate();
  if (!valid) return;
  try {
    const res = await axios.patch(
      `http://localhost:7042/api/Employee/update/${"52e97c43-3a30-49b3-ba28-9b761da64680"}`,
      {
        firstName: name.value,
        lastName: lastName.value,
        email: email.value,
        pesel: pesel.value,
        specialization: specialization.value,
        phoneNumber: phone.value,
        dateOfBirth: birthDate.value,
      }
    );
    if (res.status === 200) error.value = false;
  } catch (e) {
    error.value = true;
  } finally {
    snackbar.value = true;
  }
};
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg mt-6">
    <v-card-item>
      <!-- <v-card-title font-size="32">Twoje dane</v-card-title> -->
      <p class="font-weight-bold text-h4">Dane lekarza</p>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent>
        <v-container>
          <v-row>
            <v-col align="center">
              <v-img
                height="200"
                src="https://st2.depositphotos.com/1010683/5848/i/950/depositphotos_58482379-stock-photo-male-asian-doctor.jpg"
              >
              </v-img>
            </v-col>
          </v-row>
          <v-btn
            variant="text"
            align-self="center"
            size="small"
            color="blue-darken-2"
            class="mt-2 button"
          >
            Edytuj zdjęcie
          </v-btn>
          <v-card-text>
            <v-form ref="form">
              <v-row>
                <v-col>
                  <v-text-field
                    type="input"
                    label="Imie"
                    v-model="name"
                    variant="solo"
                    color="blue-darken-2"
                    :rules="nameRules"
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
                    variant="solo"
                    :rules="peselRules"
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
                    :rules="dateRules"
                    v-model="birthDate"
                    variant="solo"
                    color="blue-darken-2"
                    required
                  >
                  </v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-select
                    variant="solo"
                    v-model="specialization"
                    label="Specjalizacja"
                    :rules="notNull"
                    :items="[
                      { value: 0, title: 'Internista' },
                      { value: 1, title: 'Pulmonolog' },
                      { value: 2, title: 'Okulista' },
                      { value: 3, title: 'Gastrolog' },
                    ]"
                    item-title="title"
                    item-value="value"
                  >
                  </v-select>
                </v-col>
              </v-row>
            </v-form>
          </v-card-text>
        </v-container>

        <v-row justify="start">
          <v-col cols="auto" class="me-auto">
            <v-sheet class="pa-2 ma-2">
              <router-link to="/doctor/doctorpage" custom v-slot="{ navigate }">
                <v-btn
                  type="submit"
                  variant="outlined"
                  text="blue-darken"
                  color="blue-darken-2"
                  class="mt-2 button"
                  @click="navigate"
                >
                  Wstecz
                </v-btn>
              </router-link>
            </v-sheet>
          </v-col>
          <v-col cols="auto">
            <v-sheet class="pa-2 ma-2">
              <v-btn
                type="submit"
                color="blue-darken-2"
                class="mt-2 button"
                @click="submitData"
              >
                Zapisz
              </v-btn>
            </v-sheet>
            <router-link to="/staff/passwordreset" custom v-slot="{ navigate }">
              <v-btn
                variant="text"
                align-self="center"
                size="small"
                color="blue-darken-2"
                class="button pa-0 ma-0"
                @click="navigate"
              >
                Zmień hasło
              </v-btn>
            </router-link>
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
    <v-snackbar v-model="snackbar" timeout="3000">
      {{ snackbarText }}

      <template v-slot:actions>
        <v-btn color="blue" variant="text" @click="snackbar = false">
          Zamknij
        </v-btn>
      </template>
    </v-snackbar>
  </v-card>
</template>

<style>
.button {
  text-transform: unset !important;
}
</style>
