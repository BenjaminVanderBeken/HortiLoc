import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { MaintenanceService } from '../../services/maintenance';
import { MaterielService } from '../../services/materiel';
import { CreateMaintenance } from '../../models/create-maintenance';

@Component({
  selector: 'app-maintenances',
  imports: [ReactiveFormsModule],
  templateUrl: './maintenances.html',
  styleUrl: './maintenances.css'
})
export class Maintenances implements OnInit {
  private readonly fb = inject(FormBuilder);

  maintenanceService = inject(MaintenanceService);
  materielService = inject(MaterielService);

  formulaire = this.fb.group({
    materielId: [null as number | null, Validators.required],
    dateDebut: ['', Validators.required],
    motif: ['', Validators.required]
  });

  ngOnInit(): void {
    this.maintenanceService.charger();
    this.materielService.charger();
  }

  enregistrer(): void {
    if (this.formulaire.invalid) {
      this.formulaire.markAllAsTouched();
      return;
    }

    const valeur = this.formulaire.getRawValue();

    const dto: CreateMaintenance = {
      materielId: Number(valeur.materielId),
      dateDebut: valeur.dateDebut ?? '',
      motif: valeur.motif?.trim() ?? ''
    };

    this.maintenanceService.erreur.set('');

    this.maintenanceService.creer(dto).subscribe({
      next: () => {
        this.formulaire.reset({
          materielId: null,
          dateDebut: '',
          motif: ''
        });

        this.maintenanceService.charger();
      },
      error: err => {
        const message =
          typeof err.error === 'string'
            ? err.error
            : 'Impossible de créer la maintenance.';

        this.maintenanceService.erreur.set(message);
      }
    });
  }

  demarrer(id: number): void {
    this.changerStatut(id, 'EN_COURS');
  }

  terminer(id: number): void {
    if (!confirm('Confirmer la fin de cette maintenance ?')) {
      return;
    }

    this.changerStatut(id, 'TERMINEE');
  }

  private changerStatut(id: number, statut: string): void {
    this.maintenanceService.erreur.set('');

    this.maintenanceService.modifierStatut(id, statut).subscribe({
      next: () => {
        this.maintenanceService.charger();
      },
      error: err => {
        const message =
          typeof err.error === 'string'
            ? err.error
            : 'Impossible de modifier le statut.';

        this.maintenanceService.erreur.set(message);
      }
    });
  }
}