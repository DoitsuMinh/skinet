import { Routes } from "@angular/router";
import { ShopComponent } from "./features/shop/shop.component";
import { LoginComponent } from "./features/login/login.component";
import { authGuard } from "./guards/auth.guard";
import { ProductDetailsComponent } from "./features/shop/product-details/product-details.component";
import { HomeComponent } from "./layout/home/home.component";


export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: '', component: HomeComponent },
      { path: 'shop', component: ShopComponent },
      { path: 'shop/:id', component: ProductDetailsComponent },
      { path: '**', redirectTo: '', pathMatch: 'full' }
    ]
  },
];