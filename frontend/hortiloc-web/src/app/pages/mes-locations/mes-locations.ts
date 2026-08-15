import { Component, OnInit, inject } from '@angular/core';
import { LocationService } from '../../services/location';

@Component({
  selector: 'app-mes-locations',
  imports: [],
  templateUrl: './mes-locations.html',
  styleUrl: './mes-locations.css'
})
export class MesLocations implements OnInit {
  readonly locationService = inject(LocationService);

  ngOnInit(): void {
    this.locationService.chargerMesLocations();
  }
}