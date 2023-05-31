<script lang="ts" setup>
import { VForm } from "vuetify/lib/components/index";
import { router, authorized, specializations } from "@/main";
import { onBeforeMount, ref } from "vue";
import {
  dateRules,
  peselRules,
  phoneRules,
  surnameRules,
  nameRules,
  emailRules,
} from "@/validation";
// const visible = ref(false);
// const visible2 = ref(false);

const name = ref<string>("");
const lastName = ref<string>("");
const email = ref<string>("");
const pesel = ref<string>("");
const phone = ref<string>("");
const meds = ref<string>("");
const allergies = ref<string>("");
const birthDate = ref<string>("");
// const specialization = ref<number>(0);
onBeforeMount(async () => {
  const res = await authorized.get(`/Patient/patient-card`);
  const data = res.data;
  name.value = data.firstName;
  lastName.value = data.lastName;
  pesel.value = data.pesel;
  email.value = data.email;
  phone.value = data.phoneNumber;
  birthDate.value = data.dateOfBirth;
  allergies.value = data.allergies;
  meds.value = data.medicines;
  // specialization.value = data.specialization;
});

// const submit = async (data: SubmitEvent) => {
//   const valid = ((await data) as any).valid;
//   if (!valid) return;
//   alert(
//     `Imię: ${name.value}\nNazwisko: ${surname.value}\nData urodzenia: ${birth_date.value}\nPesel: ${pesel.value}\nHasło: ${pass.value}\nPowtórzone hasło: ${rpass.value}\n`
//   );
//   form.value?.reset();
// };
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
      <v-card-title>Dane Pacjenta</v-card-title>
      <v-card-subtitle>Edytuj dane pacjenta</v-card-subtitle>
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
            <v-textarea
              variant="filled"
              auto-grow
              label="Przyjmowane Leki"
              v-model="meds"
              rows="2"
              row-height="20"
            ></v-textarea>
          </v-row>
          <v-row>
            <v-textarea
              variant="filled"
              auto-grow
              label="alergia"
              v-model="allergies"
              rows="2"
              row-height="20"
            ></v-textarea>
          </v-row>
        </div>
        <v-row justify="start">
          <v-col align-self="center" class="text-left">
            <v-btn
              variant="outlined"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
            >
              Wstecz
            </v-btn>
          </v-col>
          <v-col>
            <v-btn size="large" class="mt-2 button" color="blue-darken-2">
              zapisz
            </v-btn>
          </v-col>
        </v-row>
        <v-row>
          <v-col> </v-col>
          <v-col>
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
