import { Component, OnInit, inject } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { LocationService } from '../../services/location';
import { ClientService } from '../../services/client';
import { MaterielService } from '../../services/materiel';
import { CreateLocation } from '../../models/create-location';

@Component({
  selector: 'app-locations',
  imports: [ReactiveFormsModule],
  templateUrl: './locations.html',
  styleUrl: './locations.css'
})
export class Locations implements OnInit {
  private readonly fb = inject(FormBuilder);

  locationService = inject(LocationService);
  clientService = inject(ClientService);
  materielService = inject(MaterielService);

  formulaire = this.fb.group({
    clientId: [null as number | null, Validators.required],
    dateDebut: ['', Validators.required],
    dateFinPrevue: ['', Validators.required],
    notes: [''],
    details: this.fb.array([
      this.creerLigne()
    ])
  });

  ngOnInit(): void {
    this.locationService.charger();
    this.clientService.charger();
    this.materielService.charger();
  }

  get details(): FormArray {
    return this.formulaire.controls.details;
  }

  private creerLigne(): FormGroup {
    return this.fb.group({
      materielId: [null as number | null, Validators.required],
      quantite: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ajouterLigne(): void {
    this.details.push(this.creerLigne());
  }

  supprimerLigne(index: number): void {
    if (this.details.length > 1) {
      this.details.removeAt(index);
    }
  }

  enregistrer(): void {
    if (this.formulaire.invalid) {
      this.formulaire.markAllAsTouched();
      return;
    }

    const valeur = this.formulaire.getRawValue();

    const dto: CreateLocation = {
      clientId: Number(valeur.clientId),
      dateDebut: valeur.dateDebut ?? '',
      dateFinPrevue: valeur.dateFinPrevue ?? '',
      notes: valeur.notes?.trim() || null,
      details: valeur.details.map(ligne => ({
        materielId: Number(ligne['materielId']),
quantite: Number(ligne['quantite'])
      }))
    };

    this.locationService.erreur.set('');

    this.locationService.creer(dto).subscribe({
      next: () => {
        this.reinitialiserFormulaire();
        this.locationService.charger();
        this.materielService.charger();
      },
      error: err => {
        const message =
          typeof err.error === 'string'
            ? err.error
            : 'Impossible de créer la location.';

        this.locationService.erreur.set(message);
      }
    });
  }

  retourner(id: number): void {
    if (!confirm('Confirmer le retour de cette location ?')) {
      return;
    }

    this.locationService.erreur.set('');

    this.locationService.retourner(id).subscribe({
      next: () => {
        this.locationService.charger();
        this.materielService.charger();
      },
      error: err => {
        const message =
          typeof err.error === 'string'
            ? err.error
            : 'Impossible de retourner cette location.';

        this.locationService.erreur.set(message);
      }
    });
  }

  private reinitialiserFormulaire(): void {
    this.details.clear();
    this.details.push(this.creerLigne());

    this.formulaire.patchValue({
      clientId: null,
      dateDebut: '',
      dateFinPrevue: '',
      notes: ''
    });
  }
}