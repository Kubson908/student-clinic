<script setup lang="ts">
import { ref } from "vue";
import { computed, onBeforeMount } from "vue";
import { authorized, user, router } from "@/main";
const search = ref<string>("");
const patients = ref<any>(null);
onBeforeMount(async () => {
  const card = await authorized.get("/patient/");
  patients.value = card.data.map((patient: any) => {
    return { id: patient.id, name: patient.firstName + " " + patient.lastName };
  });
});
const checkRole = (role: string) => {
  const roles = user.roles!;
  return roles.includes(role);
};
const filteredPatients = computed(() => {
  if (!patients.value) return [];
  return patients.value.filter((patient: any) => {
    return patient.name.toLowerCase().indexOf(search.value.toLowerCase()) != -1;
  });
});
</script>
<template>
  <v-row justify="center" no-gutters>
    <v-col cols="12" sm="6" md="6" align-self="center">
      <v-card elevation="5" class="rounded-lg">
        <v-card-item>
          <v-container class="d-flex justify-center align-center">
            <h1>Pacjenci</h1>
          </v-container>
          <v-row no-gutters class="justify-center text-center"
            ><v-col cols="9" class="d-flex justify-center"
              ><v-text-field
                label="Wyszukiwarka pacjentów"
                v-model="search"
                class="w-75"
                variant="solo"
                append-inner-icon="mdi-account-search"
                placeholder="Imię Nazwisko"
              ></v-text-field></v-col
          ></v-row>
        </v-card-item>
        <v-card-text>
          <v-divider></v-divider>
          <v-list height="50vh">
            <v-list-item
              v-for="(patient, idx) in filteredPatients"
              :key="idx"
              class="mx-8 my-4 pa-4 rounded-lg"
              elevation="3"
            >
              <template #prepend>
                <v-list-item-title class="mx-4">{{
                  patient.name
                }}</v-list-item-title>
              </template>
              <template #append>
                <v-row no-gutters v-if="checkRole('Staff')">
                  
                  <router-link
                    :to="`/staff/patient/edit/${patient.id} `"
                    custom
                    v-slot="{ navigate }"
                  >
                  <v-btn class="ma-2 button" color="blue-darken-2"
                  :value="`/staff/patient/edit/${patient.id} `"
                  @click="navigate"
                    >Dane pacjenta</v-btn
                  >
                  </router-link>
                  <router-link
                    :to="'/staff/patient/' + patient.id + '/card'"
                    custom
                    v-slot="{ navigate }"
                  >
                    <v-btn
                      variant="outlined"
                      class="ma-2 button hp-bright"
                      @click="navigate"
                      >Karta pacjenta</v-btn
                    >
                  </router-link>
                </v-row>
                <v-row no-gutters v-else-if="checkRole('Employee')">
                  <router-link
                    :to="'/doctor/patient/' + patient.id + '/card'"
                    custom
                    v-slot="{ navigate }"
                  >
                    <v-btn
                      class="ma-2 button"
                      color="blue-darken-2"
                      @click="navigate"
                      >Karta pacjenta
                    </v-btn> 
                  </router-link>
                </v-row>
              </template>
            </v-list-item>
          </v-list>
          <v-divider></v-divider>
        </v-card-text>
        <v-card-actions>
          <v-btn
            variant="outlined"
            color="blue-darken-2"
            class="mx-8 my-2"
            @click="router.back()"
            >Wstecz</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>
<style></style>
