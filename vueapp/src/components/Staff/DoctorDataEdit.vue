<script setup lang="ts">
import UploadImage from "./UploadImage.vue";
import { ref, onBeforeMount } from "vue";
import { authorized, snackbar, specializations, router } from "@/main";
import { prefix } from "@/config";
import {
  nameRules,
  surnameRules,
  emailRules,
  phoneRules,
  dateRules,
  getPeselRules,
  notNull,
} from "@/validation";

const name = ref<string>("");
const lastName = ref<string>("");
const email = ref<string>("");
const pesel = ref<string>("");
const phone = ref<string>("");
const birthDate = ref<string>("");
const specialization = ref<number>(0);
const employeeId = ref<string>("");
const waiting = ref(false);
const uploadImage = ref(false);
const form = ref<any>();
const imgPath = ref<string>("");
// eslint-disable-next-line
// const props = defineProps({
//   id: Number,
// });
//const specialization = ref<string>("");
onBeforeMount(async () => {
  const id = router.currentRoute.value.params["id"] ?? null;
  if (id !== null) {
    try {
      const res = await authorized.get("/employee/" + id);
      const data = res.data;
      name.value = data.firstName;
      lastName.value = data.lastName;
      pesel.value = data.pesel;
      email.value = data.email;
      phone.value = data.phoneNumber;
      birthDate.value = data.dateOfBirth;
      specialization.value = data.specialization;
      employeeId.value = data.id;
      imgPath.value = `${prefix}/StaticFiles/${
        employeeId.value
      }.png?${new Date().getTime()}`;
    } catch (e: any) {
      console.log(e);
      snackbar.error = true;
      snackbar.text = "Wystąpił błąd podczas pobierania danych";
      snackbar.showing = true;
    }
  } else {
    try {
      const res = await authorized.get("/employee/account");
      const data = res.data;
      name.value = data.firstName;
      lastName.value = data.lastName;
      pesel.value = data.pesel;
      email.value = data.email;
      phone.value = data.phoneNumber;
      birthDate.value = data.dateOfBirth;
      specialization.value = data.specialization;
      employeeId.value = data.id;
    } catch (e: any) {
      console.log(e);
      snackbar.error = true;
      snackbar.text = "Wystąpił błąd podczas pobierania danych";
      snackbar.showing = true;
    }
  }
});

const submitData = async () => {
  const valid = await form.value.validate();
  if (!valid) return;
  try {
    waiting.value = true;
    const res = await authorized.patch(
      `http://localhost:7042/api/employee/update/${employeeId.value}`,
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
    if (res.status === 200) snackbar.error = false;
    snackbar.text = "Pomyślnie zaktualizowano dane";
    router.push("/staff/doctors");
  } catch (e) {
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd podczas edycji";
  } finally {
    waiting.value = false;
    snackbar.showing = true;
  }
};

const reloadImage = () => {
  imgPath.value = `${prefix}/StaticFiles/${
    employeeId.value
  }.png?${new Date().getTime()}`;
};
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg">
    <template #loader>
      <v-progress-linear
        :active="waiting"
        color="deep-purple"
        height="4"
        indeterminate
      ></v-progress-linear>
    </template>
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
              <v-img height="200" :src="imgPath"> </v-img>
            </v-col>
          </v-row>
          <v-dialog v-model="uploadImage">
            <template #activator>
              <v-btn
                variant="text"
                align-self="center"
                size="small"
                color="blue-darken-2"
                class="mt-2 button"
                @click="uploadImage = true"
              >
                Edytuj zdjęcie
              </v-btn>
            </template>
            <v-row no-gutters>
              <v-col class="d-flex justify-center">
                <UploadImage
                  @back="uploadImage = false"
                  :id="router.currentRoute.value.params['id'] as string ?? null"
                  @reload="reloadImage()"
                />
              </v-col>
            </v-row>
          </v-dialog>
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
                    :rules="getPeselRules(new Date(birthDate))"
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
                    :items="specializations"
                    item-title="title"
                    item-value="value"
                  >
                  </v-select>
                </v-col>
              </v-row>
            </v-form>
          </v-card-text>
        </v-container>

        <v-row no-gutters justify="start">
          <v-col cols="auto" class="me-auto">
            <v-btn
              type="submit"
              variant="outlined"
              text="blue-darken"
              color="blue-darken-2"
              class="mt-2 button"
              @click="router.push('/staff/doctors')"
            >
              Wstecz
            </v-btn>
          </v-col>
          <v-col cols="auto">
            <v-row no-gutters>
              <v-btn
                type="submit"
                color="blue-darken-2"
                class="button"
                @click="submitData"
              >
                Zapisz
              </v-btn>
            </v-row>
            <v-row no-gutters>
              <router-link
                v-if="employeeId"
                :to="`/staff/password/${employeeId}/reset`"
                custom
                v-slot="{ navigate }"
              >
                <v-btn
                  variant="text"
                  align-self="center"
                  size="small"
                  color="blue-darken-2"
                  class="button pa-0 mt-2"
                  @click="navigate"
                >
                  Zmień hasło
                </v-btn>
              </router-link>
            </v-row>
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
