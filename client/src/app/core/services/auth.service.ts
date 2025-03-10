import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { map, switchMap, tap } from 'rxjs';
import { Token } from 'src/app/shared/models/token';
import { User } from 'src/app/shared/models/user';

import { environment } from 'src/environments/environment';

const USER_STORAGE_KEY = 'user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  #userSignal = signal<User | null>(null);
  user = this.#userSignal.asReadonly();

  #isRefreshToken = signal<boolean>(false);
  // isRefreshToken = this.#isRefreshToken.asReadonly();
  isRefreshingToken = computed(() => this.#isRefreshToken())

  isLoggedIn = computed(() => !!this.user())

  http = inject(HttpClient);

  router = inject(Router);

  constructor() {
    // this.loadUserFromStorage();
    // effect(() => {
    //   const user = this.user();
    //   if (user) {
    //     localStorage.setItem(USER_STORAGE_KEY,
    //       JSON.stringify(user));
    //   }
    // })
  }

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
      this.#userSignal.set(user);
    }
  }

  login(email: string, password: string) {
    return this.validateLoginInput(email, password).pipe(
      // After validating credentials, get the token
      switchMap(() => this.getToken(email)),
      // After getting the token, get the current user
      switchMap((tokenResponse: Token) => {
        return this.getCurrentUser(tokenResponse.accessToken);
      })
    );
  }

  validateLoginInput(email: string, password: string) {
    return this.http.post(`${environment.apiUrl}/account/login`, { email, password })
  }

  getToken(email: string) {
    return this.http.post<Token>(`${environment.apiUrl}/token`, { email, pasword: '' }, { withCredentials: true }).pipe(
      tap((result: Token) => {
        const tempUser: User = {
          token: result.accessToken,
          email: '',
          displayName: '',
          role: ''
        }
        this.#userSignal.set(tempUser);
      })
    )
  }

  getCurrentUser(accessToken: string) {
    return this.http.get<User>(`${environment.apiUrl}/account`).pipe(
      map(user => {
        user.token = accessToken;
        this.#userSignal.set(user);
      })
    )
  }

  refreshToken() {
    return this.http.post<Token>(`${environment.apiUrl}/token/refresh`, null, { withCredentials: true }).pipe(
      map((result: Token) => {
        this.#userSignal.update(currentUser => {
          // if (!currentUser) return null;
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
    return this.http.post(`${environment.apiUrl}/token/revoke`, null, { withCredentials: true })
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
    if (!this.isLoggedIn()) { this.revokeToken().subscribe(); }
    this.#userSignal.set(null);
    // localStorage.removeItem(USER_STORAGE_KEY);
  }

  // private getDecodedToken(token: string): any {
  //   return JSON.parse(atob(token.split('.')[1]));
  // }
}
