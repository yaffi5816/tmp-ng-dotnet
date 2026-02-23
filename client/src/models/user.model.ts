export interface UserReadOnly {
  userId: number;
  userName: string;
  firstName: string;
  lastName: string;
  isAdmin: boolean;
}

export interface UserRegister {
  userName: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface UserLogin {
  userName: string;
  password: string;
}

export interface UserUpdate {
  userName: string;
  firstName: string;
  lastName: string;
  isAdmin: boolean;
}

// Alias for backward compatibility
export type User = UserReadOnly;
