# HortiLoc

HortiLoc est une application web de gestion de location de matériel horticole.

L'application permet de gérer les clients, les catégories de matériel, le matériel horticole, les locations, les retours et les maintenances.

## 1. Technologies utilisées

### Backend

- C#
- ASP.NET Core
- .NET 10
- Dapper
- MySql.Data
- MySQL 9.1

### Frontend

- Angular 21
- TypeScript
- Reactive Forms
- HttpClient
- Signals Angular
- Syntaxe moderne Angular :
  - `@if`
  - `@for`
  - `@switch`

### Base de données

- MySQL
- Scripts SQL fournis dans le dossier `database`

---

## 2. Architecture du projet

Le backend respecte une architecture en couches.

```text
HortiLoc
│
├── backend
│   ├── HortiLoc.API
│   │   └── Controllers
│   │
│   ├── HortiLoc.Core
│   │   ├── DTOs
│   │   ├── Entities
│   │   ├── Interfaces
│   │   └── Services
│   │
│   └── HortiLoc.Infrastructure
│       ├── Data
│       └── Repositories
│
├── frontend
│   └── hortiloc-web
│
├── database
│   ├── 01-create-database.sql
│   ├── 02-create-tables.sql
│   └── 03-insert-test-data.sql
│
└── README.md
```

### Flux général

```text
Composant Angular
        ↓
Service Angular
        ↓
HttpClient
        ↓
Controller ASP.NET Core
        ↓
Service Core
        ↓
Interface Repository
        ↓
Repository Infrastructure
        ↓
Dapper
        ↓
MySQL
```

Les contrôleurs ne contiennent pas de requêtes SQL.

Les accès à la base de données sont réalisés dans les repositories de la couche Infrastructure avec Dapper.

---

## 3. Fonctionnalités

### Gestion des clients

- afficher les clients ;
- ajouter un client ;
- modifier un client ;
- désactiver un client ;
- réactiver un client ;
- vérification de l'unicité de l'adresse e-mail.

### Gestion des catégories

- afficher les catégories ;
- ajouter une catégorie ;
- modifier une catégorie ;
- désactiver une catégorie ;
- réactiver une catégorie.

Les catégories utilisées dans la gestion du matériel sont chargées depuis MySQL via l'API.

### Gestion du matériel horticole

- afficher le matériel ;
- ajouter un matériel ;
- modifier un matériel ;
- désactiver un matériel ;
- réactiver un matériel ;
- gestion de la quantité totale ;
- gestion de la quantité disponible ;
- association du matériel à une catégorie ;
- prix journalier.

### Gestion des locations

- sélectionner un client actif ;
- choisir un ou plusieurs matériels ;
- définir la quantité ;
- définir les dates de location ;
- calcul automatique du montant par le backend ;
- vérification du stock disponible ;
- diminution automatique du stock lors d'une location.

### Retour du matériel

Lors du retour d'une location :

- le statut passe à `RETOURNEE` ;
- la date de retour est enregistrée ;
- les quantités de matériel sont remises automatiquement en stock.

La mise à jour est effectuée dans une transaction Dapper.

### Gestion des maintenances

- créer une maintenance ;
- associer une maintenance à un matériel ;
- statut `PLANIFIEE` ;
- démarrer une maintenance ;
- statut `EN_COURS` ;
- terminer une maintenance ;
- statut `TERMINEE` ;
- enregistrement automatique de la date de fin.

---

## 4. Prérequis

Avant de lancer le projet, installer :

- .NET SDK 10 ;
- Node.js et npm ;
- Angular CLI ;
- MySQL ;
- Git.

Vérification :

```powershell
dotnet --version
node --version
npm --version
ng version
mysql --version
git --version
```

---

## 5. Installation du projet

Cloner le dépôt GitHub :

```powershell
git clone <URL_DU_DEPOT_GITHUB>
cd HortiLoc
```

Si le projet est fourni sous forme de ZIP, extraire simplement le dossier puis ouvrir un terminal dans le dossier `HortiLoc`.

---

## 6. Création de la base de données

Démarrer le serveur MySQL.

Depuis PowerShell :

```powershell
mysql --default-character-set=utf8mb4 -u root -p
```

Entrer le mot de passe MySQL.

Puis dans MySQL :

```sql
SOURCE C:/Users/Benja/HortiLoc/database/01-create-database.sql;
SOURCE C:/Users/Benja/HortiLoc/database/02-create-tables.sql;
SOURCE C:/Users/Benja/HortiLoc/database/03-insert-test-data.sql;
```

