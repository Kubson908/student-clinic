<script setup lang="ts">
import { router, authorized, specializations, user, snackbar } from "@/main";
import { prefix } from "@/config";
import { onBeforeMount, ref } from "vue";
import {
  nameRules,
  surnameRules,
  dateRules,
  getPeselRules,
  emailRules,
  phoneRules,
} from "@/validation";
const checkRole = (role: string) => {
  const roles = user.roles!;
  return roles.includes(role);
};
const name = ref<string>("");
const lastName = ref<string>("");
const email = ref<string>("");
const pesel = ref<string>("");
const phone = ref<string>("");
const birthDate = ref<string>("");
const specialization = ref<string>("");
const employeeId = ref<string>("");
const form = ref<any>();
const waiting = ref(false);
const specializationNumber = ref<number | null>(null);
onBeforeMount(async () => {
  try {
    const res = await authorized.get(`/employee/account`);
    const data = res.data;
    name.value = data.firstName;
    lastName.value = data.lastName;
    pesel.value = data.pesel;
    email.value = data.email;
    phone.value = data.phoneNumber;
    birthDate.value = data.dateOfBirth;
    specialization.value =
      specializations.find((s) => s.value === data.specialization)?.title ?? "";
    employeeId.value = data.id;
    specializationNumber.value = data.specialization;
  } catch (error) {
    console.log(error);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd przy pobieraniu danych";
    snackbar.showing = true;
  }
});
const submitData = async () => {
  const valid = await form.value.validate();
  if (!valid) return;
  try {
    waiting.value = true;
    const res = await authorized.patch(
      `http://localhost:7042/api/Employee/update/${employeeId.value}`,
      {
        firstName: name.value,
        lastName: lastName.value,
        email: email.value,
        pesel: pesel.value,
        specialization: specializationNumber.value,
        phoneNumber: phone.value,
        dateOfBirth: birthDate.value,
      }
    );
    if (res.status === 200) snackbar.error = false;
    snackbar.text = "Pomyślnie zaktualizowano dane";
    router.push("/staff/doctors");
  } catch (e) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd podczas edycji";
  } finally {
    waiting.value = false;
    snackbar.showing = true;
  }
};
</script>

