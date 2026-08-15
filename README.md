# HortiLoc

HortiLoc est une application web de gestion de location de matériel horticole réalisée avec Angular, ASP.NET Core, Dapper et MySQL.

L'application permet de gérer les clients, les catégories, le matériel horticole, les locations, les retours et les maintenances.

Elle intègre également une authentification JWT avec deux rôles :

- `ADMIN`
- `CLIENT`

---

## 1. Fonctionnalités principales

### Gestion des clients

L'administrateur peut :

- afficher les clients ;
- ajouter un client ;
- modifier un client ;
- désactiver un client ;
- réactiver un client ;
- vérifier l'unicité de l'adresse e-mail.

### Gestion des catégories

L'administrateur peut :

- afficher les catégories ;
- ajouter une catégorie ;
- modifier une catégorie ;
- désactiver une catégorie ;
- réactiver une catégorie.

Les catégories utilisées dans la gestion du matériel sont chargées depuis MySQL via l'API.

### Gestion du matériel horticole

L'administrateur peut :

- afficher le matériel ;
- ajouter un matériel ;
- modifier un matériel ;
- désactiver un matériel ;
- réactiver un matériel ;
- définir un prix journalier ;
- gérer la quantité totale ;
- gérer la quantité disponible ;
- associer le matériel à une catégorie.

### Gestion des locations

L'administrateur peut :

- créer une location ;
- sélectionner un client actif ;
- choisir un ou plusieurs matériels ;
- définir les quantités ;
- définir une date de début ;
- définir une date de fin prévue ;
- ajouter des notes.

Le backend :

- vérifie le stock disponible ;
- calcule automatiquement le montant total ;
- enregistre les détails de la location ;
- diminue automatiquement les quantités disponibles.

### Retour du matériel

Lors du retour d'une location :

- le statut passe à `RETOURNEE` ;
- la date de retour est enregistrée ;
- les quantités de matériel sont remises en stock.

Le retour utilise une transaction Dapper afin de garantir la cohérence des données.

### Gestion des maintenances

L'administrateur peut :

- créer une maintenance ;
- modifier une maintenance ;
- supprimer une maintenance planifiée ;
- démarrer une maintenance ;
- terminer une maintenance.

Les statuts disponibles sont :

```text
PLANIFIEE
EN_COURS
TERMINEE
```

Lorsqu'une maintenance est terminée, sa date de fin est enregistrée automatiquement.

### Espace client

Un utilisateur avec le rôle `CLIENT` dispose d'un espace :

```text
Mes locations
```

Il peut consulter uniquement les locations associées à son propre compte client.

Le `clientId` est récupéré directement depuis le token JWT côté backend.

---

## 2. Technologies utilisées

### Backend

- C#
- ASP.NET Core
- .NET 10
- Dapper
- MySql.Data
- JWT Bearer Authentication
- ASP.NET Core PasswordHasher

### Frontend

- Angular
- TypeScript
- Reactive Forms
- HttpClient
- Angular Signals
- Angular Router
- Guards
- HTTP Interceptor

Syntaxe Angular moderne utilisée :

```text
@if
@for
@switch
```

### Base de données

- MySQL
- scripts SQL fournis avec le projet

---

## 3. Architecture

Le backend respecte une architecture en couches.

```text
HortiLoc
│
├── backend
│   │
│   ├── HortiLoc.API
│   │   ├── Controllers
│   │   └── Services
│   │
│   ├── HortiLoc.Core
│   │   ├── DTOs
│   │   ├── Entities
│   │   ├── Interfaces
│   │   └── Services
│   │
│   └── HortiLoc.Infrastructure
│       ├── Data
│       ├── Repositories
│       └── Services
│
├── frontend
│   └── hortiloc-web
│
├── database
│   ├── 01-create-database.sql
│   ├── 02-create-tables.sql
│   └── 03-insert-test-data.sql
│
├── HortiLoc.slnx
└── README.md
```

---

## 4. Flux général de l'application

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

Toutes les requêtes SQL sont centralisées dans les repositories de la couche Infrastructure.

---

## 5. Authentification

L'application utilise une authentification JWT.

Le flux de connexion est :

```text
Page Login Angular
        ↓
AuthService Angular
        ↓
POST /api/auth/login
        ↓
AuthController
        ↓
AuthService Core
        ↓
UtilisateurRepository
        ↓
Dapper / MySQL
        ↓
Vérification du mot de passe
        ↓
Création du JWT
        ↓
Retour du token vers Angular
```

Angular stocke ensuite les informations d'authentification et un interceptor ajoute automatiquement :

```text
Authorization: Bearer <token>
```

aux requêtes HTTP.

---

## 6. Rôles

### ADMIN

Le rôle `ADMIN` peut accéder aux pages :

