<script setup lang="ts">
// eslint-disable-next-line
defineProps<{
  img: string;
  doctor: { name: string; profession: string };
  disabled: boolean;
}>();
</script>

<template>
  <v-card width="288" height="192" class="round-lg" :class="disabled">
    <v-row no-gutters class="fill-height">
      <v-col cols="6">
        <v-img cover class="fill-height" :src="img"></v-img>
      </v-col>
      <v-col cols="6" class="d-flex flex-column">
        <v-card-item class="text-left pa-2">
          <v-card-title class="text-subtitle-2 py-0 my-0">
            {{ doctor.profession }}
          </v-card-title>
          <v-card-subtitle class="text-caption py-0 my-0">
            {{ doctor.name }}
          </v-card-subtitle>
        </v-card-item>
        <v-card-actions
          class="d-flex flex-column align-end mt-auto"
          align-self="bottom"
        >
          <v-btn
            size="small"
            color="blue-darken-2"
            variant="outlined"
            class="text-body-2"
            >Profil</v-btn
          >
          <v-btn
            size="small"
            color="blue-darken-2"
            variant="elevated"
            class="text-body-2 mt-2"
            >Harmonogram</v-btn
          >
        </v-card-actions>
      </v-col>
    </v-row>
  </v-card>
</template>

<style>
.doctor-disabled {
  filter: brightness(0.75);
  user-select: None;
}
</style>
