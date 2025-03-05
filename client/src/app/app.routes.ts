import { Routes } from "@angular/router";
import { ShopComponent } from "./features/shop/shop.component";
import { LoginComponent } from "./features/login/login.component";
import { authGuard } from "./guards/auth.guard";


export const routes: Routes = [
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: '', component: ShopComponent },
      { path: 'home', component: ShopComponent },
    ]
  },

  { path: 'login', component: LoginComponent }
];