import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Materiel } from '../../models/materiel';
import { MaterielService } from '../../services/materiel';

@Component({
  selector: 'app-materiels',
  imports: [ReactiveFormsModule],
  templateUrl: './materiels.html',
  styleUrl: './materiels.css'
})
export class Materiels implements OnInit {
  materielService = inject(MaterielService);
  private readonly fb = inject(FormBuilder);

  categories = [
    { id: 1, nom: 'Tonte' },
    { id: 2, nom: 'Coupe' },
    { id: 3, nom: 'Travail du sol' },
    { id: 4, nom: 'Nettoyage' },
    { id: 5, nom: 'Broyage' }
  ];

  formulaire = this.fb.group({
    categorieId: [1, Validators.required],
    nom: ['', Validators.required],
    description: [''],
    prixJournalier: [0, [Validators.required, Validators.min(0)]],
    quantiteTotale: [1, [Validators.required, Validators.min(1)]]
  });

  ngOnInit(): void {
    this.materielService.charger();
  }

  enregistrer(): void {
    if (this.formulaire.invalid) {
      this.formulaire.markAllAsTouched();
      return;
    }

    const valeur = this.formulaire.getRawValue();

    const dto = {
      categorieId: valeur.categorieId!,
      nom: valeur.nom!,
      description: valeur.description || null,
      prixJournalier: valeur.prixJournalier!,
      quantiteTotale: valeur.quantiteTotale!
    };

    const id = this.materielService.materielEnEditionId();

    if (id === null) {
      this.materielService.creer(dto).subscribe({
        next: () => {
          this.annuler();
          this.materielService.charger();
        },
        error: err => console.error(err)
      });
    } else {
      this.materielService.modifier(id, dto).subscribe({
        next: () => {
          this.annuler();
          this.materielService.charger();
        },
        error: err => console.error(err)
      });
    }
  }

  modifier(materiel: Materiel): void {
    this.materielService.materielEnEditionId.set(materiel.id);

    this.formulaire.patchValue({
      categorieId: materiel.categorieId,
      nom: materiel.nom,
      description: materiel.description ?? '',
      prixJournalier: materiel.prixJournalier,
      quantiteTotale: materiel.quantiteTotale
    });
  }

  annuler(): void {
    this.materielService.materielEnEditionId.set(null);

    this.formulaire.reset({
      categorieId: 1,
      nom: '',
      description: '',
      prixJournalier: 0,
      quantiteTotale: 1
    });
  }

  desactiver(materiel: Materiel): void {
    if (!confirm(`Désactiver ${materiel.nom} ?`))
      return;

    this.materielService.desactiver(materiel.id).subscribe({
      next: () => this.materielService.charger(),
      error: err => console.error(err)
    });
  }

  reactiver(materiel: Materiel): void {
    if (!confirm(`Réactiver ${materiel.nom} ?`))
      return;

    this.materielService.reactiver(materiel.id).subscribe({
      next: () => this.materielService.charger(),
      error: err => console.error(err)
    });
  }
}