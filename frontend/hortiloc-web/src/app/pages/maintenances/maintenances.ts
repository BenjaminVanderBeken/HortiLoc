import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { MaintenanceService } from '../../services/maintenance';
import { MaterielService } from '../../services/materiel';
import { CreateMaintenance } from '../../models/create-maintenance';
import { Maintenance } from '../../models/maintenance';

@Component({
  selector: 'app-maintenances',
  imports: [ReactiveFormsModule],
  templateUrl: './maintenances.html',
  styleUrl: './maintenances.css'
})
export class Maintenances implements OnInit {
  private readonly fb = inject(FormBuilder);

  readonly maintenanceService = inject(MaintenanceService);
  readonly materielService = inject(MaterielService);

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

    const id =
      this.maintenanceService.maintenanceEnEditionId();

    if (id === null) {
      this.creer(dto);
    } else {
      this.modifier(id, dto);
    }
  }

  modifierMaintenance(maintenance: Maintenance): void {
    this.maintenanceService.commencerEdition(
      maintenance.id
    );

    this.formulaire.patchValue({
      materielId: maintenance.materielId,
      dateDebut: maintenance.dateDebut.substring(0, 10),
      motif: maintenance.motif
    });
  }

  annuler(): void {
    this.maintenanceService.terminerEdition();

    this.formulaire.reset({
      materielId: null,
      dateDebut: '',
      motif: ''
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

  supprimer(maintenance: Maintenance): void {
    if (
      !confirm(
        `Supprimer la maintenance de "${maintenance.materielNom}" ?`
      )
    ) {
      return;
    }

    this.maintenanceService.effacerErreur();

    this.maintenanceService.supprimer(maintenance.id).subscribe({
      next: () => {
        if (
          this.maintenanceService.maintenanceEnEditionId()
          === maintenance.id
        ) {
          this.annuler();
        }

        this.maintenanceService.charger();
      },
      error: err => {
        this.afficherErreur(
          err,
          'Impossible de supprimer la maintenance.'
        );
      }
    });
  }

  private creer(dto: CreateMaintenance): void {
    this.maintenanceService.effacerErreur();

    this.maintenanceService.creer(dto).subscribe({
      next: () => {
        this.annuler();
        this.maintenanceService.charger();
      },
      error: err => {
        this.afficherErreur(
          err,
          'Impossible de créer la maintenance.'
        );
      }
    });
  }

  private modifier(
    id: number,
    dto: CreateMaintenance
  ): void {
    this.maintenanceService.effacerErreur();

    this.maintenanceService.modifier(id, dto).subscribe({
      next: () => {
        this.annuler();
        this.maintenanceService.charger();
      },
      error: err => {
        this.afficherErreur(
          err,
          'Impossible de modifier la maintenance.'
        );
      }
    });
  }

  private changerStatut(
    id: number,
    statut: string
  ): void {
    this.maintenanceService.effacerErreur();

    this.maintenanceService.modifierStatut(id, statut).subscribe({
      next: () => {
        this.maintenanceService.charger();
      },
      error: err => {
        this.afficherErreur(
          err,
          'Impossible de modifier le statut.'
        );
      }
    });
  }

  private afficherErreur(
    err: { error?: unknown },
    messageParDefaut: string
  ): void {
    const message =
      typeof err.error === 'string'
        ? err.error
        : messageParDefaut;

    this.maintenanceService.definirErreur(message);
  }
}