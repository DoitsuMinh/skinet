import { Routes } from "@angular/router";
import { ShopComponent } from "./features/shop/shop.component";
import { LoginComponent } from "./features/login/login.component";

export const routes: Routes = [
  { path: '', component: ShopComponent },
  { path: 'shop', component: ShopComponent },
  { path: 'login', component: LoginComponent }
];