Les scripts :

```text
01-create-database.sql
```

crée la base `hortiloc`.

```text
02-create-tables.sql
```

crée les tables et les relations.

```text
03-insert-test-data.sql
```

insère les données de démonstration.

Pour quitter MySQL :

```sql
exit;
```

---

## 7. Configuration de la connexion MySQL

Le fichier de configuration du backend se trouve dans :

```text
backend/HortiLoc.API/appsettings.json
```

Configuration utilisée pendant le développement :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=hortiloc;User=root;Password=;"
  }
}
```

Si l'utilisateur `root` possède un mot de passe, modifier la valeur `Password`.

Exemple :

```text
Password=monMotDePasse;
```

---

## 8. Installation des dépendances Angular

Depuis la racine du projet :

```powershell
cd frontend\hortiloc-web
npm install
```

---

## 9. Lancer le backend

Ouvrir un premier terminal.

Depuis la racine du projet :

```powershell
cd C:\Users\Benja\HortiLoc
dotnet run --project backend\HortiLoc.API
```

L'API est accessible sur :

```text
http://localhost:5177
```

Exemple :

```text
http://localhost:5177/api/clients
```

---

## 10. Lancer le frontend Angular

Ouvrir un deuxième terminal.

```powershell
cd C:\Users\Benja\HortiLoc\frontend\hortiloc-web
ng serve
```

L'application est accessible sur :

```text
http://localhost:4200
```

---

## 11. Pages principales

```text
http://localhost:4200/clients
http://localhost:4200/categories
http://localhost:4200/materiels
http://localhost:4200/locations
http://localhost:4200/maintenances
```

La navigation entre ces pages est également disponible directement dans l'interface.

---

## 12. Compiler le backend

Depuis la racine :

```powershell
cd C:\Users\Benja\HortiLoc
dotnet build
```

Le projet doit se compiler sans erreur.

---

## 13. Compiler Angular

```powershell
cd C:\Users\Benja\HortiLoc\frontend\hortiloc-web
ng build
```

Le build Angular doit se terminer par :

```text
Application bundle generation complete.
```

---

## 14. Principales règles métier

### Clients

Une adresse e-mail ne peut pas être utilisée par plusieurs clients.

### Matériel

La quantité totale doit être supérieure à zéro.

Le prix journalier ne peut pas être négatif.

La quantité totale ne peut pas devenir inférieure à la quantité actuellement louée.

### Locations

Une location doit contenir au moins un matériel.

Le client doit exister et être actif.

Le matériel doit exister et être actif.

La quantité demandée ne peut pas dépasser le stock disponible.

La date de fin prévue ne peut pas être antérieure à la date de début.

Le montant d'une location est calculé par le backend.

### Retours

Une location déjà retournée ne peut pas être retournée une seconde fois.

Une location annulée ne peut pas être retournée.

Un retour rétablit automatiquement les quantités disponibles du matériel.

### Maintenances

Une maintenance doit être associée à un matériel existant et actif.

Les statuts possibles sont :

```text
PLANIFIEE
EN_COURS
TERMINEE
```

Une maintenance terminée reçoit automatiquement une date de fin.

Une maintenance prévue dans le futur ne peut pas être démarrée avant sa date de début.

---

## 15. Transactions

La création et le retour des locations utilisent des transactions afin de garantir la cohérence des données.

Exemple lors de la création d'une location :

```text
Création de la location
        ↓
Création des détails
        ↓
Vérification du stock
        ↓
Diminution du stock
        ↓
COMMIT
```

En cas d'erreur :

```text
ROLLBACK
```

Aucune modification partielle n'est alors conservée.

---

## 16. Données de démonstration

Le script `03-insert-test-data.sql` crée notamment :

- 3 clients ;
- 5 catégories ;
- 9 matériels horticoles ;
- des locations ;
- des détails de locations ;
- une maintenance.

Ces données permettent de tester directement l'application après l'installation.

---

## 17. Commandes de lancement rapide

### Terminal 1 - Backend

```powershell
cd C:\Users\Benja\HortiLoc
dotnet run --project backend\HortiLoc.API
```

### Terminal 2 - Frontend

```powershell
cd C:\Users\Benja\HortiLoc\frontend\hortiloc-web
ng serve
```

Puis ouvrir :

```text
http://localhost:4200
```

---

## 18. Auteur

Projet réalisé dans le cadre du cours Angular & .NET.

Application : **HortiLoc**

Sujet : gestion de location de matériel horticole.