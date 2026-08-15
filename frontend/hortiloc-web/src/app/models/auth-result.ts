export interface AuthResult {
  token: string;
  email: string;
  role: string;
  clientId: number | null;
  expiration: string;
}