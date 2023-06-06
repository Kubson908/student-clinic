<script setup lang="ts">
import { authorized, snackbar } from "@/main";
import { ref, watch } from "vue";
import { notNull } from "@/validation";
import axios from "axios";
// eslint-disable-next-line
const props = defineProps({
  id: String,
});
// eslint-disable-next-line
const emit = defineEmits(["back", "reload"]);
const file = ref<File[]>([]);
// eslint-disable-next-line
const urlFile = ref<File>(new File(["" as BlobPart], "Plik"));
const url = ref<string>("");
const src = ref<string>("");
watch(file, () => {
  if (file.value.length !== 0) src.value = URL.createObjectURL(file.value[0]);
  urlFile.value = file.value[0];
});
watch(urlFile, () => {
  if (file.value.length !== 0) src.value = URL.createObjectURL(file.value[0]);
  file.value[0] = urlFile.value;
});
watch(url, async () => {
  await load();
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
  } catch (error: any) {
    console.log(error);
    snackbar.error = true;
    snackbar.text = "Wystąpił błąd przy wysyłaniu pliku";
  } finally {
    snackbar.showing = true;
  }
};
const load = async () => {
  try {
    const res = await axios.get(url.value, {
      responseType: "arraybuffer",
    });
    (document.getElementById("fileInput") as HTMLInputElement).value = "";
    // eslint-disable-next-line
    urlFile.value = new File([res.data as BlobPart], "Plik");
    file.value[0] = urlFile.value;
  } catch (error: any) {
    console.log(error);
    snackbar.error = true;
    snackbar.text = "Błędny adres zdjęcia";
    snackbar.showing = true;
  }
};
</script>

<template>
  <v-card elevation="5" class="rounded-lg w-50">
    <v-form @submit.prevent>
      <v-file-input
        id="fileInput"
        v-model="file"
        label="Prześlij zdjęcie"
        class="mx-8 mt-8"
        accept="image/*"
        show-size
        :rules="notNull"
        prepend-icon="mdi-camera"
        variant="outlined"
      ></v-file-input>
      <v-text-field
        class="mx-8"
        label="lub podaj link do zdjęcia"
        variant="outlined"
        v-model="url"
        prepend-icon="mdi-search-web"
        @click:prepend="load()"
      >
      </v-text-field>
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
