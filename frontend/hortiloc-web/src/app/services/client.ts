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

  clients = signal<Client[]>([]);
  chargement = signal(false);
  erreur = signal('');
  clientEnEditionId = signal<number | null>(null);

  charger(): void {
    this.chargement.set(true);
    this.erreur.set('');

    this.http.get<Client[]>(this.apiUrl).subscribe({
      next: clients => {
        this.clients.set(clients);
        this.chargement.set(false);
      },
      error: () => {
        this.erreur.set('Impossible de charger les clients.');
        this.chargement.set(false);
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
    return this.http.patch<void>(`${this.apiUrl}/${id}/reactiver`, {});
  }
}