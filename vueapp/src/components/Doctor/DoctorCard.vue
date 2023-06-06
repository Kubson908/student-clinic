<script setup lang="ts">
import { specializations } from "@/main";
// eslint-disable-next-line
const props = defineProps<{
  img: string;
  doctor: any;
  disabled: boolean;
}>();
</script>

<template>
  <v-card width="288" height="192" class="round-lg" :class="disabled" elevation="4">
    <v-row no-gutters class="fill-height">
      <v-col cols="6" class="fill-height max-height">
        <v-img cover class="fill-height max-height" :src="img" />
      </v-col>
      <v-col cols="6" class="d-flex flex-column">
        <v-card-item class="text-left pa-2">
          <v-card-title class="text-subtitle-2 py-0 my-0">
            {{
              specializations.find(
                (specialization) =>
                  specialization.value === props.doctor.specialization
              )?.title
            }}
          </v-card-title>
          <v-card-subtitle class="text-caption py-0 my-0">
            {{ props.doctor.firstName }} {{ props.doctor.lastName }}
          </v-card-subtitle>
        </v-card-item>
        <v-card-actions
          class="d-flex flex-column align-end mt-auto"
          align-self="bottom"
        >
          <router-link
            :to="`/staff/doctors/profile/${doctor.id}`"
            custom
            v-slot="{ navigate }"
          >
            <v-btn
              size="small"
              color="blue-darken-2"
              variant="outlined"
              class="text-body-2"
              :value="`staff/doctors/profile/${doctor.id}`"
              @click="navigate"
              >Profil</v-btn
            >
          </router-link>
          <router-link
            :to="`/staff/doctors/harmonogram/${doctor.id}`"
            custom
            v-slot="{ navigate }"
          >
            <v-btn
              size="small"
              color="blue-darken-2"
              variant="elevated"
              class="text-body-2 mt-2"
              :value="`staff/doctors/harmonogram/${doctor.id}`"
              @click="navigate"
              >Harmonogram</v-btn
            >
          </router-link>
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
.max-img-height {
  max-height: 192px !important;
  min-height: 192px !important;
}
</style>
