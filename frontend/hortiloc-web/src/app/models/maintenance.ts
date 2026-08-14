export interface Maintenance {
  id: number;
  materielId: number;
  materielNom: string;
  dateDebut: string;
  dateFin: string | null;
  motif: string;
  statut: string;
  dateCreation: string;
}