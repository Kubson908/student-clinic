import SignUp from "./components/SignUp.vue";
import AppointmentList from "./components/AppointmentList.vue";
import LoginForm from "./components/LoginForm.vue";
import HomePage from "./components/HomePage.vue";
import { Component } from "vue";
import NewVisit from "./components/NewVisit.vue";

export const routes: Array<{ path: string; component: Component }> = [
    { path: "/", component: HomePage },
    { path: "/wizyty", component: AppointmentList },
    { path: "/login", component: LoginForm },
    { path: "/signup", component: SignUp },
    { path: "/nowa_wizyta", component: NewVisit },
  ];