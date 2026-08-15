import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Materiel } from '../../models/materiel';
import { MaterielService } from '../../services/materiel';
import { CategorieService } from '../../services/categorie';

@Component({
  selector: 'app-materiels',
  imports: [ReactiveFormsModule],
  templateUrl: './materiels.html',
  styleUrl: './materiels.css'
})
export class Materiels implements OnInit {
  readonly materielService = inject(MaterielService);
  readonly categorieService = inject(CategorieService);

  private readonly fb = inject(FormBuilder);

  formulaire = this.fb.group({
    categorieId: [null as number | null, Validators.required],
    nom: ['', Validators.required],
    description: [''],
    prixJournalier: [0, [Validators.required, Validators.min(0)]],
    quantiteTotale: [1, [Validators.required, Validators.min(1)]]
  });

  ngOnInit(): void {
    this.materielService.charger();
    this.categorieService.charger();
  }

  enregistrer(): void {
    if (this.formulaire.invalid) {
      this.formulaire.markAllAsTouched();
      return;
    }

    const valeur = this.formulaire.getRawValue();

    const dto = {
      categorieId: Number(valeur.categorieId),
      nom: valeur.nom!,
      description: valeur.description || null,
      prixJournalier: valeur.prixJournalier!,
      quantiteTotale: valeur.quantiteTotale!
    };

    const id = this.materielService.materielEnEditionId();

    this.materielService.effacerErreur();

    if (id === null) {
      this.materielService.creer(dto).subscribe({
        next: () => {
          this.annuler();
          this.materielService.charger();
        },
        error: err => {
          const message =
            typeof err.error === 'string'
              ? err.error
              : 'Impossible de créer le matériel.';

          this.materielService.definirErreur(message);
        }
      });
    } else {
      this.materielService.modifier(id, dto).subscribe({
        next: () => {
          this.annuler();
          this.materielService.charger();
        },
        error: err => {
          const message =
            typeof err.error === 'string'
              ? err.error
              : 'Impossible de modifier le matériel.';

          this.materielService.definirErreur(message);
        }
      });
    }
  }

  modifier(materiel: Materiel): void {
    this.materielService.commencerEdition(materiel.id);

    this.formulaire.patchValue({
      categorieId: materiel.categorieId,
      nom: materiel.nom,
      description: materiel.description ?? '',
      prixJournalier: materiel.prixJournalier,
      quantiteTotale: materiel.quantiteTotale
    });
  }

  annuler(): void {
    this.materielService.terminerEdition();

    this.formulaire.reset({
      categorieId: null,
      nom: '',
      description: '',
      prixJournalier: 0,
      quantiteTotale: 1
    });
  }

  desactiver(materiel: Materiel): void {
    if (!confirm(`Désactiver ${materiel.nom} ?`)) {
      return;
    }

    this.materielService.effacerErreur();

    this.materielService.desactiver(materiel.id).subscribe({
      next: () => this.materielService.charger(),
      error: err => {
        const message =
          typeof err.error === 'string'
            ? err.error
            : 'Impossible de désactiver le matériel.';

        this.materielService.definirErreur(message);
      }
    });
  }

  reactiver(materiel: Materiel): void {
    if (!confirm(`Réactiver ${materiel.nom} ?`)) {
      return;
    }

    this.materielService.effacerErreur();

    this.materielService.reactiver(materiel.id).subscribe({
      next: () => this.materielService.charger(),
      error: err => {
        const message =
          typeof err.error === 'string'
            ? err.error
            : 'Impossible de réactiver le matériel.';

        this.materielService.definirErreur(message);
      }
    });
  }
}