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
import { User } from "./typings";

const router = VueRouter.createRouter({
  history: VueRouter.createWebHistory(),
  routes: routes,
});

// router.beforeEach((to, from) => {
//   if (!user.loggedIn && to.name !== "Login") {
//     return { name: "Login" };
//   }
//   return false;
// });

export const user = reactive<User>({
  id: 0,
  name: localStorage.getItem("user") ?? "Niezalogowany",
  loggedIn: localStorage.getItem("user") ? true : false,
});
const vuetify = createVuetify({
  components,
  directives,
});
createApp(App).use(router).use(vuetify).mount("#app");
