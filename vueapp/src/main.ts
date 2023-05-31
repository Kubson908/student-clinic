import { createApp, reactive } from "vue";
import App from "./App.vue";
import "vuetify/styles";
import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import "@vuepic/vue-datepicker/dist/main.css";
import "@mdi/font/css/materialdesignicons.css";
import * as VueRouter from "vue-router";
import { routes } from "./router";
import { User, Snackbar } from "./typings";
import axios from "axios";
import { prefix } from "./config";
import * as labsComponents from "vuetify/labs/components";

export const user = reactive<User>({
  id: 0,
  name: localStorage.getItem("user") ?? "Niezalogowany",
  isLoggedIn: localStorage.getItem("user") ? true : false,
  roles: localStorage.getItem("roles")
    ? JSON.parse(localStorage.getItem("roles") ?? "")
    : [],
});

export const specializations: Array<{ value: number; title: string }> = [
  { value: 0, title: "Internista" },
  { value: 1, title: "Pulmonolog" },
  { value: 2, title: "Okulista" },
  { value: 3, title: "Gastrolog" },
];

export const spec: Array<string> = [
  "Internista",
  "Pulmonolog",
  "Okulista",
  "Gastrolog",
];

export const authorized = axios.create({
  headers: {
    Authorization: `Bearer ${localStorage.getItem("token")}`,
  },
  baseURL: `${prefix}/api`,
  timeout: 5000,
});

export const unauthorized = axios.create({
  baseURL: `${prefix}/api`,
  timeout: 5000,
});

export const snackbar = reactive<Snackbar>({
  showing: false,
  error: false,
  text: "",
});

declare module "vue-router" {
  interface RouteMeta {
    roles: Array<string> | null;
  }
}
export const router = VueRouter.createRouter({
  history: VueRouter.createWebHistory(),
  routes,
});

router.beforeEach((to) => {
  if (
    to.meta.roles != null &&
    !to.meta.roles.some((e) => user.roles.includes(e))
  ) {
    return {
      path: "/unauthorized",
      query: { redirect: to.fullPath },
    };
  }
});

const vuetify = createVuetify({
  components: {
    ...components,
    ...labsComponents,
  },
  directives: {
    ...directives,
  },
});
createApp(App).use(router).use(vuetify).mount("#app");
