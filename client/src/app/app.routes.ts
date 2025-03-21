import { Routes } from "@angular/router";
import { ShopComponent } from "./features/shop/shop.component";
import { LoginComponent } from "./features/login/login.component";
import { authGuard } from "./core/guards/auth.guard";
import { ProductDetailsComponent } from "./features/shop/product-details/product-details.component";
import { HomeComponent } from "./layout/home/home.component";
import { CartComponent } from "./features/cart/cart.component";
import { CheckoutComponent } from "./features/checkout/checkout.component";
import { RegisterComponent } from "./features/register/register.component";
import { emptyCartGuard } from "./core/guards/empty-cart.guard";


export const routes: Routes = [
  { path: 'account/login', component: LoginComponent },
  { path: 'account/register', component: RegisterComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: '', component: HomeComponent },
      { path: 'shop', component: ShopComponent },
      { path: 'shop/:id', component: ProductDetailsComponent },
      { path: 'cart', component: CartComponent },
      { path: 'checkout', component: CheckoutComponent, canActivate: [emptyCartGuard] },
      { path: '**', redirectTo: '', pathMatch: 'full' },
    ]
  },
];