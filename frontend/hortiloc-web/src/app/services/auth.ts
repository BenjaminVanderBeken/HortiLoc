import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { AuthResult } from '../models/auth-result';
import { Login } from '../models/login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5177/api/auth';

  private readonly _utilisateur =
    signal<AuthResult | null>(this.chargerUtilisateur());

  private readonly _erreur = signal('');

  readonly utilisateur = this._utilisateur.asReadonly();
  readonly erreur = this._erreur.asReadonly();

  login(dto: Login): Observable<AuthResult> {
    this._erreur.set('');

    return this.http
      .post<AuthResult>(`${this.apiUrl}/login`, dto)
      .pipe(
        tap(resultat => {
          localStorage.setItem(
            'hortiloc_auth',
            JSON.stringify(resultat)
          );

          this._utilisateur.set(resultat);
        })
      );
  }

  logout(): void {
    localStorage.removeItem('hortiloc_auth');
    this._utilisateur.set(null);
    this._erreur.set('');
  }

  estConnecte(): boolean {
    return this._utilisateur() !== null;
  }

  estAdmin(): boolean {
    return this._utilisateur()?.role === 'ADMIN';
  }

  estClient(): boolean {
    return this._utilisateur()?.role === 'CLIENT';
  }

  token(): string | null {
    return this._utilisateur()?.token ?? null;
  }

  effacerErreur(): void {
    this._erreur.set('');
  }

  definirErreur(message: string): void {
    this._erreur.set(message);
  }

  private chargerUtilisateur(): AuthResult | null {
    const valeur = localStorage.getItem('hortiloc_auth');

    if (!valeur) {
      return null;
    }

    try {
      const utilisateur =
        JSON.parse(valeur) as AuthResult;

      const expiration =
        new Date(utilisateur.expiration);

      if (expiration <= new Date()) {
        localStorage.removeItem('hortiloc_auth');
        return null;
      }

      return utilisateur;
    }
    catch {
      localStorage.removeItem('hortiloc_auth');
      return null;
    }
  }
}