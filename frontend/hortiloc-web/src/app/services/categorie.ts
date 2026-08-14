import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { Categorie } from '../models/categorie';
import { SaveCategorie } from '../models/save-categorie';

@Injectable({
  providedIn: 'root'
})
export class CategorieService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5177/api/categories';

  categories = signal<Categorie[]>([]);
  chargement = signal(false);
  erreur = signal('');
  categorieEnEditionId = signal<number | null>(null);

  charger(): void {
    this.chargement.set(true);
    this.erreur.set('');

    this.http.get<Categorie[]>(this.apiUrl).subscribe({
      next: categories => {
        this.categories.set(categories);
        this.chargement.set(false);
      },
      error: () => {
        this.erreur.set('Impossible de charger les catégories.');
        this.chargement.set(false);
      }
    });
  }

  creer(dto: SaveCategorie): Observable<Categorie> {
    return this.http.post<Categorie>(this.apiUrl, dto);
  }

  modifier(id: number, dto: SaveCategorie): Observable<Categorie> {
    return this.http.put<Categorie>(`${this.apiUrl}/${id}`, dto);
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
}