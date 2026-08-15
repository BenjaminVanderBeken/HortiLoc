import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { tap } from 'rxjs';
import { AuthResult } from '../models/auth-result';
import { Login } from '../models/login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5177/api/auth';

  utilisateur = signal<AuthResult | null>(this.chargerUtilisateur());
  erreur = signal('');

  login(dto: Login) {
    this.erreur.set('');

    return this.http
      .post<AuthResult>(`${this.apiUrl}/login`, dto)
      .pipe(
        tap(resultat => {
          localStorage.setItem(
            'hortiloc_auth',
            JSON.stringify(resultat)
          );

          this.utilisateur.set(resultat);
        })
      );
  }

  logout(): void {
    localStorage.removeItem('hortiloc_auth');
    this.utilisateur.set(null);
  }

  estConnecte(): boolean {
    return this.utilisateur() !== null;
  }

  estAdmin(): boolean {
    return this.utilisateur()?.role === 'ADMIN';
  }

  estClient(): boolean {
    return this.utilisateur()?.role === 'CLIENT';
  }

  token(): string | null {
    return this.utilisateur()?.token ?? null;
  }

  private chargerUtilisateur(): AuthResult | null {
    const valeur = localStorage.getItem('hortiloc_auth');

    if (!valeur)
      return null;

    try {
      return JSON.parse(valeur) as AuthResult;
    }
    catch {
      localStorage.removeItem('hortiloc_auth');
      return null;
    }
  }
}