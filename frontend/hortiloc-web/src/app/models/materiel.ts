export interface Materiel {
  id: number;
  categorieId: number;
  categorieNom: string;
  nom: string;
  description: string | null;
  imageUrl: string | null;
  prixJournalier: number;
  quantiteTotale: number;
  quantiteDisponible: number;
  actif: boolean;
  dateCreation: string;
  dateModification: string;
}