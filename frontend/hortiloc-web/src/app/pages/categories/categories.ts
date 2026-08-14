import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { CategorieService } from '../../services/categorie';
import { Categorie } from '../../models/categorie';
import { SaveCategorie } from '../../models/save-categorie';

@Component({
  selector: 'app-categories',
  imports: [ReactiveFormsModule],
  templateUrl: './categories.html',
  styleUrl: './categories.css'
})
export class Categories implements OnInit {
  private readonly fb = inject(FormBuilder);

  categorieService = inject(CategorieService);

  formulaire = this.fb.group({
    nom: ['', Validators.required],
    description: ['']
  });

  ngOnInit(): void {
    this.categorieService.charger();
  }

  enregistrer(): void {
    if (this.formulaire.invalid) {
      this.formulaire.markAllAsTouched();
      return;
    }

    const valeur = this.formulaire.getRawValue();

    const dto: SaveCategorie = {
      nom: valeur.nom?.trim() ?? '',
      description: valeur.description?.trim() || null
    };

    const id = this.categorieService.categorieEnEditionId();

    if (id === null) {
      this.creer(dto);
    } else {
      this.modifier(id, dto);
    }
  }

  modifierCategorie(categorie: Categorie): void {
    this.categorieService.categorieEnEditionId.set(categorie.id);

    this.formulaire.patchValue({
      nom: categorie.nom,
      description: categorie.description ?? ''
    });
  }

  annuler(): void {
    this.categorieService.categorieEnEditionId.set(null);
    this.formulaire.reset({
      nom: '',
      description: ''
    });
  }

  desactiver(categorie: Categorie): void {
    if (!confirm(`Désactiver la catégorie "${categorie.nom}" ?`)) {
      return;
    }

    this.categorieService.desactiver(categorie.id).subscribe({
      next: () => this.categorieService.charger(),
      error: () => {
        this.categorieService.erreur.set(
          'Impossible de désactiver la catégorie.'
        );
      }
    });
  }

  reactiver(categorie: Categorie): void {
    if (!confirm(`Réactiver la catégorie "${categorie.nom}" ?`)) {
      return;
    }

    this.categorieService.reactiver(categorie.id).subscribe({
      next: () => this.categorieService.charger(),
      error: () => {
        this.categorieService.erreur.set(
          'Impossible de réactiver la catégorie.'
        );
      }
    });
  }

  private creer(dto: SaveCategorie): void {
    this.categorieService.creer(dto).subscribe({
      next: () => {
        this.annuler();
        this.categorieService.charger();
      },
      error: err => {
        this.afficherErreur(err, 'Impossible de créer la catégorie.');
      }
    });
  }

  private modifier(id: number, dto: SaveCategorie): void {
    this.categorieService.modifier(id, dto).subscribe({
      next: () => {
        this.annuler();
        this.categorieService.charger();
      },
      error: err => {
        this.afficherErreur(err, 'Impossible de modifier la catégorie.');
      }
    });
  }

  private afficherErreur(err: any, messageParDefaut: string): void {
    const message =
      typeof err.error === 'string'
        ? err.error
        : messageParDefaut;

    this.categorieService.erreur.set(message);
  }
}