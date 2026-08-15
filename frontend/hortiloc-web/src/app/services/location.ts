import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { Location } from '../models/location';
import { CreateLocation } from '../models/create-location';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5177/api/locations';

  private readonly _locations = signal<Location[]>([]);
  private readonly _chargement = signal(false);
  private readonly _erreur = signal('');

  readonly locations = this._locations.asReadonly();
  readonly chargement = this._chargement.asReadonly();
  readonly erreur = this._erreur.asReadonly();

  private readonly _mesLocations = signal<Location[]>([]);
  private readonly _chargementMesLocations = signal(false);
  private readonly _erreurMesLocations = signal('');

  readonly mesLocationsClient = this._mesLocations.asReadonly();
  readonly chargementMesLocations = this._chargementMesLocations.asReadonly();
  readonly erreurMesLocations = this._erreurMesLocations.asReadonly();

  charger(): void {
    this._chargement.set(true);
    this._erreur.set('');

    this.http.get<Location[]>(this.apiUrl).subscribe({
      next: locations => {
        this._locations.set(locations);
        this._chargement.set(false);
      },
      error: () => {
        this._erreur.set('Impossible de charger les locations.');
        this._chargement.set(false);
      }
    });
  }

  creer(dto: CreateLocation): Observable<Location> {
    return this.http.post<Location>(this.apiUrl, dto);
  }

  retourner(id: number): Observable<void> {
    return this.http.patch<void>(
      `${this.apiUrl}/${id}/retour`,
      {}
    );
  }

  chargerMesLocations(): void {
    this._chargementMesLocations.set(true);
    this._erreurMesLocations.set('');

    this.http.get<Location[]>(
      `${this.apiUrl}/mes-locations`
    ).subscribe({
      next: locations => {
        this._mesLocations.set(locations);
        this._chargementMesLocations.set(false);
      },
      error: () => {
        this._erreurMesLocations.set(
          'Impossible de charger vos locations.'
        );
        this._chargementMesLocations.set(false);
      }
    });
  }

  effacerErreur(): void {
    this._erreur.set('');
  }

  definirErreur(message: string): void {
    this._erreur.set(message);
  }
}