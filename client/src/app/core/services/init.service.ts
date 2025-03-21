import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { filter, Observable, of, switchMap } from 'rxjs';
import { Cart } from 'src/app/shared/models/cart';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  private cartService = inject(CartService);
  private authService = inject(AuthService);

  init(): Observable<Cart> {
    if (window.location.href.includes('/account/')) {
      return of(null);
    }
    return this.getCurrentUserToken().pipe(
      switchMap(() => this.getCartObservable())
    )
  }

  getCartObservable(): Observable<Cart> {
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartService.getCart(cartId) : of(null);
    return cart$;
  }

  getCurrentUserToken() {
    return of(this.authService.isLoggedIn()).pipe(
      filter((x) => !x),
      switchMap(() => this.authService.refreshToken()),
      switchMap((token: string) => this.authService.getCurrentUser(token))
    )
  }
}
