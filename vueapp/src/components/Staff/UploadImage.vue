<script setup lang="ts">
import { authorized, snackbar } from "@/main";
import { ref, watch } from "vue";
import { notNull } from "@/validation";
// eslint-disable-next-line
const props = defineProps({
  id: String,
});
// eslint-disable-next-line
const emit = defineEmits(["back", "reload"]);
const file = ref<File[]>();
const src = ref<string>("");
watch(file, () => {
  if (file.value!.length !== 0) src.value = URL.createObjectURL(file.value![0]);
});
const upload = async () => {
  let formData = new FormData();
  formData.append("postedFile", file.value![0]);
  try {
    await authorized.post(`/employee/upload-image/${props.id}`, formData);
    snackbar.text = "Pomyślnie przesłano plik";
    snackbar.error = false;
    emit("reload");
    emit("back");
    // window.location.reload();
  } catch (error: any) {
    console.log(error);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd przy wysyłaniu pliku";
  } finally {
    snackbar.showing = true;
  }
};
</script>

<template>
  <v-card elevation="5" class="rounded-lg w-50">
    <v-form>
      <v-file-input
        v-model="file"
        label="Prześlij zdjęcie"
        class="ma-8"
        accept="image/*"
        show-size
        :rules="notNull"
        prepend-icon="mdi-camera"
        variant="outlined"
      ></v-file-input>
      <v-row no-gutters class="justify-center">
        <v-col
          cols="12"
          sm="10"
          md="8"
          class="text-grey-darken-3 font-weight-bold"
        >
          Podgląd zdjęcia:
          <v-card
            elevation="4"
            max-height="50vh"
            min-height="50vh"
            class="rounded-lg overflow-auto mt-1"
          >
            <v-img :src="src" class="ma-8 rounded-lg" />
          </v-card>
        </v-col>
      </v-row>
      <v-row no-gutters class="py-4">
        <v-col class="d-flex justify-space-between">
          <v-btn
            variant="outlined"
            class="mx-8 my-2 hp-bright button"
            @click="$emit('back')"
            >Wstecz</v-btn
          >
          <v-btn class="mx-8 my-2 hp-dark button" @click="upload()"
            >Prześlij</v-btn
          >
        </v-col>
      </v-row>
    </v-form>
  </v-card>
</template>

<style>
.button {
  text-transform: unset !important;
}
</style>
