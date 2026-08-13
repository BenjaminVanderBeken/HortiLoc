import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Materiel } from '../models/materiel';

@Injectable({
  providedIn: 'root'
})
export class MaterielService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5177/api/materiels';

  materiels = signal<Materiel[]>([]);
  chargement = signal(false);
  erreur = signal('');

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
}