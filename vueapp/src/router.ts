import SignUp from "./components/SignUp.vue";
import AppointmentList from "./components/Doctor/AppointmentList.vue";
import LoginForm from "./components/LoginForm.vue";
import HomePage from "./components/HomePage.vue";
import { Component } from "vue";
import NewVisit from "./components/NewVisit.vue";
import VisitHarmonogram from "./components/VisitHarmonogram.vue";
import VisitDetails from "./components/VisitDetails.vue";
import VisitSummary from "./components/VisitSummary.vue";
import PatientCard from "./components/PatientCard.vue";
import DoctorData from "./components/DoctorData.vue";
import PatientData from "./components/PatientData.vue";
import PatientVisitSummary from "./components/PatientVisitSummary.vue";
import PatientCancelVisit from "./components/Patient/PatientCancelVisit.vue";
import PatientChangePassword from "./components/Patient/ChangePassword.vue";
import GuestPasswordReset2 from "./components/Guest/GuestPasswordReset2.vue";
import GuestPasswordReset from "./components/Guest/GuestPasswordReset.vue";

export const routes: Array<{ path: string; component: Component }> = [
  { path: "/", component: HomePage },
  { path: "/appointments", component: AppointmentList },
  { path: "/login", component: LoginForm },
  { path: "/signup", component: SignUp },
  { path: "/new_visit", component: NewVisit },
  { path: "/visit_harmonogram", component: VisitHarmonogram },
  { path: "/visit", component: VisitDetails },
  { path: "/visitsummary", component: VisitSummary },
  { path: "/patientcard", component: PatientCard },
  { path: "/doctordata", component: DoctorData },
  { path: "/patientdata", component: PatientData },
  { path: "/patientvisitsummary", component: PatientVisitSummary },
  { path: "/patient/cancelvisit", component: PatientCancelVisit },
  { path: "/patient/changepassword", component: PatientChangePassword },
  { path: "/guest/passwordreset2", component: GuestPasswordReset2 },
  { path: "/guest/passwordreset", component: GuestPasswordReset },
];
