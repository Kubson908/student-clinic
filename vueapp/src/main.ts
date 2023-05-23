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

export const user = reactive<User>({
  id: 0,
  name: localStorage.getItem("user") ?? "Niezalogowany",
  isLoggedIn: localStorage.getItem("user") ? true : false,
  role: localStorage.getItem("role") ?? undefined,
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
    !to.meta.roles.includes(localStorage.getItem("role") as string)
  ) {
    return {
      path: "/unauthorized",
      query: { redirect: to.fullPath },
    };
  }
});

const vuetify = createVuetify({
  components,
  directives,
});
createApp(App).use(router).use(vuetify).mount("#app");
