export type User = {
  email: string;
  token: string;
  firstName: string;
  lastName: string;
  address: Address;
  role: string;
}

export type Address = {
  line1: string;
  line2?: string;
  city: string;
  state: string;
  postalCode: string;
}