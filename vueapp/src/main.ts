import { createApp } from "vue";
import App from "./App.vue";
import "vuetify/styles";
import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import "@mdi/font/css/materialdesignicons.css";
import * as VueRouter from "vue-router";
import { routes } from "./router";
import { User } from "./typings";

const router = VueRouter.createRouter({
  history: VueRouter.createWebHistory(),
  routes,
});

export const user: User = {
  id: 0,
  name: "Niezalogowany",
  surname: "",
};
const vuetify = createVuetify({
  components,
  directives,
});
createApp(App).use(router).use(vuetify).mount("#app");
