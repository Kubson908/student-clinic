import SignUp from "./components/Guest/SignUp.vue";
import AppointmentList from "./components/Patient/AppointmentList.vue";
import LoginForm from "./components/Guest/LoginForm.vue";
import HomePage from "./components/HomePage.vue";
import { Component } from "vue";
import NewVisit from "./components/Patient/NewVisit.vue";
import VisitHarmonogram from "./components/Doctor/VisitHarmonogram.vue";
import VisitDetails from "./components/Doctor/VisitDetails.vue";
import VisitSummary from "./components/Doctor/VisitSummary.vue";
import PatientCard from "./components/Patient/PatientCard.vue";
import DoctorData from "./components/Doctor/DoctorData.vue";
import PatientData from "./components/Patient/PatientData.vue";
import PatientVisitSummary from "./components/Patient/PatientVisitSummary.vue";
import PatientCancelVisit from "./components/Patient/PatientCancelVisit.vue";
import PatientChangePassword from "./components/Patient/ChangePassword.vue";
import GuestPasswordReset2 from "./components/Guest/GuestPasswordReset2.vue";
import GuestPasswordReset from "./components/Guest/GuestPasswordReset.vue";
import DoctorPasswordReset from "./components/Doctor/DoctorPasswordReset.vue";
import UnauthorizedView from "./components/UnauthorizedView.vue";
import DoctorDataEdit from "./components/Doctor/DoctorDataEdit.vue";
import DoctorPage from "./components/Doctor/DoctorPage.vue";
import DoctorVisitDetails from "./components/Doctor/DoctorVisitDetails.vue";
import PatientNewVisit from "./components/Patient/PatientNewVisit.vue";
import VisitAssign from "./components/Doctor/VisitAssign.vue";

export const routes: Array<{
  path: string;
  component: Component;
  meta: { roles: Array<string> | null };
}> = [
  { path: "/", component: HomePage, meta: { roles: null } },
  { path: "/unauthorized", component: UnauthorizedView, meta: { roles: null } },
  {
    path: "/patient/appointments",
    component: AppointmentList,
    meta: { roles: null },
  },
  { path: "/login", component: LoginForm, meta: { roles: null } },
  { path: "/signup", component: SignUp, meta: { roles: null } },
<<<<<<< Updated upstream
  { path: "/new_visit", component: NewVisit, meta: { roles: ["Patient"] } },
  { path: "/visit_harmonogram", component: VisitHarmonogram, meta: { roles: ["Patient"] } },
  { path: "/visit", component: VisitDetails, meta: { roles: null } },
  { path: "/visitsummary", component: VisitSummary, meta: { roles: [""] } },
  { path: "/patientcard", component: PatientCard, meta: { roles: null } },
  { path: "/doctordata", component: DoctorData, meta: { roles: null } },
  { path: "/doctordata/passwordreset", component: DoctorPasswordReset, meta: { roles: [""] } },
  { path: "/doctor", component: DoctorPage, meta: { roles: null } },
  { path: "/doctor/doctordataedit", component: DoctorDataEdit, meta: { roles: null } },
  { path: "/patientdata", component: PatientData, meta: { roles: [""] } },
  { path: "/patientvisitsummary", component: PatientVisitSummary, meta: { roles: null } },
  { path: "/patient/cancelvisit", component: PatientCancelVisit, meta: { roles: [""] } },
  { path: "/patient/changepassword", component: PatientChangePassword, meta: { roles: [""] } },
  { path: "/guest/passwordreset2", component: GuestPasswordReset2, meta: { roles: [""] } },
  { path: "/guest/passwordreset", component: GuestPasswordReset, meta: { roles: [""] } },
  { path: "/doctor/doctorvisitdetails", component: DoctorVisitDetails, meta: { roles: null } },
  { path: "/patient/patientnewvisit", component: PatientNewVisit, meta: { roles: null } },
  { path: "/visit_assign", component: VisitAssign, meta: { roles: null } },
=======
  { path: "/patient/new_visit", component: NewVisit, meta: { roles: null } },
  {
    path: "/doctor/visit_harmonogram",
    component: VisitHarmonogram,
    meta: { roles: null },
  },
  { path: "/doctor/visit", component: VisitDetails, meta: { roles: null } },
  {
    path: "/doctor/visitsummary",
    component: VisitSummary,
    meta: { roles: null },
  },
  {
    path: "/patient/patientcard",
    component: PatientCard,
    meta: { roles: null },
  },
  { path: "/doctor/doctordata", component: DoctorData, meta: { roles: null } },
  {
    path: "/doctordata/passwordreset",
    component: DoctorPasswordReset,
    meta: { roles: null },
  },
  { path: "/doctor/doctorpage", component: DoctorPage, meta: { roles: null } },
  {
    path: "/doctor/doctordataedit",
    component: DoctorDataEdit,
    meta: { roles: null },
  },
  {
    path: "/patient/patientdata",
    component: PatientData,
    meta: { roles: null },
  },
  {
    path: "/patient/patientvisitsummary",
    component: PatientVisitSummary,
    meta: { roles: null },
  },
  {
    path: "/patient/cancelvisit",
    component: PatientCancelVisit,
    meta: { roles: null },
  },
  {
    path: "/patient/changepassword",
    component: PatientChangePassword,
    meta: { roles: null },
  },
  {
    path: "/guest/passwordreset2",
    component: GuestPasswordReset2,
    meta: { roles: null },
  },
  {
    path: "/guest/passwordreset",
    component: GuestPasswordReset,
    meta: { roles: null },
  },
  {
    path: "/visitdetails",
    component: DoctorVisitDetails,
    meta: { roles: null },
  },
  {
    path: "/patient/patientnewvisit",
    component: PatientNewVisit,
    meta: { roles: null },
  },
  {
    path: "/doctor/visit_assign",
    component: VisitAssign,
    meta: { roles: null },
  },
  {
    path: "/doctor/patient_reception_edit",
    component: PatientDataReceptionEdit,
    meta: { roles: null },
  },
>>>>>>> Stashed changes
];

// roles: ["Patient"]
// roles: null
// roles: [""]
