import { HttpClient, HttpParams } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { map, switchMap, tap } from 'rxjs';
import { AuthenticatedResponse } from 'src/app/shared/models/authenticatedResponse';
import { Token } from 'src/app/shared/models/token';
import { Address, User } from 'src/app/shared/models/user';

import { environment } from 'src/environments/environment';

const USER_STORAGE_KEY = 'user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  currentUser = signal<User | null>(null);
  user = this.currentUser.asReadonly();

  #isRefreshToken = signal<boolean>(false);
  // isRefreshToken = this.#isRefreshToken.asReadonly();
  isRefreshingToken = computed(() => this.#isRefreshToken())

  isLoggedIn = computed(() => !!this.user())

  private http = inject(HttpClient);
  private router = inject(Router);

  startRefreshingToken() {
    this.#isRefreshToken.set(true);
  }

  completeRefreshToken() {
    this.#isRefreshToken.set(false);

  }

  loadUserFromStorage() {
    const json = localStorage.getItem(USER_STORAGE_KEY);
    if (json) {
      const user = JSON.parse(json);
      this.currentUser.set(user);
    }
  }

  login(email: string, password: string) {
    return this.validateLoginInput(email, password).pipe(
      switchMap((token: AuthenticatedResponse) => {
        return this.getCurrentUser(token.accessToken);
      })
    );
  }

  register(values: any) {
    return this.http.post(`${environment.apiUrl}/account/register`, values);
  }

  validateLoginInput(email: string, password: string) {
    let params = new HttpParams();
    params = params.append('useCookies', true);
    return this.http.post<AuthenticatedResponse>(
      `${environment.apiUrl}/account/login`,
      { email, password },
      {
        params, withCredentials: true
      }
    ).pipe(
      tap((result: AuthenticatedResponse) => {
        const tempUser: User = {
          token: result.accessToken,
          email: '',
          firstName: '',
          lastName: '',
          address: null,
          role: ''
        }
        this.currentUser.set(tempUser);

        return result;
      })
    )
  }

  getCurrentUser(token: string) {
    return this.http.get<User>(`${environment.apiUrl}/account/user-info`).pipe(
      map(user => {
        console.log(user)
        user.token = token;
        this.currentUser.set(user);
      })
    )
  }

  refreshToken() {
    return this.http.post<Token>(`${environment.apiUrl}/token/refresh`, {}).pipe(
      map((result: Token) => {
        this.currentUser.update(currentUser => {
          return {
            ...currentUser,
            token: result.accessToken
          }
        })
        return result.accessToken;
      })
    )
  }

  revokeToken() {
    return this.http.post(`${environment.apiUrl}/token/revoke`, {})
  }

  updateAddress(address: Address) {
    return this.http.post(`${environment.apiUrl}/account/adress`, address);
  }

  // getCurrentUser_LocalStorage(): void {
  //   const key = localStorage.getItem('user');
  //   if (key) {
  //     const userToken = JSON.parse(key!);
  //     const userDecoded = this.getDecodedToken(userToken);
  //     const user: User = {
  //       username: userDecoded.username,
  //       email: userDecoded.email,
  //       token: userToken,
  //       refreshToken: userDecoded.refreshToken,
  //       displayName: userDecoded.displayName,
  //       role: userDecoded.roles
  //     };
  //     this.#userSignal.set(user);
  //   }
  // }

  logout() {
    // this.#userSignal.set(null);
    // if (this.isLoggedIn()) { this.revokeToken().subscribe(); }
    return this.http.post(`${environment.apiUrl}/account/logout`, {})
    // this.#userSignal.set(null);
    // localStorage.removeItem(USER_STORAGE_KEY);
  }

  // private getDecodedToken(token: string): any {
  //   return JSON.parse(atob(token.split('.')[1]));
  // }
}
