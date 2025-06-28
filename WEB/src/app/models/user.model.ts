export interface User {
  id: string;
  firstName: string;
  lastName: string;
  username: string;
  role: 'User' | 'Admin';
}

export interface UserLoginRequest {
  username: string;
  password: string;
}

export interface UserRegistrationRequest {
  firstName: string;
  lastName: string;
  username: string;
  password: string;
}

export interface UserLoginResponse {
  token: string;
}