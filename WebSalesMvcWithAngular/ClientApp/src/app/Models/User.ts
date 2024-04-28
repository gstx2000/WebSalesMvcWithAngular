export interface User {
  email: string;
  username?: string;
  password: string;
  rememberMe?: boolean;
  error?: string;
  recoveryToken?: string;
}
