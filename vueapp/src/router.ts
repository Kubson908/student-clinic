import { Route } from "./typings";

import {
  AppointmentStatistics,
  AwaitingAppointments,
  StaffPasswordReset,
  PatientList,
  VisitAssign,
  PatientDataReceptionEdit,
  StaffView,
} from "./components/Staff";

import {
  PatientPasswordReset,
  NewVisit,
  PatientCancelVisit,
  PatientCard,
  PatientData,
  AppointmentList,
  PatientView,
} from "./components/Patient";

import { GuestPasswordReset, LoginForm, SignUp } from "./components/Guest";

import {
  VisitHarmonogram,
  DoctorVisit,
  DoctorVisitSummary,
  DoctorData,
  DoctorDataEdit,
  DoctorPage,
  DoctorPasswordReset,
  DoctorView,
} from "./components/Doctor";

import { VisitDetails, ChangePassword } from "./components/Shared";

import { HomePage, UnauthorizedView } from "./components";

export const routes: Array<Route> = [
  { path: "/", component: HomePage, meta: { roles: null } },
  { path: "/unauthorized", component: UnauthorizedView, meta: { roles: null } },
  { path: "/login", component: LoginForm, meta: { roles: null } }, // guest - Logowanie
  { path: "/signup", component: SignUp, meta: { roles: null } }, // guest - rejestracja
  {
    path: "/patient",
    component: PatientView,
    meta: { roles: ["Patient", "Employee"] },
    children: [
      {
        path: "appointments", // pacjent - Lista wizyt
        component: AppointmentList,
        meta: { roles: null },
      },
      { path: "appointments/new", component: NewVisit, meta: { roles: null } }, // pacjent - Nowa wizyta 2 / pacjent - Nowa wizyta 1 / pacjent - Podsumowanie wizyty
      {
        path: "card", // doctor - Karta pacjenta / pacjent - Karta pacjenta - pacjent
        component: PatientCard,
        meta: { roles: null },
      },
      {
        path: "profile", // pacjent - Dane pacjenta / Dane pacjenta - recepcjonista ???? jakies niedorobione
        component: PatientData,
        meta: { roles: null },
      },
      {
        path: "appointment/:id/cancel", // pacjent - Anuluj wizytę
        component: PatientCancelVisit,
        meta: { roles: null },
      },
      {
        path: "appointment/:id", // pacjent - Szczegóły wizyty - pacjent / pacjent - Szczegóły wizyty - pacjent 2
        component: VisitDetails,
        meta: { roles: null },
      },
      {
        path: "change-password", // pacjent - Zmień hasło
        component: ChangePassword,
        meta: { roles: null },
      },
      {
        path: "passwordreset", // guest - Reset hasła
        component: PatientPasswordReset,
        meta: { roles: null },
      },
    ],
  },
  {
    path: "/doctor",
    component: DoctorView,
    meta: { roles: ["Employee"] },
    children: [
      {
        path: "harmonogram", // doctor - Harmonogram wizyt
        component: VisitHarmonogram,
        meta: { roles: null },
      },
      {
        path: "appointment/:id/finish",
        component: DoctorVisit,
        meta: { roles: null },
      }, // doctor - Wizyta
      {
        path: "profile",
        component: DoctorData,
        meta: { roles: null },
      }, // doctor - Dane lekarza - lekarz
      {
        path: "passwordreset", // lekarz - Zmień hasło - lekarz / dorobic weryfikacje starego hasla
        component: DoctorPasswordReset,
        meta: { roles: null },
      },
      {
        path: "doctors",
        component: DoctorPage,
        meta: { roles: null },
      }, // recepcja - Panel admina - recepcja
      {
        path: "profile/edit", // recepcja - Dane lekarza - recepcjonista
        component: DoctorDataEdit,
        meta: { roles: null },
      },
      {
        path: "appointment/:id", // doctor - Szczegóły wizyty - z harmonogramu / doctor - Szczegóły wizyty (recipe)
        component: VisitDetails,
        meta: { roles: null },
      },
      {
        path: "patient/:id/card",
        component: PatientCard,
        meta: { roles: null },
      },
      {
        path: "patients", // recepcja - Pacjenci - recepcjonista
        component: PatientList,
        meta: { roles: ["Staf", "Employee"] },
      },
    ],
  },
  {
    path: "/staff",
    component: StaffView,
    meta: { roles: ["Staff"] },
    children: [
      {
        path: "appointments/:id", // doctor - Szczegóły wizyty - z harmonogramu / doctor - Szczegóły wizyty (recipe)
        component: VisitDetails,
        meta: { roles: null },
      },
      {
        path: "appointments/:id/assign", // recepcja - Przydziel wizytę(visitassign)
        component: VisitAssign,
        meta: { roles: null },
      },
      {
        path: "appointments/awaiting", // recepcja - Oczekujące wizyty
        component: AwaitingAppointments,
        meta: { roles: null },
      },
      {
        path: "statistics", // recepcja - Statystyka
        component: AppointmentStatistics,
        meta: { roles: null },
      },
      {
        path: "patients", // recepcja - Pacjenci - recepcjonista
        component: PatientList,
        meta: { roles: ["Staff", "Employee"] },
      },
      {
        path: "patient/edit/:id", // JAKIES MOCNO NIEDOROBIONE recepcja - Dane pacjenta - recepcjonista / pacjent - Dane pacjenta
        component: PatientDataReceptionEdit,
        meta: { roles: null },
      },
      {
        path: "passwordreset", // recepcja - Zmień hasło - recepcjonista
        component: StaffPasswordReset,
        meta: { roles: null },
      },
      {
        path: "patient/:id/card",
        component: PatientCard,
        meta: { roles: null },
      },
      {
        path: "doctors",
        component: DoctorPage,
        meta: { roles: null },
      },
      {
        path: "doctors/profile/:id",
        component: DoctorDataEdit,
        meta: { roles: null },
      }, // doctor - Dane lekarza - lekarz
      {
        path: "doctors/harmonogram/:id",
        component: VisitHarmonogram,
        meta: { roles: null },
      }, // doctor - harmonogram - lekarz
      {
        path: "profile",
        component: DoctorData,
        meta: { roles: null },
      }, // doctor - Dane lekarza - lekarz
    ],
  },
  {
    path: "/auth/password-reset", // guest - Podaj nowe hasło
    component: GuestPasswordReset,
    meta: { roles: null },
  },
];

// roles: ["Patient"]
// roles: null
// roles: [""]
