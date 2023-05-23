import SignUp from "./components/Guest/SignUp.vue";
import AppointmentList from "./components/Patient/AppointmentList.vue";
import LoginForm from "./components/Guest/LoginForm.vue";
import HomePage from "./components/HomePage.vue";
import { Component } from "vue";
import VisitHarmonogram from "./components/Doctor/VisitHarmonogram.vue";
import VisitDetails from "./components/Doctor/VisitDetails.vue";
import VisitSummary from "./components/Doctor/VisitSummary.vue";
import PatientCard from "./components/Patient/PatientCard.vue";
import DoctorData from "./components/Doctor/DoctorData.vue";
import PatientData from "./components/Patient/PatientData.vue";
import PatientCancelVisit from "./components/Patient/PatientCancelVisit.vue";
import PatientChangePassword from "./components/Patient/ChangePassword.vue";
import GuestPasswordReset2 from "./components/Guest/GuestPasswordReset2.vue";
import GuestPasswordReset from "./components/Guest/GuestPasswordReset.vue";
import DoctorPasswordReset from "./components/Doctor/DoctorPasswordReset.vue";
import UnauthorizedView from "./components/UnauthorizedView.vue";
import DoctorDataEdit from "./components/Doctor/DoctorDataEdit.vue";
import DoctorPage from "./components/Doctor/DoctorPage.vue";
import DoctorVisitDetails from "./components/Doctor/DoctorVisitDetails.vue";
import VisitAssign from "./components/Doctor/VisitAssign.vue";
import PatientDataReceptionEdit from "./components/Doctor/PatientDataReceptionEdit.vue";
import AwaitingAppointments from "./components/Staff/AwaitingAppointments.vue";
import AppointmentStatistics from "./components/Staff/AppointmentStatistics.vue";
import PatientList from "./components/Staff/PatientList.vue";
import NewVisit from "./components/Patient/NewVisit.vue";
import PatientPasswordReset from "./components/Patient/PasswordReset.vue";
import StaffPasswordReset from "./components/Staff/StaffPasswordReset.vue";

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
    path: "/doctor/visit_assign",
    component: VisitAssign,
    meta: { roles: null },
  },
  {
    path: "/doctor/patient_reception_edit",
    component: PatientDataReceptionEdit,
    meta: { roles: null },
  },
  {
    path: "/staff/awaitingappointments",
    component: AwaitingAppointments,
    meta: { roles: null },
  },
  {
    path: "/staff/statistics",
    component: AppointmentStatistics,
    meta: { roles: null },
  },
  {
    path: "/staff/patientlist",
    component: PatientList,
    meta: { roles: null },
  },
  {
    path: "/patient/passwordreset",
    component: PatientPasswordReset,
    meta: { roles: null },
  },
  {
    path: "/staff/passwordreset",
    component: StaffPasswordReset,
    meta: { roles: null },
  },
];

// roles: ["Patient"]
// roles: null
// roles: [""]
