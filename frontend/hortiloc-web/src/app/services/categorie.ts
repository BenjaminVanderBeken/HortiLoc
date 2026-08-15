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

  private readonly _categories = signal<Categorie[]>([]);
  private readonly _chargement = signal(false);
  private readonly _erreur = signal('');
  private readonly _categorieEnEditionId = signal<number | null>(null);

  readonly categories = this._categories.asReadonly();
  readonly chargement = this._chargement.asReadonly();
  readonly erreur = this._erreur.asReadonly();
  readonly categorieEnEditionId = this._categorieEnEditionId.asReadonly();

  charger(): void {
    this._chargement.set(true);
    this._erreur.set('');

    this.http.get<Categorie[]>(this.apiUrl).subscribe({
      next: categories => {
        this._categories.set(categories);
        this._chargement.set(false);
      },
      error: () => {
        this._erreur.set('Impossible de charger les catégories.');
        this._chargement.set(false);
      }
    });
  }

  creer(dto: SaveCategorie): Observable<Categorie> {
    return this.http.post<Categorie>(this.apiUrl, dto);
  }

  modifier(id: number, dto: SaveCategorie): Observable<Categorie> {
    return this.http.put<Categorie>(
      `${this.apiUrl}/${id}`,
      dto
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
    this._categorieEnEditionId.set(id);
  }

  terminerEdition(): void {
    this._categorieEnEditionId.set(null);
  }

  effacerErreur(): void {
    this._erreur.set('');
  }

  definirErreur(message: string): void {
    this._erreur.set(message);
  }
}