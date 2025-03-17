import { Token } from "./token";

export type User = {
  email: string;
  firstName: string;
  lastName: string;
  address: Address;
  role: string;
  token: string
}

export type Address = {
  line1: string;
  line2?: string;
  city: string;
  state: string;
  postalCode: string;
}