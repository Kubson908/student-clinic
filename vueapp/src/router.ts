import { Route } from "./typings";

import {
  AppointmentStatistics,
  AwaitingAppointments,
  StaffPasswordReset,
  PatientList,
  VisitAssign,
  PatientDataReceptionEdit,
  StaffView,
  DoctorDataEdit,
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
  DoctorPage,
  DoctorView,
} from "./components/Doctor";

import {
  VisitDetails,
  ChangePassword,
  EmployeeData,
} from "./components/Shared";

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
        meta: { roles: ["Patient", "Employee"] },
      },
      { path: "appointments/new", component: NewVisit, meta: { roles: null } }, // pacjent - Nowa wizyta 2 / pacjent - Nowa wizyta 1 / pacjent - Podsumowanie wizyty
      {
        path: "card", // doctor - Karta pacjenta / pacjent - Karta pacjenta - pacjent
        component: PatientCard,
        meta: { roles: ["Patient", "Employee"] },
      },
      {
        path: "profile", // pacjent - Dane pacjenta / Dane pacjenta - recepcjonista
        component: PatientData,
        meta: { roles: ["Patient", "Employee"] },
      },
      {
        path: "appointment/:id/cancel", // pacjent - Anuluj wizytę
        component: PatientCancelVisit,
        meta: { roles: ["Patient", "Employee"] },
      },
      {
        path: "appointment/:id", // pacjent - Szczegóły wizyty - pacjent / pacjent - Szczegóły wizyty - pacjent 2
        component: VisitDetails,
        meta: { roles: ["Patient", "Employee"] },
      },
      {
        path: "change-password", // pacjent - Zmień hasło
        component: ChangePassword,
        meta: { roles: ["Patient", "Employee"] },
      },
      {
        path: "passwordreset", // guest - Reset hasła
        component: PatientPasswordReset,
        meta: { roles: ["Patient", "Employee"] },
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
        meta: { roles: ["Employee"] },
      },
      {
        path: "appointment/:id/finish",
        component: DoctorVisit,
        meta: { roles: ["Employee"] },
      }, // doctor - Wizyta
      {
        path: "profile",
        component: EmployeeData,
        meta: { roles: ["Employee"] },
      }, // doctor - Dane lekarza - lekarz
      {
        path: "change-password", // lekarz - Zmień hasło - lekarz / dorobic weryfikacje starego hasla
        component: ChangePassword,
        meta: { roles: ["Employee"] },
      },
      {
        path: "doctors",
        component: DoctorPage,
        meta: { roles: ["Employee"] },
      }, // recepcja - Panel admina - recepcja
      // {
      //   path: "profile/edit", // recepcja - Dane lekarza - recepcjonista
      //   component: DoctorDataEdit,
      //   meta: { roles: ["Employee"] },
      // },
      {
        path: "appointment/:id", // doctor - Szczegóły wizyty - z harmonogramu / doctor - Szczegóły wizyty (recipe)
        component: VisitDetails,
        meta: { roles: ["Patient", "Employee", "Staff"] },
      },
      {
        path: "patient/:id/card",
        component: PatientCard,
        meta: { roles: ["Employee"] },
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
        meta: { roles: ["Staff"] },
      },
      {
        path: "appointments/:id/cancel", // doctor - Szczegóły wizyty - z harmonogramu / doctor - Szczegóły wizyty (recipe)
        component: PatientCancelVisit,
        meta: { roles: ["Staff"] },
      },
      {
        path: "appointments/:id/assign", // recepcja - Przydziel wizytę(visitassign)
        component: VisitAssign,
        meta: { roles: ["Staff"] },
      },
      {
        path: "appointments/awaiting", // recepcja - Oczekujące wizyty
        component: AwaitingAppointments,
        meta: { roles: ["Staff"] },
      },
      {
        path: "statistics", // recepcja - Statystyka
        component: AppointmentStatistics,
        meta: { roles: ["Staff"] },
      },
      {
        path: "patients", // recepcja - Pacjenci - recepcjonista
        component: PatientList,
        meta: { roles: ["Staff", "Employee"] },
      },
      {
        path: "patient/edit/:id", // recepcja - Dane pacjenta - recepcjonista / pacjent - Dane pacjenta
        component: PatientDataReceptionEdit,
        meta: { roles: ["Staff"] },
      },
      {
        path: "password/:id/reset", // recepcja - Zmień hasło - recepcjonista
        component: StaffPasswordReset,
        meta: { roles: ["Staff"] },
      },
      {
        path: "patient/:id/card",
        component: PatientCard,
        meta: { roles: ["Staff"] },
      },
      {
        path: "doctors",
        component: DoctorPage,
        meta: { roles: ["Staff"] },
      },
      {
        path: "doctors/profile/:id",
        component: DoctorDataEdit,
        meta: { roles: ["Staff"] },
      }, // doctor - Dane lekarza - lekarz
      {
        path: "doctors/harmonogram/:id",
        component: VisitHarmonogram,
        meta: { roles: ["Staff"] },
      }, // doctor - harmonogram - lekarz
      {
        path: "profile",
        component: EmployeeData,
        meta: { roles: ["Staff"] },
      }, // doctor - Dane lekarza - lekarz
    ],
  },
  {
    path: "/auth/password-reset", // guest - zapomniałem hasła
    component: GuestPasswordReset,
    meta: { roles: null },
  },
];

// roles: ["Patient"]
// roles: null
// roles: [""]
