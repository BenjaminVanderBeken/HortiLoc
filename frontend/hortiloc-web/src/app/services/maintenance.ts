import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { Maintenance } from '../models/maintenance';
import { CreateMaintenance } from '../models/create-maintenance';

@Injectable({
  providedIn: 'root'
})
export class MaintenanceService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5177/api/maintenances';

  private readonly _maintenances = signal<Maintenance[]>([]);
  private readonly _chargement = signal(false);
  private readonly _erreur = signal('');
  private readonly _maintenanceEnEditionId = signal<number | null>(null);

  readonly maintenances = this._maintenances.asReadonly();
  readonly chargement = this._chargement.asReadonly();
  readonly erreur = this._erreur.asReadonly();
  readonly maintenanceEnEditionId =
    this._maintenanceEnEditionId.asReadonly();

  charger(): void {
    this._chargement.set(true);
    this._erreur.set('');

    this.http.get<Maintenance[]>(this.apiUrl).subscribe({
      next: maintenances => {
        this._maintenances.set(maintenances);
        this._chargement.set(false);
      },
      error: () => {
        this._erreur.set(
          'Impossible de charger les maintenances.'
        );
        this._chargement.set(false);
      }
    });
  }

  creer(dto: CreateMaintenance): Observable<Maintenance> {
    return this.http.post<Maintenance>(
      this.apiUrl,
      dto
    );
  }

  modifier(
    id: number,
    dto: CreateMaintenance
  ): Observable<Maintenance> {
    return this.http.put<Maintenance>(
      `${this.apiUrl}/${id}`,
      dto
    );
  }

  modifierStatut(
    id: number,
    statut: string
  ): Observable<void> {
    return this.http.patch<void>(
      `${this.apiUrl}/${id}/statut`,
      { statut }
    );
  }

  supprimer(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }

  commencerEdition(id: number): void {
    this._maintenanceEnEditionId.set(id);
  }

  terminerEdition(): void {
    this._maintenanceEnEditionId.set(null);
  }

  effacerErreur(): void {
    this._erreur.set('');
  }

  definirErreur(message: string): void {
    this._erreur.set(message);
  }
}