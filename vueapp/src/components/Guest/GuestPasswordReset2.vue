<script setup lang="ts">
//import VueDatePicker from "@vuepic/vue-datepicker";
// let date = new Date(Date.now() - new Date().getTimezoneOffset() * 60000)
//   .toISOString()
//   .substr(0, 10);
// let menu = false;
// let modal = false;
// let menu2 = false;
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg">
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
      <v-card-title class="font-weight-bold text-h5" font-size="56">
        Zresetuj hasło
      </v-card-title>
      <v-card-subtitle>Podaj nowe hasło do konta</v-card-subtitle>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent>
        <v-container>
          <v-text-field
            v-model="pass"
            label="Nowe hasło"
            variant="solo"
            :type="visible ? 'text' : 'password'"
            :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
            @click:append-inner="() => (visible = !visible)"
            :rules="passwordRules"
            class="py-1"
            color="blue-darken-2"
            required
          >
          </v-text-field>
          <v-text-field
            v-model="pass"
            label="Powtórz hasło"
            variant="solo"
            :type="visible ? 'text' : 'password'"
            :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
            @click:append-inner="() => (visible = !visible)"
            :rules="passwordRules"
            class="py-1"
            color="blue-darken-2"
            required
          >
          </v-text-field>
          
        </v-container>

        <v-row justify="center">
          <v-col xs="12" sm="6" md="3" align-self="center" class="text-left">
            <v-btn
              variant="outlined"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
            >
              Wstecz
            </v-btn>
          </v-col>
          <v-col justify="center" class="text-right">
            <v-btn
              xs="12"
              sm="6"
              md="3"
              align-self="center"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
            >
              Potwierdź
            </v-btn>
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
