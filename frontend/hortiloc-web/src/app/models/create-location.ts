export interface CreateDetailLocation {
  materielId: number;
  quantite: number;
}

export interface CreateLocation {
  clientId: number;
  dateDebut: string;
  dateFinPrevue: string;
  notes: string | null;
  details: CreateDetailLocation[];
}