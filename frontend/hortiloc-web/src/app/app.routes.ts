import { Routes } from '@angular/router';
import { Clients } from './pages/clients/clients';
import { Materiels } from './pages/materiels/materiels';

export const routes: Routes = [
  {
    path: 'clients',
    component: Clients
  },
  {
  path: 'materiels',
  component: Materiels
},
  {
    path: '',
    redirectTo: 'clients',
    pathMatch: 'full'
  },
  
];