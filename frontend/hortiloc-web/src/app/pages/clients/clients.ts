import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ClientService } from '../../services/client';

@Component({
  selector: 'app-clients',
  imports: [ReactiveFormsModule],
  templateUrl: './clients.html',
  styleUrl: './clients.css'
})
export class Clients implements OnInit {
  clientService = inject(ClientService);
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

    this.clientService.creer({
      nom: valeur.nom!,
      prenom: valeur.prenom!,
      email: valeur.email || null,
      telephone: valeur.telephone || null,
      adresse: valeur.adresse || null
    }).subscribe({
      next: () => {
        this.formulaire.reset();
        this.clientService.charger();
      },
      error: err => {
        console.error(err);
      }
    });
  }
}