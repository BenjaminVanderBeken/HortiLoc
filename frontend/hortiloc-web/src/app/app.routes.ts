import { Routes } from '@angular/router';
import { Clients } from './pages/clients/clients';

export const routes: Routes = [
  {
    path: 'clients',
    component: Clients
  },
  {
    path: '',
    redirectTo: 'clients',
    pathMatch: 'full'
  }
];