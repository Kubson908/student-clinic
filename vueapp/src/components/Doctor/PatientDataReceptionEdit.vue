<script setup lang="ts">
// Wojtek 
    import { ref } from "vue";
    const name = ref<string>("");
    const lastName = ref<string>("");
    const email = ref<string>("");
    const pesel = ref<string>("");
    const phone = ref<string>("");
    const birthDate = ref<string>("");
    const medicinec = ref<string>("");
    const allergiest = ref<string>("");
</script>

<template>
  <v-card width="560px" location="center" elevation="5" class="rounded-lg mt-6">
    <v-card-item>
      <!-- <v-card-title font-size="32">Twoje dane</v-card-title> -->
      <p class="font-weight-bold text-h4">Dane pacjenta</p>
      <v-card-subtitle>Edytuj dane pacjęta</v-card-subtitle>
    </v-card-item>
    <v-spacer></v-spacer>
    <v-card-text>
      <v-form @submit.prevent>
          <v-container>
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
                          <v-text-field type="input"
                                        label="Przyjmowane leki"
                                        v-model="medicinec"
                                        variant="solo"
                                        color="blue-darken-2"
                                        required>
                          </v-text-field>
                        </v-col>
                      </v-row>
                      <v-row>
                        <v-col>
                          <v-text-field type="input"
                                        label="Alergie"
                                        v-model="allergiest"
                                        variant="solo"
                                        color="blue-darken-2"
                                        required>
                          </v-text-field>
                        </v-col>
                      </v-row>
                    </v-form>
                    </v-card-text>
                    <v-row>
                        <v-col>
                        <v-checkbox
                          v-model="terms"
                          color="blue-darken-2"
                          label="Zweryfikuj"
                        ></v-checkbox>
                        </v-col>
                      </v-row>
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