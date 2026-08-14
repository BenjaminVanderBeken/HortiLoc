import { DetailLocation } from './detail-location';

export interface Location {
  id: number;
  clientId: number;
  clientNom: string;
  clientPrenom: string;
  dateDebut: string;
  dateFinPrevue: string;
  dateRetour: string | null;
  statut: string;
  montantTotal: number;
  notes: string | null;
  dateCreation: string;
  details: DetailLocation[];
}