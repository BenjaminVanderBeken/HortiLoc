import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { Materiel } from '../models/materiel';
import { SaveMateriel } from '../models/save-materiel';

@Injectable({
  providedIn: 'root'
})
export class MaterielService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5177/api/materiels';

  materiels = signal<Materiel[]>([]);
  chargement = signal(false);
  erreur = signal('');
  materielEnEditionId = signal<number | null>(null);

  charger(): void {
    this.chargement.set(true);
    this.erreur.set('');

    this.http.get<Materiel[]>(this.apiUrl).subscribe({
      next: materiels => {
        this.materiels.set(materiels);
        this.chargement.set(false);
      },
      error: () => {
        this.erreur.set('Impossible de charger le matériel.');
        this.chargement.set(false);
      }
    });
  }

  creer(materiel: SaveMateriel): Observable<Materiel> {
    return this.http.post<Materiel>(this.apiUrl, materiel);
  }

  modifier(id: number, materiel: SaveMateriel): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, materiel);
  }

  desactiver(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  reactiver(id: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/reactiver`, {});
  }
}