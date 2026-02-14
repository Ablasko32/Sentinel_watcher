export interface ILoginUserRequest {
  email: string;
  password: string;
}

export interface IAppUser {
  id: string;
  email: string;
  username: string;
  role: string;
}
