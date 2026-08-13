import { Component, OnInit, inject } from '@angular/core';
import { ClientService } from '../../services/client';

@Component({
  selector: 'app-clients',
  imports: [],
  templateUrl: './clients.html',
  styleUrl: './clients.css'
})
export class Clients implements OnInit {
  clientService = inject(ClientService);

  ngOnInit(): void {
    this.clientService.charger();
  }
}