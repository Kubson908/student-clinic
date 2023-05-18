<script lang="ts" setup>
import { ref } from "vue";

const form = ref<HTMLFormElement | null>(null);
const email = ref<string>("");
const emailrepeat = ref<string>("");

const submit = async (data: SubmitEvent) => {
  const valid = ((await data) as any).valid;
  if (!valid) return;
  alert(`email: ${email.value}\n`);
  form.value?.reset();
};
</script>

<template>
  <v-card width="440px" location="center" elevation="5" class="rounded-lg">
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
      <v-card-title>Zmień hasło</v-card-title>
      <v-card-subtitle
        >Zmień hasło lekarza: imie i nazwisko lekarza</v-card-subtitle>
    </v-card-item>
    <v-card-text>
      <v-form @submit.prevent="submit" ref="form">
        <v-row class="mx-4">
          <v-text-field
            type="input"
            label="Nowe hasło"
            v-model="email"
            variant="solo"
            :rules="[(v) => !!v || 'Pole jest wymagane']"
            color="blue-darken-2"
            required
          >
          </v-text-field>
        </v-row>
      </v-form>
    </v-card-text>
    <v-card-text>
      <v-form @submit.prevent="submit" ref="form">
        <v-row class="mx-4">
          <v-text-field
            type="input"
            label="Powtórz hasło"
            v-model="emailrepeat"
            variant="solo"
            :rules="[(v) => !!v || 'Pole jest wymagane']"
            color="blue-darken-2"
            required
          >
          </v-text-field>
        </v-row>
        <v-row>
          <v-col cols="auto" class="me-auto">
            <v-sheet class="pa-2 ma-2">
              <v-btn 
              type="submit"
              variant="outlined"
              text="blue-darken"
              color="blue-darken-2"  
              class="mt-2 button"
                >Wstecz</v-btn
              >
            </v-sheet>
          </v-col>
          <v-col cols="auto">
            <v-sheet class="pa-2 ma-2">
              <v-btn
                type="submit"
                color="blue-darken-2"
                class="mt-2 button"
                >Dalej</v-btn
              >
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
