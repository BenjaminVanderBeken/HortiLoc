import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';

export const clientGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.estConnecte()) {
    return router.parseUrl('/login');
  }

  if (!authService.estClient()) {
    return router.parseUrl('/clients');
  }

  return true;
};