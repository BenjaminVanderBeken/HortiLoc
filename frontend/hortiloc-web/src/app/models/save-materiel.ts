export interface SaveMateriel {
  categorieId: number;
  nom: string;
  description: string | null;
  imageUrl: string | null;
  prixJournalier: number;
  quantiteTotale: number;
}