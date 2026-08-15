import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Client } from '../../models/client';
import { ClientService } from '../../services/client';

@Component({
  selector: 'app-clients',
  imports: [ReactiveFormsModule],
  templateUrl: './clients.html',
  styleUrl: './clients.css'
})
export class Clients implements OnInit {
  readonly clientService = inject(ClientService);
  private readonly fb = inject(FormBuilder);

  formulaire = this.fb.group({
    nom: ['', Validators.required],
    prenom: ['', Validators.required],
    email: [''],
    telephone: [''],
    adresse: ['']
  });

  ngOnInit(): void {
    this.clientService.charger();
  }

  enregistrer(): void {
    if (this.formulaire.invalid) {
      this.formulaire.markAllAsTouched();
      return;
    }

    const valeur = this.formulaire.getRawValue();

    const dto = {
      nom: valeur.nom!,
      prenom: valeur.prenom!,
      email: valeur.email || null,
      telephone: valeur.telephone || null,
      adresse: valeur.adresse || null
    };

    const id = this.clientService.clientEnEditionId();

    this.clientService.effacerErreur();

    if (id === null) {
      this.clientService.creer(dto).subscribe({
        next: () => {
          this.annuler();
          this.clientService.charger();
        },
        error: err => {
          const message =
            typeof err.error === 'string'
              ? err.error
              : 'Impossible de créer le client.';

          this.clientService.definirErreur(message);
        }
      });
    } else {
      this.clientService.modifier(id, dto).subscribe({
        next: () => {
          this.annuler();
          this.clientService.charger();
        },
        error: err => {
          const message =
            typeof err.error === 'string'
              ? err.error
              : 'Impossible de modifier le client.';

          this.clientService.definirErreur(message);
        }
      });
    }
  }

  modifier(client: Client): void {
    this.clientService.commencerEdition(client.id);

    this.formulaire.patchValue({
      nom: client.nom,
      prenom: client.prenom,
      email: client.email ?? '',
      telephone: client.telephone ?? '',
      adresse: client.adresse ?? ''
    });
  }

  annuler(): void {
    this.clientService.terminerEdition();
    this.formulaire.reset();
  }

  desactiver(client: Client): void {
    if (!confirm(`Désactiver ${client.prenom} ${client.nom} ?`)) {
      return;
    }

    this.clientService.effacerErreur();

    this.clientService.desactiver(client.id).subscribe({
      next: () => this.clientService.charger(),
      error: err => {
        const message =
          typeof err.error === 'string'
            ? err.error
            : 'Impossible de désactiver le client.';

        this.clientService.definirErreur(message);
      }
    });
  }

  reactiver(client: Client): void {
    if (!confirm(`Réactiver ${client.prenom} ${client.nom} ?`)) {
      return;
    }

    this.clientService.effacerErreur();

    this.clientService.reactiver(client.id).subscribe({
      next: () => this.clientService.charger(),
      error: err => {
        const message =
          typeof err.error === 'string'
            ? err.error
            : 'Impossible de réactiver le client.';

        this.clientService.definirErreur(message);
      }
    });
  }
}