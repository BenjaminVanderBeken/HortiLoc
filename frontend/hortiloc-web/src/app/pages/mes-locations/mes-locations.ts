import { Component, OnInit, inject, signal } from '@angular/core';
import { Location } from '../../models/location';
import { LocationService } from '../../services/location';

@Component({
  selector: 'app-mes-locations',
  imports: [],
  templateUrl: './mes-locations.html',
  styleUrl: './mes-locations.css'
})
export class MesLocations implements OnInit {
  private readonly locationService = inject(LocationService);

  locations = signal<Location[]>([]);
  chargement = signal(false);
  erreur = signal('');

  ngOnInit(): void {
    this.charger();
  }

  charger(): void {
    this.chargement.set(true);
    this.erreur.set('');

    this.locationService.mesLocations().subscribe({
      next: locations => {
        this.locations.set(locations);
        this.chargement.set(false);
      },
      error: () => {
        this.erreur.set(
          'Impossible de charger vos locations.'
        );
        this.chargement.set(false);
      }
    });
  }
}