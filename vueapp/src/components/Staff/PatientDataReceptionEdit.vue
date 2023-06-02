<script setup lang="ts">
import { authorized, router } from "@/main";
import { ref,onBeforeMount } from "vue";

const name = ref<string>("");
const lastName = ref<string>("");
const email = ref<string>("");
const pesel = ref<string>("");
const phone = ref<string>("");
const birthDate = ref<string>("");
const terms = ref<boolean>(false);

  onBeforeMount(async () => {
  // const res = await authorized.get("/appointment");
  const patientId = router.currentRoute.value.params["id"] as string;
  //console.log(patientId);
  let card = await authorized.get("/patient/patient-card/" + patientId);
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


const submit = () => {};
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
      <v-form @submit.prevent>
        <v-container>
          <v-card-text>
            <v-form @submit.prevent="submit" ref="form">
              <v-row>
                <v-col>
                  <v-text-field
                    type="input"
                    label="Imie"
                    v-model="name"
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
                    variant="solo"
                    color="blue-darken-2"
                    required
                  >
                  </v-text-field>
                </v-col>
                <v-col>
                  <v-text-field
                    type="input"
                    label="Data urodzenia"
                    v-model="birthDate"
                    variant="solo"
                    color="blue-darken-2"
                    required
                  >
                  </v-text-field>
                </v-col>
              </v-row>
            </v-form>
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
                type="submit"
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
