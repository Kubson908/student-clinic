<script lang="ts" setup>

</script>

<template>
    <v-card width="560px" location="center" elevation="5" class="rounded-lg">
        <v-card-item>
            <v-container class="d-flex justify-center align-center">
                <v-card height="64" width="64" color="#36BFF1" class="d-flex justify-center align-center">
                    <v-icon icon="mdi-hospital-building" size="48" color="white"></v-icon>
                </v-card>
            </v-container>
            <v-card-title font-size="56">Nowa wizyta</v-card-title>
            <v-card-subtitle>Wpisz szczegóły</v-card-subtitle>
        </v-card-item>
        <v-spacer></v-spacer>
        <v-card-text>
            <v-form @submit.prevent>
                <v-select label="Wybierz specjalistów"
                          :items="['Internista', 'Pulmunolog', 'Okulista']"></v-select>
                <v-textarea label="Objawy"></v-textarea>
                <v-textarea label="Stosowane leki"></v-textarea>
                <v-row justify="center"> 
                    <v-col xs="12" sm="6" md="3" align-self="center" class="text-left">
                        <v-btn variant="outlined" size="large" class="mt-2 button" color="blue-darken-2">
                            Wstecz
                        </v-btn>
                    </v-col>
                    <v-col justify="center" class="text-right">
                        <v-btn xs="12" sm="6" md="3" align-self="center" size="large" class="mt-2 button" color="blue-darken-2">
                            Dalej
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