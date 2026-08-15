import { Routes } from '@angular/router';
import { Clients } from './pages/clients/clients';
import { Materiels } from './pages/materiels/materiels';
import { Locations } from './pages/locations/locations';
import { Maintenances } from './pages/maintenances/maintenances';
import { Categories } from './pages/categories/categories';
import { Login } from './pages/login/login';
import { adminGuard } from './guards/admin.guard';
import { MesLocations } from './pages/mes-locations/mes-locations';
import { clientGuard } from './guards/client.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: Login
  },
  {
    path: 'clients',
    component: Clients,
    canActivate: [adminGuard]
  },
  {
    path: 'categories',
    component: Categories,
    canActivate: [adminGuard]
  },
  {
    path: 'materiels',
    component: Materiels,
    canActivate: [adminGuard]
  },
  {
    path: 'locations',
    component: Locations,
    canActivate: [adminGuard]
  },
  {
    path: 'maintenances',
    component: Maintenances,
    canActivate: [adminGuard]
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
{
  path: 'mes-locations',
  component: MesLocations,
  canActivate: [clientGuard]
  },

];