<template>
  <v-row no-gutters class="ma-auto"
    ><v-col class="ma-auto">
      <v-card
        width="560px"
        elevation="5"
        class="rounded-lg ma-4"
        v-if="checkRole('Staff')"
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
        <v-card-title>Twoje dane</v-card-title>
        <v-card-subtitle>Edytuj dane</v-card-subtitle>
      </v-card-item>
      <v-spacer></v-spacer>
        <v-card-text>
          <v-form @submit.prevent ref="form">
            <v-container>
              <v-row no-gutters>
                <p class="font-weight-bold">Dane osobowe</p>
              </v-row>
              <v-row no-gutters><v-divider></v-divider></v-row>
              <v-row no-gutters>
                <v-col class="mt-4">
                  <v-text-field
                    type="input"
                    v-model="name"
                    label="Imię"
                    variant="solo"
                    :rules="nameRules"
                    color="blue-darken-2"
                    class="mx-2"
                    required
                  >
                  </v-text-field>
                </v-col>
                <v-col class="mt-4">
                  <v-text-field
                    type="input"
                    v-model="lastName"
                    label="Nazwisko"
                    variant="solo"
                    :rules="surnameRules"
                    color="blue-darken-2"
                    class="mx-2"
                    required
                  >
                  </v-text-field>
                </v-col>
              </v-row>
              <v-row no-gutters>
                <v-col>
                  <v-text-field
                    type="Date"
                    label="Data urodzenia"
                    v-model="birthDate"
                    variant="solo"
                    :rules="dateRules"
                    color="blue-darken-2"
                    class="mx-2"
                    required
                  >
                  </v-text-field>
                </v-col>
                <v-col>
                  <v-text-field
                    type="input"
                    label="Pesel"
                    v-model="pesel"
                    variant="solo"
                    :rules="getPeselRules(new Date(birthDate))"
                    color="blue-darken-2"
                    class="mx-2"
                    required
                  >
                  </v-text-field>
                </v-col>
              </v-row>
              <v-row no-gutters>
                <v-col>
                  <v-text-field
                    type="email"
                    label="Email"
                    v-model="email"
                    variant="solo"
                    :rules="emailRules"
                    color="blue-darken-2"
                    class="mx-2"
                    required
                  >
                  </v-text-field>
                </v-col>
                <v-col>
                  <v-text-field
                    type="input"
                    label="Nr telefonu"
                    v-model="phone"
                    variant="solo"
                    :rules="phoneRules"
                    color="blue-darken-2"
                    class="mx-2"
                    required
                  >
                  </v-text-field>
                </v-col>
              </v-row>
              <v-row no-gutters><v-divider></v-divider></v-row>
            </v-container>

            <v-row no-gutters justify="start">
              <v-col
                xs="12"
                sm="6"
                md="3"
                align-self="center"
                class="text-left"
              >
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
              <v-col class="text-right">
                <v-btn
                  size="large"
                  class="mt-2 button"
                  color="blue-darken-2"
                  @click="submitData"
                >
                  Zapisz
                </v-btn>
              </v-col>
            </v-row>
            <v-row no-gutters>
              <v-col class="text-right">
                <router-link
                  to="/doctor/change-password"
                  custom
                  v-slot="{ navigate }"
                >
                  <v-btn
                    variant="text"
                    align-self="center"
                    size="small"
                    class="mt-2 button"
                    value="/doctor/change-password"
                    @click="navigate"
                  >
                    Zmień hasło
                  </v-btn>
                </router-link>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>

      <v-card
        width="560px"
        elevation="5"
        class="rounded-lg ma-4"
        v-else-if="checkRole('Employee')"
      >
        <v-card-item>
          <!-- <v-card-title font-size="32">Twoje dane</v-card-title> -->
          <p class="font-weight-bold text-h4">Twoje dane</p>
        </v-card-item>
        <v-spacer></v-spacer>
        <v-card-text>
          <v-form @submit.prevent>
            <v-container>
              <v-row>
                <v-col align="center" class="img-static-height">
                  <v-img
                    v-if="employeeId"
                    height="200"
                    :src="`${prefix}/StaticFiles/${employeeId}.png`"
                  >
                  </v-img>
                </v-col>
              </v-row>
              <v-row>
                <p class="font-weight-bold">Dane osobowe</p>
              </v-row>
              <v-row><v-divider></v-divider></v-row>
              <v-row>
                <v-col
                  class="font-weight-bold text-blue-darken-1 text-left"
                  cols="4"
                >
                  Imię i nazwisko
                </v-col>
                <v-col class="text-left"> {{ name }} {{ lastName }} </v-col>
              </v-row>
              <v-row>
                <v-col
                  class="font-weight-bold text-blue-darken-1 text-left"
                  cols="4"
                >
                  PESEL
                </v-col>
                <v-col class="text-left"> {{ pesel }} </v-col>
              </v-row>
              <v-row>
                <v-col
                  cols="4"
                  class="font-weight-bold text-blue-darken-1 text-left"
                >
                  Data urodzenia
                </v-col>
                <v-col class="text-left"> {{ birthDate }} </v-col>
              </v-row>
              <v-row>
                <v-col
                  class="font-weight-bold text-blue-darken-1 text-left"
                  cols="4"
                >
                  Nr telefonu
                </v-col>
                <v-col class="text-left"> {{ phone }} </v-col>
              </v-row>
              <v-row>
                <v-col
                  class="font-weight-bold text-blue-darken-1 text-left"
                  cols="4"
                >
                  Email
                </v-col>
                <v-col class="text-left"> {{ email }} </v-col>
              </v-row>
              <v-row>
                <v-col
                  class="font-weight-bold text-blue-darken-1 text-left"
                  cols="4"
                >
                  Specjalizacja
                </v-col>
                <v-col class="text-left">
                  {{ specialization }}
                </v-col>
              </v-row>
              <v-row><v-divider></v-divider></v-row>
            </v-container>

            <v-row justify="start">
              <v-col
                xs="12"
                sm="6"
                md="3"
                align-self="center"
                class="text-left"
              >
                <v-btn
                  variant="outlined"
                  size="large"
                  class="mt-2 button"
                  color="blue-darken-2"
                  @click="router.back()"
                >
                  Wstecz
                </v-btn>
                <router-link
                  to="/doctor/change-password"
                  custom
                  v-slot="{ navigate }"
                >
                  <v-btn
                    variant="text"
                    align-self="center"
                    size="small"
                    class="mt-2 button"
                    value="/doctor/change-password"
                    @click="navigate"
                  >
                    Zmień hasło
                  </v-btn>
                </router-link>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>
    </v-col></v-row
  >
</template>

<style>
.button {
  text-transform: unset !important;
}
.img-static-height {
  height: 200px !important;
}
</style>
