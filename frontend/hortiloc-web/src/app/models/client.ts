export interface Client {
  id: number;
  nom: string;
  prenom: string;
  email: string | null;
  telephone: string | null;
  adresse: string | null;
  actif: boolean;
  dateCreation: string;
  dateModification: string;
}