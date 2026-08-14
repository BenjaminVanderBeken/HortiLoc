import { Routes } from '@angular/router';
import { Clients } from './pages/clients/clients';
import { Materiels } from './pages/materiels/materiels';
import { Locations } from './pages/locations/locations';
import { Maintenances } from './pages/maintenances/maintenances';
import { Categories } from './pages/categories/categories';

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

  {
  path: 'locations',
  component: Locations
},
{
  path: 'maintenances',
  component: Maintenances
},
{
  path: 'categories',
  component: Categories
},
  
];