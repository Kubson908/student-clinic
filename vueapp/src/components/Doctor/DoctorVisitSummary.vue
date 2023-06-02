<script setup lang="ts">
// eslint-disable-next-line
const emit = defineEmits(["page", "submit"]);
const change_page = (arg: number) => {
  emit("page", arg);
};

const submit = () => {
  emit("submit");
};
// eslint-disable-next-line
const props = defineProps({
  data: Object,
});
console.log(props.data);
console.log(props.data?.select);
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
      <v-card-title font-size="56">Podsumowanie wizyty</v-card-title>
      <v-card-subtitle>Sprawdź i zatwierdź dane</v-card-subtitle>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent>
        <v-container>
          <v-row>
            <p class="font-weight-bold">Informacje o wizycie</p>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Informacje o wizycie
            </v-col>
            <v-col class="text-left">
              {{ new Date(props.data?.appointmentDate).toLocaleDateString() }},
              {{
                new Date(props.data?.appointmentDate)
                  .toLocaleTimeString()
                  .substring(0, 5)
              }}
            </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Pacjent
            </v-col>
            <v-col class="text-left"> {{ props.data?.patientName }} </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              Podane objawy
            </v-col>
            <v-col class="text-left"> {{ props.data?.symptoms ? props.data?.symptoms : "Nie podano" }} </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Przyjmowane leki
            </v-col>
            <v-col class="text-left"> {{ props.data?.medicines ? props.data?.medicines: "Nie podano" }} </v-col>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-row>
            <p class="font-weight-bold">Podane informacje</p>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Diagnoza
            </v-col>
            <v-col class="text-left">
              <!-- Pacjent choruje na bakteryjne zapalenie zatok. -->
              {{ props.data?.diagnose ? props.data?.diagnose : "Nie podano" }}
            </v-col>
          </v-row>
          <v-row>
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Leki
            </v-col>
            <v-col class="text-left"> {{ props.data?.meds ? props.data?.meds : "Nie podano" }}</v-col>
          </v-row>
          <v-row>
            <v-col
              cols="4"
              class="font-weight-bold text-blue-darken-1 text-left"
            >
              Zalecenia
            </v-col>
            <v-col class="text-left">
              {{ props.data?.recomendations ? props.data?.recomendations : "Nie podano" }}
            </v-col>
          </v-row>
          <v-row v-if="props.data?.dateHour && props.data.dateDay && props.data?.controlVisit">
            <v-col
              class="font-weight-bold text-blue-darken-1 text-left"
              cols="4"
            >
              Wizyta kontrolna
            </v-col>
            <!-- 31.01.2023, 17:30 -->
            <v-col class="text-left">
              {{ `${props.data?.dateDay.toLocaleDateString()}, ${props.data?.dateHour}` }}
            </v-col>
          </v-row>
          <v-row><v-divider></v-divider></v-row>
        </v-container>

        <v-row justify="center">
          <v-col xs="12" sm="6" md="3" align-self="center" class="text-left">
            <v-btn
              variant="outlined"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
              @click="change_page(props.data?.controlVisit ? -1 : -2)"
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
              @click="submit()"
              size="large"
              class="mt-2 button"
              color="blue-darken-2"
            >
              Zatwierdź
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
