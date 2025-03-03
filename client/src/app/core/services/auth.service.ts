import { HttpClient } from '@angular/common/http';
import { computed, effect, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs';
import { User } from 'src/app/models/user';
import { environment } from 'src/environments/environment';

const USER_STORAGE_KEY = 'user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  #userSignal = signal<User | null>(null);
  user = this.#userSignal.asReadonly();

  isLoggedIn = computed(() => !!this.user());

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

  loadUserFromStorage() {
    const json = localStorage.getItem(USER_STORAGE_KEY);
    if (json) {
      const user = JSON.parse(json);
      this.#userSignal.set(user);
    }
  }

  login(email: string, password: string) {
    return this.http.post<User>(`${environment.apiUrl}/account/login`, { email, password }).pipe(
      map(user => {
        this.#userSignal.set(user);
        // localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(user.token));
      })
    )
  }


  getCurrentUser(): void {

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
    this.#userSignal.set(null);
    // localStorage.removeItem(USER_STORAGE_KEY);
  }

  // private getDecodedToken(token: string): any {
  //   return JSON.parse(atob(token.split('.')[1]));
  // }
}
