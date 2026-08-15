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

  locations = signal<Location[]>([]);
  chargement = signal(false);
  erreur = signal('');

  charger(): void {
    this.chargement.set(true);
    this.erreur.set('');

    this.http.get<Location[]>(this.apiUrl).subscribe({
      next: locations => {
        this.locations.set(locations);
        this.chargement.set(false);
      },
      error: () => {
        this.erreur.set('Impossible de charger les locations.');
        this.chargement.set(false);
      }
    });
  }
  creer(dto: CreateLocation): Observable<Location> {
  return this.http.post<Location>(this.apiUrl, dto);
}

  retourner(id: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/retour`, {});
  }
  mesLocations(): Observable<Location[]> {
  return this.http.get<Location[]>(
    `${this.apiUrl}/mes-locations`
  );
}
}