```text
Clients
Catégories
Matériel
Locations
Maintenances
```

### CLIENT

Le rôle `CLIENT` peut accéder uniquement à :

```text
Mes locations
```

Les routes Angular sont protégées avec des guards.

Les endpoints sensibles de l'API sont également protégés avec :

```csharp
[Authorize(Roles = "ADMIN")]
```

ou :

```csharp
[Authorize(Roles = "CLIENT")]
```

La sécurité ne repose donc pas uniquement sur le frontend.

---

## 7. Comptes de démonstration

Les comptes de démonstration sont créés automatiquement au démarrage de l'API en environnement de développement s'ils n'existent pas encore.

### Administrateur

```text
Email : admin@hortiloc.be
Mot de passe : Admin123!
Rôle : ADMIN
```

### Client

```text
Email : client@hortiloc.be
Mot de passe : Client123!
Rôle : CLIENT
Client associé : id 1
```

Les mots de passe sont stockés sous forme hashée dans MySQL.

Ils ne sont pas enregistrés en clair dans la table `utilisateurs`.

---

## 8. Prérequis

Avant de lancer le projet, installer :

- .NET SDK 10 ;
- Node.js ;
- npm ;
- Angular CLI ;
- MySQL ;
- Git.

Pour vérifier les installations :

```powershell
dotnet --version
node --version
npm --version
ng version
mysql --version
git --version
```

---

## 9. Récupérer le projet

Avec Git :

```powershell
git clone <URL_DU_DEPOT_GITHUB>
cd HortiLoc
```

Remplacer :

```text
<URL_DU_DEPOT_GITHUB>
```

par l'URL publique du dépôt GitHub HortiLoc.

Si le projet est fourni sous forme de ZIP, extraire simplement le dossier.

---

## 10. Création de la base de données

Démarrer MySQL.

Puis lancer :

```powershell
mysql --default-character-set=utf8mb4 -u root -p
```

Entrer le mot de passe MySQL si nécessaire.

Dans MySQL, exécuter les trois scripts dans cet ordre :

```sql
SOURCE C:/chemin/vers/HortiLoc/database/01-create-database.sql;
SOURCE C:/chemin/vers/HortiLoc/database/02-create-tables.sql;
SOURCE C:/chemin/vers/HortiLoc/database/03-insert-test-data.sql;
```

Exemple sous Windows :

```sql
SOURCE C:/Users/Benja/HortiLoc/database/01-create-database.sql;
SOURCE C:/Users/Benja/HortiLoc/database/02-create-tables.sql;
SOURCE C:/Users/Benja/HortiLoc/database/03-insert-test-data.sql;
```

Les scripts permettent de :

```text
01-create-database.sql
→ créer la base hortiloc

02-create-tables.sql
→ créer les tables, relations et contraintes

03-insert-test-data.sql
→ ajouter les données de démonstration
```

Pour quitter MySQL :

```sql
exit;
```

---

## 11. Configuration MySQL

La chaîne de connexion se trouve dans :

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

Si l'utilisateur MySQL `root` possède un mot de passe, modifier :

```text
Password=;
```

par exemple :

```text
Password=monMotDePasse;
```

---

## 12. Configuration JWT

La configuration JWT se trouve également dans :

```text
backend/HortiLoc.API/appsettings.json
```

Exemple :

```json
{
  "Jwt": {
    "Key": "HortiLoc-Development-Jwt-Key-2026-Change-Me-123456789",
    "Issuer": "HortiLoc.API",
    "Audience": "HortiLoc.Angular"
  }
}
```

Cette configuration est prévue pour l'environnement local et scolaire du projet.

Dans une application de production, la clé JWT ne devrait pas être stockée directement dans le dépôt Git.

---

## 13. Installer les dépendances Angular

Depuis le dossier du frontend :

```powershell
cd frontend\hortiloc-web
npm install
```

---

## 14. Lancer le backend

Ouvrir un premier terminal à la racine du projet :

