export type User = {
  id?: number;
  name?: string;
  roles: Array<string>;
  isLoggedIn: boolean;
};
