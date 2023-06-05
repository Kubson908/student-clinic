import { ref } from 'vue';
const passwordRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (value.toLowerCase() !== value && /\d/.test(value) && value.length >= 8)
      return true;
    else
      return "Hasło musi zawierać przynajmniej 8 znaków, małą literę, dużą literę i cyfrę";
  },
];
const nameRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (
      value[0].toLowerCase() !== value[0] &&
      value.slice(1).toLowerCase() === value.slice(1)
    )
      return true;
    else return "Imię powinno zaczynać się od wielkiej litery";
  },
  (value: string) => {
    if (
      !/\d/.test(value) &&
      !/[^a-zA-Z0-9\u0118\u0119\u00F3\u00D3\u0141\u0142\u0104\u0105\u017B\u017C\u017A\u0179\u0106\u0107\u0143\u0144\u015A\u015B]/.test(
        value
      ) &&
      !/_/.test(value)
    )
      return true;
    else return "Imię może zawierać tylko litery";
  },
];
const surnameRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (
      value[0].toLowerCase() !== value[0] &&
      value.slice(1).toLowerCase() === value.slice(1)
    )
      return true;
    else return "Nazwisko powinno zaczynać się od wielkiej litery";
  },
  (value: string) => {
    if (
      !/\d/.test(value) &&
      !/[^a-zA-Z0-9\u0118\u0119\u00F3\u00D3\u0141\u0142\u0104\u0105\u017B\u017C\u017A\u0179\u0106\u0107\u0143\u0144\u015A\u015B]/.test(
        value
      ) &&
      !/_/.test(value)
    )
      return true;
    else return "Nazwisko może zawierać tylko litery";
  },
];
const peselRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (!/\D/.test(value) && value.length === 11) return true;
    else return "Pesel musi składać się z 11 cyfr";
  },
];
const emailRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) =>
    /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(value) ||
    "Nieprawidłowy e-mail",
];
const phoneRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    if (!/\D/.test(value) && value.length === 9) return true;
    else return "Numer telefonu musi składać się z cyfr";
  },
];
const dateRules = [
  (value: string) => {
    if (value) return true;
    else return "Pole jest wymagane";
  },
  (value: string) => {
    const dateFromValue: Date = new Date(value);
    if (dateFromValue < new Date()) return true;
    else return "Podaj prawidłową datę";
  },
];
const notNull = [
  (value: any) => {
    if (value !== null && value !== undefined && value !== "") return true;
    else return "Pole jest wymagane";
  },
];
const max1000 = [
  (value: string) => {
    if (value.length <= 1000) return true;
    else return "Za dużo znaków";
  },
];
const symptomsRules = [
  notNull[0],
  max1000[0]
]
const getPeselRules = (birthDate: any) => {
  return [
    notNull[0],
    (value: string) => {
      if (!/\D/.test(value) && value.length === 11) return true;
      else return "Pesel musi składać się z 11 cyfr";
    },
    (value: string) => {
      if (birthDate === "Invalid Date") return true;
      const to_add = birthDate.getFullYear() <= 1899 ? 80 : birthDate.getFullYear() <= 1999 ? 0 : birthDate.getFullYear() <= 2099 ? 20 : birthDate.getFullYear() <= 2199 ? 40 : 60; 
      if (birthDate.getYear() % 100 === parseInt(value.substr(0, 2)) && birthDate.getMonth() + 1 + to_add === parseInt(value.substr(2, 2)) && birthDate.getDate() === parseInt(value.substr(4, 2))) return true;
      else return "Błędny pesel"
    },
    (value: string) => {
      if (birthDate === "Invalid Date") return true;
      const multipliers = [1, 3, 7, 9]
      let sum = 0;
      for (let i = 0; i < 11; i++) {
        sum += (multipliers[i % 4] * parseInt(value[i])) % 10;
      }
      if (sum % 10 === parseInt(value[10])) return true;
      else return "Błędny pesel";
    }
  ]
}
export {
  nameRules,
  surnameRules,
  passwordRules,
  emailRules,
  phoneRules,
  dateRules,
  notNull,
  max1000,
  symptomsRules,
  getPeselRules,
};
