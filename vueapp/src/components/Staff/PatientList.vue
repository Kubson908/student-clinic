<script setup lang="ts">
import { ref } from "vue";
import { computed, onBeforeMount } from "vue";
import { authorized, user, router, snackbar } from "@/main";
const search = ref<string>("");
const patients = ref<any>(null);
const loading = ref<boolean>(true);
onBeforeMount(async () => {
  try {
    const card = await authorized.get("/patient/");
    patients.value = card.data.map((patient: any) => {
      return {
        id: patient.id,
        name: patient.firstName + " " + patient.lastName,
      };
    });
  } catch (e) {
    console.log(e);
    snackbar.text = "Wystąpił błąd podczas pobierania listy pacjentów";
    snackbar.error = true;
    snackbar.showing = true;
  } finally {
    loading.value = false;
  }
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
    <v-col cols="12" sm="9" md="6" align-self="center">
      <v-card elevation="5" class="rounded-lg">
        <template #loader>
          <v-progress-linear
            :active="loading"
            color="deep-purple"
            height="4"
            indeterminate
          ></v-progress-linear>
        </template>
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
          <v-row
            no-gutters
            v-if="filteredPatients.length === 0"
            class="justify-center temporary-height"
          >
            <v-col class="ma-4 text-grey">
              {{ loading ? "Ładowanie" : "Brak pacjentów" }}
            </v-col>
          </v-row>
          <TransitionGroup name="list">
          <v-virtual-scroll
            :items="filteredPatients"
            :height="filteredPatients.length === 0 ? '40vh' : '50vh'"
          >
            <template #default="{ item }">
              <v-list-item class="mx-8 my-4 pa-4 rounded-lg" elevation="3">
                <template #prepend>
                  <v-list-item-title class="mx-4">{{
                    (item as any).name
                  }}</v-list-item-title>
                </template>
                <template #append>
                  <v-row no-gutters v-if="checkRole('Staff')">
                    <router-link
                      :to="`/staff/patient/edit/${(item as any).id} `"
                      custom
                      v-slot="{ navigate }"
                    >
                      <v-btn
                        class="ma-2 button"
                        color="blue-darken-2"
                        :value="`/staff/patient/edit/${(item as any).id} `"
                        @click="navigate"
                        >Dane pacjenta</v-btn
                      >
                    </router-link>
                    <router-link
                      :to="'/staff/patient/' + (item as any).id + '/card'"
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
                      :to="'/doctor/patient/' + (item as any).id + '/card'"
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
            </template>
          </v-virtual-scroll>
        </TransitionGroup>
          <v-divider></v-divider>
        </v-card-text>
        <v-card-actions>
          <v-btn
            width="20%"
            variant="outlined"
            color="blue-darken-2"
            class="mt-2 mb-3 button mx-10"
            @click="router.back()"
            >Wstecz</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>
<style>
.temporary-height {
  height: 10vh !important;
}
</style>
