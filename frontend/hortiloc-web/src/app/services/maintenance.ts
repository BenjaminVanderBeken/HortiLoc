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

  maintenances = signal<Maintenance[]>([]);
  chargement = signal(false);
  erreur = signal('');
  maintenanceEnEditionId = signal<number | null>(null);

  charger(): void {
    this.chargement.set(true);
    this.erreur.set('');

    this.http.get<Maintenance[]>(this.apiUrl).subscribe({
      next: maintenances => {
        this.maintenances.set(maintenances);
        this.chargement.set(false);
      },
      error: () => {
        this.erreur.set('Impossible de charger les maintenances.');
        this.chargement.set(false);
      }
    });
  }

  creer(dto: CreateMaintenance): Observable<Maintenance> {
    return this.http.post<Maintenance>(this.apiUrl, dto);
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

  modifierStatut(id: number, statut: string): Observable<void> {
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
}