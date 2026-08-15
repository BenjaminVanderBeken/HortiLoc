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

  private readonly _materiels = signal<Materiel[]>([]);
  private readonly _chargement = signal(false);
  private readonly _erreur = signal('');
  private readonly _materielEnEditionId = signal<number | null>(null);

  readonly materiels = this._materiels.asReadonly();
  readonly chargement = this._chargement.asReadonly();
  readonly erreur = this._erreur.asReadonly();
  readonly materielEnEditionId = this._materielEnEditionId.asReadonly();

  charger(): void {
    this._chargement.set(true);
    this._erreur.set('');

    this.http.get<Materiel[]>(this.apiUrl).subscribe({
      next: materiels => {
        this._materiels.set(materiels);
        this._chargement.set(false);
      },
      error: () => {
        this._erreur.set('Impossible de charger le matériel.');
        this._chargement.set(false);
      }
    });
  }

  creer(materiel: SaveMateriel): Observable<Materiel> {
    return this.http.post<Materiel>(this.apiUrl, materiel);
  }

  modifier(id: number, materiel: SaveMateriel): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      materiel
    );
  }

  desactiver(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }

  reactiver(id: number): Observable<void> {
    return this.http.patch<void>(
      `${this.apiUrl}/${id}/reactiver`,
      {}
    );
  }

  commencerEdition(id: number): void {
    this._materielEnEditionId.set(id);
  }

  terminerEdition(): void {
    this._materielEnEditionId.set(null);
  }

  effacerErreur(): void {
    this._erreur.set('');
  }

  definirErreur(message: string): void {
    this._erreur.set(message);
  }
}