```powershell
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

Les routes protégées nécessitent un token JWT valide.

---

## 15. Lancer Angular

Ouvrir un deuxième terminal :

```powershell
cd frontend\hortiloc-web
ng serve
```

L'application est accessible sur :

```text
http://localhost:4200
```

La page de connexion est :

```text
http://localhost:4200/login
```

---

## 16. Pages Angular

### Administrateur

```text
/login
/clients
/categories
/materiels
/locations
/maintenances
```

### Client

```text
/login
/mes-locations
```

---

## 17. Principales routes API

### Authentification

```text
POST /api/auth/login
```

### Clients

```text
GET    /api/clients
GET    /api/clients/{id}
POST   /api/clients
PUT    /api/clients/{id}
DELETE /api/clients/{id}
PATCH  /api/clients/{id}/reactiver
```

### Catégories

```text
GET    /api/categories
GET    /api/categories/{id}
POST   /api/categories
PUT    /api/categories/{id}
DELETE /api/categories/{id}
PATCH  /api/categories/{id}/reactiver
```

### Matériel

```text
GET    /api/materiels
GET    /api/materiels/{id}
POST   /api/materiels
PUT    /api/materiels/{id}
DELETE /api/materiels/{id}
PATCH  /api/materiels/{id}/reactiver
```

### Locations

Routes administrateur :

```text
GET   /api/locations
GET   /api/locations/{id}
POST  /api/locations
PATCH /api/locations/{id}/retour
```

Route client :

```text
GET /api/locations/mes-locations
```

### Maintenances

```text
GET    /api/maintenances
GET    /api/maintenances/{id}
POST   /api/maintenances
PUT    /api/maintenances/{id}
PATCH  /api/maintenances/{id}/statut
DELETE /api/maintenances/{id}
```

---

## 18. Règles métier principales

### Clients

- le nom et le prénom sont obligatoires ;
- l'adresse e-mail doit être unique ;
- un client peut être désactivé puis réactivé.

### Catégories

- le nom est obligatoire ;
- le nom doit être unique ;
- une catégorie peut être désactivée puis réactivée.

### Matériel

- le matériel doit être associé à une catégorie ;
- le prix journalier ne peut pas être négatif ;
- la quantité totale doit être supérieure à zéro ;
- la quantité totale ne peut pas devenir inférieure à la quantité actuellement louée.

### Locations

- un client doit exister et être actif ;
- une location doit contenir au moins un matériel ;
- le matériel doit exister et être actif ;
- la quantité demandée ne peut pas dépasser le stock disponible ;
- la date de fin prévue ne peut pas être antérieure à la date de début ;
- le montant est calculé côté backend.

### Retours

- une location déjà retournée ne peut pas être retournée une seconde fois ;
- une location annulée ne peut pas être retournée ;
- un retour remet automatiquement le matériel en stock.

### Maintenances

- le matériel doit exister et être actif ;
- une maintenance terminée ne peut plus être modifiée ;
- seule une maintenance `PLANIFIEE` peut être supprimée ;
- une maintenance future ne peut pas être démarrée avant sa date de début ;
- lorsqu'une maintenance devient `TERMINEE`, la date de fin est enregistrée automatiquement.

---

## 19. Transactions Dapper

La création et le retour des locations utilisent des transactions.

### Création d'une location

```text
BEGIN TRANSACTION
        ↓
Création de la location
        ↓
Vérification du stock
        ↓
Création des détails
        ↓
Diminution des quantités disponibles
        ↓
COMMIT
```

En cas d'erreur :

```text
ROLLBACK
```

### Retour d'une location

```text
BEGIN TRANSACTION
        ↓
Lecture des détails
        ↓
Remise des quantités en stock
        ↓
Statut = RETOURNEE
        ↓
Enregistrement dateRetour
        ↓
COMMIT
```

---

## 20. Données de démonstration

Le script :

```text
database/03-insert-test-data.sql
```

crée notamment :

- 3 clients ;
- 5 catégories ;
- 9 matériels horticoles ;
- des locations ;
- des détails de locations ;
- une maintenance.

Les comptes de connexion sont ensuite créés automatiquement par le backend en environnement de développement.

---

## 21. Compiler le backend

Depuis la racine :

```powershell
dotnet build
```

Le projet doit se compiler sans erreur.

---

## 22. Compiler Angular

Depuis :

```powershell
cd frontend\hortiloc-web
```

lancer :

```powershell
ng build
```

Le résultat attendu contient :

```text
Application bundle generation complete.
```

---

## 23. Lancement rapide

### Terminal 1 - API

Depuis la racine :

```powershell
dotnet run --project backend\HortiLoc.API
```

### Terminal 2 - Angular

```powershell
cd frontend\hortiloc-web
ng serve
```

Puis ouvrir :

```text
http://localhost:4200
```

---

## 24. Sécurité

Le projet applique plusieurs niveaux de sécurité :

- hash des mots de passe ;
- authentification JWT ;
- expiration du token ;
- validation de la signature JWT ;
- validation de l'émetteur ;
- validation de l'audience ;
- rôles `ADMIN` et `CLIENT` ;
- guards Angular ;
- interceptor HTTP ;
- `[Authorize]` côté API ;
- récupération du `clientId` directement depuis le JWT pour l'espace client.

Un client ne peut donc pas utiliser l'interface ou l'API pour accéder aux fonctionnalités réservées à l'administrateur.

---

## 25. Auteur

Projet réalisé dans le cadre du cours Angular & .NET.

**Application : HortiLoc**

Sujet :

```text
Gestion de location de matériel horticole
```