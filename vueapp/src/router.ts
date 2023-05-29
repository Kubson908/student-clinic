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
        path: "appointments", // pacjent - Lista wizyt
        component: AppointmentList,
        meta: { roles: null },
      },
      { path: "new_visit", component: NewVisit, meta: { roles: null } }, // pacjent - Nowa wizyta 2 / pacjent - Nowa wizyta 1 / pacjent - Podsumowanie wizyty
      {
        path: "patientcard", // doctor - Karta pacjenta / pacjent - Karta pacjenta - pacjent
        component: PatientCard,
        meta: { roles: null },
      },
      {
        path: "patientdata", // pacjent - Dane pacjenta / Dane pacjenta - recepcjonista ???? jakies niedorobione
        component: PatientData,
        meta: { roles: null },
      },

      {
        path: "appointments/:id/cancel", // pacjent - Anuluj wizytę
        component: PatientCancelVisit,
        meta: { roles: null },
      },
      {
        path: "changepassword", // pacjent - Zmień hasło
        component: PatientChangePassword,
        meta: { roles: null },
      },
      {
        path: "passwordreset", // guest - Reset hasła
        component: PatientPasswordReset,
        meta: { roles: null },
      },
    ],
  },
  { path: "/login", component: LoginForm, meta: { roles: null } }, // guest - Logowanie
  { path: "/signup", component: SignUp, meta: { roles: null } }, // guest - rejestracja
  {
    path: "/doctor/visit_harmonogram", // doctor - Harmonogram wizyt
    component: VisitHarmonogram,
    meta: { roles: null },
  },
  { path: "/doctor/visit", component: VisitDetails, meta: { roles: null } }, // doctor - Wizyta
  {
    path: "/doctor/visitsummary", // doctor - Podsumowanie wizyty - lekarz
    component: VisitSummary,
    meta: { roles: null },
  },
  { path: "/doctor/doctordata", component: DoctorData, meta: { roles: null } }, // doctor - Dane lekarza - lekarz
  {
    path: "/doctordata/passwordreset", // JAKIS NIEDOROBIONY KLON(StaffPasswordReset) recepcja - Zmień hasło - recepcjonista
    component: DoctorPasswordReset,
    meta: { roles: null },
  },
  { path: "/doctor/doctorpage", component: DoctorPage, meta: { roles: null } }, // recepcja - Panel admina - recepcja
  {
    path: "/doctor/doctordataedit", // recepcja - Dane lekarza - recepcjonista
    component: DoctorDataEdit,
    meta: { roles: null },
  },
  {
    path: "/guest/passwordreset2", // guest - Podaj nowe hasło
    component: GuestPasswordReset2,
    meta: { roles: null },
  },
  {
    path: "/guest/passwordreset", // guest - Potwierdzenie przypomnienia
    component: GuestPasswordReset,
    meta: { roles: null },
  },
  {
    path: "/visitdetails", // doctor - Szczegóły wizyty - z harmonogramu / doctor - Szczegóły wizyty (recipe) / pacjent - Szczegóły wizyty - pacjent / pacjent - Szczegóły wizyty - pacjent 2
    component: DoctorVisitDetails,
    meta: { roles: null },
  },
  {
    path: "/doctor/visit_assign", // recepcja - Przydziel wizytę(visitassign)
    component: VisitAssign,
    meta: { roles: null },
  },
  {
    path: "/doctor/patient_reception_edit", // JAKIES MOCNO NIEDOROBIONE recepcja - Dane pacjenta - recepcjonista / pacjent - Dane pacjenta
    component: PatientDataReceptionEdit,
    meta: { roles: null },
  },
  {
    path: "/staff/awaitingappointments", // recepcja - Oczekujące wizyty
    component: AwaitingAppointments,
    meta: { roles: null },
  },
  {
    path: "/staff/appointmentstatistics", // recepcja - Statystyka
    component: AppointmentStatistics,
    meta: { roles: null },
  },
  {
    path: "/staff/patientlist", // recepcja - Pacjenci - recepcjonista
    component: PatientList,
    meta: { roles: ["Staf", "Employee"] },
  },
  {
    path: "/staff/passwordreset", // recepcja - Zmień hasło - recepcjonista
    component: StaffPasswordReset,
    meta: { roles: null },
  },
];

// roles: ["Patient"]
// roles: null
// roles: [""]
