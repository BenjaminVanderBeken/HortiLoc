import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';
import { Login as LoginDto } from '../../models/login';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);

  readonly authService = inject(AuthService);

  formulaire = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    motDePasse: ['', Validators.required]
  });

  connecter(): void {
    if (this.formulaire.invalid) {
      this.formulaire.markAllAsTouched();
      return;
    }

    const valeur = this.formulaire.getRawValue();

    const dto: LoginDto = {
      email: valeur.email ?? '',
      motDePasse: valeur.motDePasse ?? ''
    };

    this.authService.effacerErreur();

    this.authService.login(dto).subscribe({
      next: resultat => {
        if (resultat.role === 'ADMIN') {
          this.router.navigate(['/clients']);
        } else {
          this.router.navigate(['/mes-locations']);
        }
      },
      error: err => {
        const message =
          typeof err.error === 'string'
            ? err.error
            : 'Connexion impossible.';

        this.authService.definirErreur(message);
      }
    });
  }
}