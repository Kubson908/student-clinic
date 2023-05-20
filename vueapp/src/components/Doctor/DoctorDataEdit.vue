<script setup lang="ts">
// Wojtek 
    import { ref } from "vue";
    const name = ref<string>("");
    const lastName = ref<string>("");
    const email = ref<string>("");
    const pesel = ref<string>("");
    const phone = ref<string>("");
    const birthDate = ref<string>("");
    //const specialization = ref<string>("");
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg mt-6">
    <v-card-item>
      <!-- <v-card-title font-size="32">Twoje dane</v-card-title> -->
      <p class="font-weight-bold text-h4">Dane lekarza</p>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent>
          <v-container>
              <v-row>
                  <v-col align="center">
                      <v-img height="200" src="https://st2.depositphotos.com/1010683/5848/i/950/depositphotos_58482379-stock-photo-male-asian-doctor.jpg">
                      </v-img>
                  </v-col>
              </v-row>
              <v-btn variant="text"
                     align-self="center"
                     size="small"
                     color="blue-darken-2"
                     class="mt-2 button">
                  Edytuj zdjęcie
              </v-btn>
              <v-card-text>
                  <v-form @submit.prevent="submit" ref="form">
                      <v-row >
                        <v-col>
                          <v-text-field type="input"
                                        label="Imie"
                                        v-model="name"
                                        variant="solo"
                                        color="blue-darken-2"
                                        required>
                          </v-text-field>
                        </v-col>
                        <v-col>
                          <v-text-field type="input"
                                        label="Nazwisko"
                                        v-model="lastName"
                                        variant="solo"
                                        color="blue-darken-2"
                                        required>
                          </v-text-field>
                        </v-col>
                      </v-row>
                      <v-row>
                        <v-col>
                          <v-text-field type="input"
                                        label="Pesel"
                                        v-model="pesel"
                                        variant="solo"
                                        color="blue-darken-2"
                                        required>
                          </v-text-field>
                        </v-col>
                        <v-col>
                          <v-text-field type="input"
                                        label="Email"
                                        v-model="email"
                                        variant="solo"
                                        color="blue-darken-2"
                                        required>
                          </v-text-field>
                        </v-col>
                      </v-row>
                      <v-row>
                        <v-col>
                          <v-text-field type="input"
                                        label="Telefon"
                                        v-model="phone"
                                        variant="solo"
                                        color="blue-darken-2"
                                        required>
                          </v-text-field>
                        </v-col>
                        <v-col>
                          <v-text-field type="input"
                                        label="Data urodzenia"
                                        v-model="birthDate"
                                        variant="solo"
                                        color="blue-darken-2"
                                        required>
                          </v-text-field>
                        </v-col>
                      </v-row>
                      <v-row>
                        <v-col>
                        <v-select label="Specjalizacja"
                        :items="['Internista', 'Pulmunolog', 'Okulista']">
                        </v-select>
                        </v-col>
                      </v-row>
                    </v-form>
                    </v-card-text>
          </v-container>

          <v-row justify="start">
              <v-col cols="auto" class="me-auto">
                  <v-sheet class="pa-2 ma-2">
                      <v-btn type="submit"
                             variant="outlined"
                             text="blue-darken"
                             color="blue-darken-2"
                             class="mt-2 button">
                          Wstecz
                      </v-btn>
                  </v-sheet>
              </v-col>
              <v-col cols="auto">
                  <v-sheet class="pa-2 ma-2">
                    <v-btn type="submit"
                             color="blue-darken-2"
                             class="mt-2 button">
                             Zapisz
                    </v-btn>
                  </v-sheet>
              <v-btn variant="text"
                     align-self="center"
                     size="small"
                     color="blue-darken-2"

                     class="button pa-0 ma-0">
                  Zmień hasło
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