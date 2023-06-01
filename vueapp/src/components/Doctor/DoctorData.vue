<script setup lang="ts">
import { router, authorized, specializations, user } from "@/main";
import { onBeforeMount, ref } from "vue";
import { nameRules, surnameRules, dateRules, peselRules, emailRules, phoneRules } from "@/validation";
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
const specialization = ref<number>(0);
onBeforeMount(async () => {
  const res = await authorized.get(
    `http://localhost:7042/api/Employee/${"52e97c43-3a30-49b3-ba28-9b761da64680"}`
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

</script>

<template>
  <v-card
    width="560px"
    location="center"
    elevation="5"
    class="rounded-lg"
    v-if="checkRole('Staff')"
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
            <v-col align="center">
              <v-img
                height="200"
                src="https://st2.depositphotos.com/1010683/5848/i/950/depositphotos_58482379-stock-photo-male-asian-doctor.jpg"
              >
              </v-img>
            </v-col>
          </v-row>
          <v-row>
            <p class="font-weight-bold">Dane osobowe</p>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
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
                v-model="lastName"
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
                v-model="birthDate"
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
            <v-select v-model="specialization">
              
            </v-select>

            <!-- <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Specjalizacja
            </v-col>
            <v-col class="text-left"> {{specializations.find(
                      (s) => s.value === specialization
                    )?.title}} </v-col> -->
          </v-row>
          <v-row><v-divider></v-divider></v-row>
        </v-container>

        <v-row justify="start">
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
            <router-link
              to="/doctor/passwordreset"
              custom
              v-slot="{ navigate }"
            >
              <v-btn
                variant="text"
                align-self="center"
                size="small"
                class="mt-2 button"
                value="/doctor/passwordreset"
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
    location="center"
    elevation="5"
    class="rounded-lg"
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
            <v-col align="center">
              <v-img
                height="200"
                src="https://st2.depositphotos.com/1010683/5848/i/950/depositphotos_58482379-stock-photo-male-asian-doctor.jpg"
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
              {{
                specializations.find((s) => s.value === specialization)?.title
              }}
            </v-col>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
        </v-container>

        <v-row justify="start">
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
            <router-link
              to="/doctor/passwordreset"
              custom
              v-slot="{ navigate }"
            >
              <v-btn
                variant="text"
                align-self="center"
                size="small"
                class="mt-2 button"
                value="/doctor/passwordreset"
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
</template>

<style>
.button {
  text-transform: unset !important;
}
</style>
