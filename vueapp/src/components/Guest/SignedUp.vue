<script setup lang="ts">
import { unauthorized, snackbar, router, user } from "@/main";
import { ref } from "vue";
const code = ref<string>("");
// eslint-disable-next-line
const props = defineProps({
  email_to_confirm: String,
  password: String,
  remember_me: Boolean,
});
const codeRules = [
  (value: string) => {
    if (value.length == 6) return true;
    else return "Podaj kod";
  },
];
const loading = ref<boolean>(false);

const verify = async () => {
  if (code.value.length != 6) return;
  try {
    await unauthorized.post("/auth/confirm-email", {
      email: props.email_to_confirm,
      token: code.value,
    });
    snackbar.text = "Zweryfikowano adres email";
    snackbar.error = false;
    if (router.currentRoute.value.path === "/login") {
      loading.value = true;
      const res = await unauthorized.post("/auth/login", {
        email: props.email_to_confirm,
        password: props.password,
      });

      await localStorage.setItem("token", res.data.accessToken);
      if (props.remember_me)
        localStorage.setItem("expireDate", res.data.expireDate);
      else {
        let time = new Date(Date.now());
        time.setTime(time.getTime() + 60 * 60 * 1000);
        localStorage.setItem("expireDate", time.toString());
      }
      localStorage.setItem("user", res.data.user);
      localStorage.setItem("roles", JSON.stringify(res.data.roles));
      user.name = res.data.user;
      user.isLoggedIn = true;
      user.roles = res.data.roles;
      router.push("/");
    } else router.push("/login");
    snackbar.showing = true;
  } catch (e) {
    console.log(e);
    snackbar.error = true;
    snackbar.text = "Błąd weryfikacji";
    loading.value = false;
  } finally {
    snackbar.showing = true;
    loading.value = false;
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
  <v-card :loading="loading">
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
