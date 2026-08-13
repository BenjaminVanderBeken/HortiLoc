export interface SaveMateriel {
  categorieId: number;
  nom: string;
  description: string | null;
  prixJournalier: number;
  quantiteTotale: number;
}