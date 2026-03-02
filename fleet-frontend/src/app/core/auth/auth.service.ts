import { Injectable, signal, computed, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthResponse, UserRole } from '../models/models';

const TOKEN_KEY = 'fleet_token';
const USER_KEY  = 'fleet_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private router = inject(Router);
  private _user  = signal<AuthResponse | null>(this.loadUser());

  readonly user       = this._user.asReadonly();
  readonly isLoggedIn = computed(() => !!this._user());
  readonly role       = computed(() => this._user()?.role as UserRole | undefined);
  readonly fullName   = computed(() => this._user()?.fullName ?? '');

  setSession(auth: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, auth.token);
    localStorage.setItem(USER_KEY, JSON.stringify(auth));
    this._user.set(auth);
  }

  getToken(): string | null { return localStorage.getItem(TOKEN_KEY); }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this._user.set(null);
    this.router.navigate(['/login']);
  }

  hasRole(...roles: UserRole[]): boolean {
    return roles.includes(this.role() as UserRole);
  }

  canWrite(): boolean  { return this.hasRole('Admin', 'FleetManager'); }
  canDelete(): boolean { return this.hasRole('Admin'); }

  isTokenExpired(): boolean {
    const user = this._user();
    if (!user) return true;
    return new Date(user.expiresAt) <= new Date();
  }

  private loadUser(): AuthResponse | null {
    try {
      const raw = localStorage.getItem(USER_KEY);
      return raw ? JSON.parse(raw) : null;
    } catch { return null; }
  }
}