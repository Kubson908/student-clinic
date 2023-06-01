<script setup lang="ts">
import { unauthorized, snackbar, router } from "@/main";
import { ref } from "vue";
const code = ref<string>("");
// eslint-disable-next-line
const props = defineProps({
  email_to_confirm: null,
});
const codeRules = [
  (value: string) => {
    if (value.length == 6) return true;
    else return "Podaj kod";
  },
];

const verify = async () => {
  if (code.value.length != 6) return;
  try {
    console.log(props.email_to_confirm);
    await unauthorized.post("/auth/confirm-email", {
      email: props.email_to_confirm,
      token: code.value,
    });
    snackbar.text = "Zweryfikowano adres email";
    snackbar.error = false;
    router.push("/login");
  } catch (e) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Błąd weryfikacji";
  } finally {
    snackbar.showing = true;
  }
};

const resend = async () => {
  try {
    await unauthorized.post("/auth/resend-email", {
      email: props.email_to_confirm,
      token: code.value,
    });
    snackbar.text = "Kod został wysłany";
    snackbar.error = false;
  } catch (e) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd";
  } finally {
    snackbar.showing = true;
  }
};
</script>

<template>
  <v-card>
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
        Na podany e-mail przesłano kod
      </v-card-title>
    </v-card-item>
    <v-card-text>
      <v-form>
        <v-row>
          <v-col>
            <v-text-field
              label="Kod weryfikacyjny"
              v-model="code"
              variant="solo"
              :rules="codeRules"
              class="px-3 py-1"
              height="10px"
              color="blue-darken-2"
              required
            >
            </v-text-field>
            <v-row>
              <v-col>
                <v-btn
                  color="blue-darken-2"
                  class="mt-2 button"
                  @click="verify"
                >
                  Zweryfikuj
                </v-btn>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-btn
                  variant="text"
                  size="small"
                  class="mt-2 button"
                  @click="resend"
                >
                  Wyślij kod ponownie
                </v-btn>
              </v-col>
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
