import { Component, OnInit, inject } from '@angular/core';
import { MaterielService } from '../../services/materiel';

@Component({
  selector: 'app-materiels',
  imports: [],
  templateUrl: './materiels.html',
  styleUrl: './materiels.css'
})
export class Materiels implements OnInit {
  materielService = inject(MaterielService);

  ngOnInit(): void {
    this.materielService.charger();
  }
}