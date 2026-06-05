import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, firstValueFrom } from 'rxjs';
import { tap, catchError, map, switchMap } from 'rxjs/operators';
import { LoginRequest, RegisterRequest, UsersClient } from '../app/web-api-client';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _isAuthenticated = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this._isAuthenticated.asObservable();
  private _initialized = false;

  constructor(private usersClient: UsersClient) {}

  async initialize(): Promise<void> {
    if (this._initialized) {
      return;
    }
    this._initialized = true;

    await this.checkAuthStatus();
  }

  private async checkAuthStatus(): Promise<void> {
    try {
      await firstValueFrom(this.usersClient.infoGET());
      this._isAuthenticated.next(true);
    } catch {
      this._isAuthenticated.next(false);
    }
  }

  login(email: string, password: string): Observable<void> {
    return this.usersClient.login(true, undefined, new LoginRequest({ email, password })).pipe(
      switchMap(() => this.usersClient.infoGET()),
      tap(() => this._isAuthenticated.next(true)),
      map(() => void 0),
      catchError((error) => {
        console.error('Login failed:', error);
        this._isAuthenticated.next(false);
        throw error;
      })
    );
  }

  register(email: string, password: string): Observable<void> {
    return this.usersClient.register(new RegisterRequest({ email, password }));
  }

  logout(): Observable<void> {
    return this.usersClient.logout({}).pipe(
      tap(() => this._isAuthenticated.next(false))
    );
  }
}
    );
  }
}
