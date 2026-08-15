import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { Client } from '../models/client';
import { CreateClient } from '../models/create-client';

@Injectable({
  providedIn: 'root'
})
export class ClientService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5177/api/clients';

  private readonly _clients = signal<Client[]>([]);
  private readonly _chargement = signal(false);
  private readonly _erreur = signal('');
  private readonly _clientEnEditionId = signal<number | null>(null);

  readonly clients = this._clients.asReadonly();
  readonly chargement = this._chargement.asReadonly();
  readonly erreur = this._erreur.asReadonly();
  readonly clientEnEditionId = this._clientEnEditionId.asReadonly();

  charger(): void {
    this._chargement.set(true);
    this._erreur.set('');

    this.http.get<Client[]>(this.apiUrl).subscribe({
      next: clients => {
        this._clients.set(clients);
        this._chargement.set(false);
      },
      error: () => {
        this._erreur.set('Impossible de charger les clients.');
        this._chargement.set(false);
      }
    });
  }

  creer(client: CreateClient): Observable<Client> {
    return this.http.post<Client>(this.apiUrl, client);
  }

  modifier(id: number, client: CreateClient): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, client);
  }

  desactiver(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  reactiver(id: number): Observable<void> {
    return this.http.patch<void>(
      `${this.apiUrl}/${id}/reactiver`,
      {}
    );
  }

  commencerEdition(id: number): void {
    this._clientEnEditionId.set(id);
  }

  terminerEdition(): void {
    this._clientEnEditionId.set(null);
  }

  effacerErreur(): void {
    this._erreur.set('');
  }

  definirErreur(message: string): void {
    this._erreur.set(message);
  }
}