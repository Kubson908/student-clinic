import { Route } from "./typings";

import {
  AppointmentStatistics,
  AwaitingAppointments,
  StaffPasswordReset,
  PatientList,
} from "./components/Staff";
import {
  PatientPasswordReset,
  NewVisit,
  PatientChangePassword,
  PatientCancelVisit,
  PatientCard,
  PatientData,
  AppointmentList,
  PatientView,
} from "./components/Patient";
import {
  GuestPasswordReset,
  GuestPasswordReset2,
  LoginForm,
  SignUp,
} from "./components/Guest";
import {
  VisitHarmonogram,
  VisitDetails,
  VisitSummary,
  DoctorData,
  DoctorDataEdit,
  DoctorPage,
  PatientDataReceptionEdit,
  DoctorPasswordReset,
  DoctorVisitDetails,
  VisitAssign,
} from "./components/Doctor";
import { HomePage, UnauthorizedView } from "./components";

export const routes: Array<Route> = [
  { path: "/", component: HomePage, meta: { roles: null } },
  { path: "/unauthorized", component: UnauthorizedView, meta: { roles: null } },
  {
    path: "/patient/",
    component: PatientView,
    meta: { roles: ["Patient", "Employee"] },
    children: [
      {
        path: "appointments",
        component: AppointmentList,
        meta: { roles: null },
      },
      { path: "new_visit", component: NewVisit, meta: { roles: null } },
      {
        path: "patientcard",
        component: PatientCard,
        meta: { roles: null },
      },
      {
        path: "patientdata",
        component: PatientData,
        meta: { roles: null },
      },

      {
        path: "cancelvisit",
        component: PatientCancelVisit,
        meta: { roles: null },
      },
      {
        path: "changepassword",
        component: PatientChangePassword,
        meta: { roles: null },
      },
      {
        path: "passwordreset",
        component: PatientPasswordReset,
        meta: { roles: null },
      },
    ],
  },
  { path: "/login", component: LoginForm, meta: { roles: null } },
  { path: "/signup", component: SignUp, meta: { roles: null } },
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
    path: "/staff/appointmentstatistics",
    component: AppointmentStatistics,
    meta: { roles: null },
  },
  {
    path: "/staff/patientlist",
    component: PatientList,
    meta: { roles: ["Staf", "Employee"] },
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
