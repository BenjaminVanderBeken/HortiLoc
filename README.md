# HortiLoc

HortiLoc est une application web de gestion de location de matériel horticole.

Le projet a été réalisé avec :

- Angular pour le frontend ;
- ASP.NET Core pour l'API ;
- Dapper pour l'accès aux données ;
- MySQL pour la base de données.

Le backend respecte une architecture en couches de type Clean Architecture avec une séparation entre :

- API ;
- Core ;
- Infrastructure.

L'application possède également une authentification JWT avec deux rôles :

- `ADMIN`
- `CLIENT`

---

# 1. Fonctionnalités

## Gestion des clients

L'administrateur peut :

- consulter les clients ;
- ajouter un client ;
- modifier un client ;
- désactiver un client ;
- réactiver un client.

L'adresse e-mail d'un client doit être unique.

---

## Gestion des catégories

L'administrateur peut :

- consulter les catégories ;
- ajouter une catégorie ;
- modifier une catégorie ;
- désactiver une catégorie ;
- réactiver une catégorie.

Les catégories sont enregistrées dans MySQL et chargées depuis l'API.

---

## Gestion du matériel

L'administrateur peut :

- consulter le matériel ;
- ajouter un matériel ;
- modifier un matériel ;
- désactiver un matériel ;
- réactiver un matériel ;
- associer un matériel à une catégorie ;
- définir son prix journalier ;
- définir sa quantité totale ;
- suivre sa quantité disponible.

---

## Gestion des locations

L'administrateur peut :

- créer une location ;
- sélectionner un client ;
- choisir un ou plusieurs matériels ;
- définir une quantité pour chaque matériel ;
- définir une date de début ;
- définir une date de fin prévue ;
- ajouter des notes.

Le backend :

- vérifie le stock disponible ;
- calcule automatiquement le prix de la location ;
- crée les détails de location ;
- diminue les quantités disponibles.

---

## Retour du matériel

Lors du retour d'une location :

- le statut de la location devient `RETOURNEE` ;
- la date de retour est enregistrée ;
- les quantités louées sont remises dans le stock disponible.

Cette opération utilise une transaction Dapper afin de garantir la cohérence des données.

---

## Gestion des maintenances

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

---

## Authentification

L'application possède un système d'authentification JWT.

Deux rôles existent :

```text
ADMIN
CLIENT
```

Le rôle `ADMIN` permet d'accéder aux pages de gestion.

Le rôle `CLIENT` permet d'accéder uniquement à son espace personnel.

---

## Espace client

Un utilisateur avec le rôle `CLIENT` dispose de la page :

```text
Mes locations
```

Cette page affiche uniquement les locations associées au client connecté.

L'identifiant du client est récupéré côté API depuis le token JWT.

---

# 2. Technologies et versions utilisées

## Backend

- .NET SDK : `10.0.301`
- ASP.NET Core
- C#
- Dapper
- MySql.Data
- JWT Bearer Authentication
- ASP.NET Core PasswordHasher

## Frontend

- Angular CLI : `21.2.21`
- Angular : `21.2.20`
- Node.js : `24.14.1`
- npm : `11.11.0`
- TypeScript : `5.9.3`
- RxJS : `7.8.2`

## Base de données

- MySQL Community Server : `9.5.0`

## Environnement utilisé

- Windows 64 bits
- Visual Studio Code
- Git / GitHub

---

# 3. Architecture du projet

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
├── .gitignore
└── README.md
```

---

# 4. Clean Architecture

Le backend est séparé en trois projets.

## HortiLoc.API

Responsabilités :

- exposer les endpoints HTTP ;
- recevoir les requêtes du frontend ;
- appeler les services du Core ;
- gérer l'authentification et l'autorisation ;
- retourner les réponses HTTP.

Les contrôleurs ne contiennent aucune requête SQL.

---

## HortiLoc.Core

Responsabilités :

- entités ;
- DTOs ;
- interfaces des repositories ;
- services métier ;
- règles métier.

Le Core ne dépend pas de la base de données.

---

## HortiLoc.Infrastructure

Responsabilités :

- connexion à MySQL ;
- implémentation des repositories ;
- requêtes SQL ;
- Dapper ;
- services techniques comme le hash des mots de passe.

---

# 5. Flux d'une donnée

Le flux général de l'application est :

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

La réponse revient ensuite dans le sens inverse :

```text
MySQL
  ↓
Dapper
  ↓
Repository
  ↓
Service Core
  ↓
Controller
  ↓
HttpClient
  ↓
Service Angular
  ↓
Composant Angular
  ↓
Interface utilisateur
```

---

# 6. Gestion de l'état Angular

La gestion de l'état applicatif est réalisée via les Services Angular.

Les services utilisent des `signal()`.

Exemple de principe :

```text
API
 ↓
Service Angular
 ↓
Signal
 ↓
Composant
 ↓
HTML
```

Les signaux modifiables sont privés dans les services.

Les composants utilisent les signaux exposés en lecture seule.

Exemple :

```typescript
private readonly _clients = signal<Client[]>([]);

readonly clients = this._clients.asReadonly();
```

Les composants ne modifient donc pas directement les données métier.

Les formulaires Angular restent gérés dans les composants avec les Reactive Forms.

---

# 7. Angular moderne

Le frontend utilise notamment :

```text
@if
@for
@switch
```

Le projet utilise également :

- composants standalone ;
- services Angular ;
- Reactive Forms ;
- routing Angular ;
- guards ;
- HttpClient ;
- interceptor HTTP ;
- signals.

Aucune bibliothèque externe de gestion d'état comme NgRx ou Redux n'est utilisée.

---

# 8. Authentification JWT

Le flux de connexion est :

```text
Page Login
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
Retour du token
```

Angular stocke les informations de connexion dans le navigateur.

Un interceptor ajoute automatiquement le token aux requêtes HTTP :

```text
Authorization: Bearer <token>
```

---

# 9. Rôles et autorisations

## ADMIN

Le rôle `ADMIN` peut accéder à :

```text
Clients
Catégories
Matériel
Locations
Maintenances
```

## CLIENT

Le rôle `CLIENT` peut accéder à :

```text
Mes locations
```

Les routes Angular sont protégées avec des guards.

Les endpoints du backend sont également protégés avec :

```csharp
[Authorize(Roles = "ADMIN")]
```

ou :

```csharp
[Authorize(Roles = "CLIENT")]
```

La sécurité ne repose donc pas uniquement sur Angular.

---

# 10. Comptes de démonstration

Les comptes de démonstration sont créés automatiquement au démarrage de l'API en environnement de développement s'ils n'existent pas encore.

## Administrateur

```text
Email : admin@hortiloc.be
Mot de passe : Admin123!
Rôle : ADMIN
```

## Client

```text
Email : client@hortiloc.be
Mot de passe : Client123!
Rôle : CLIENT
Client associé : id 1
```

Les mots de passe sont stockés sous forme hashée dans la base de données.

---

# 11. Prérequis

Avant de lancer le projet, installer :

- .NET SDK 10 ;
- Node.js 24 ;
- npm ;
- Angular CLI 21 ;
- MySQL Community Server 9 ;
- Git.

Les versions peuvent être vérifiées avec :

```powershell
dotnet --version
node --version
npm --version
ng version
mysql --version
git --version
```

---

# 12. Récupération du projet

## Avec Git

```powershell
git clone <URL_DU_DEPOT_GITHUB>
cd HortiLoc
```

Remplacer :

```text
<URL_DU_DEPOT_GITHUB>
```

par l'URL publique du dépôt GitHub.

Il est également possible d'utiliser directement l'archive ZIP fournie sur Moodle.

---

# 13. Création de la base de données

Démarrer MySQL.

Ouvrir ensuite un terminal :

```powershell
mysql --default-character-set=utf8mb4 -u root -p
```

Entrer le mot de passe MySQL si nécessaire.

Les scripts SQL doivent être exécutés dans cet ordre :

```sql
SOURCE C:/chemin/vers/HortiLoc/database/01-create-database.sql;
SOURCE C:/chemin/vers/HortiLoc/database/02-create-tables.sql;
SOURCE C:/chemin/vers/HortiLoc/database/03-insert-test-data.sql;
```

Exemple :

```sql
SOURCE C:/Users/Benja/HortiLoc/database/01-create-database.sql;
SOURCE C:/Users/Benja/HortiLoc/database/02-create-tables.sql;
SOURCE C:/Users/Benja/HortiLoc/database/03-insert-test-data.sql;
```

---

# 14. Rôle des scripts SQL

## 01-create-database.sql

Ce script :

- supprime éventuellement l'ancienne base ;
- crée la base `hortiloc` ;
- configure l'encodage.

## 02-create-tables.sql

Ce script crée les différentes tables du projet :

- clients ;
- utilisateurs ;
- catégories ;
- matériels ;
- locations ;
- détails de locations ;
- maintenances.

Il crée également :

- les clés primaires ;
- les clés étrangères ;
- les contraintes ;
- les relations entre les tables.

## 03-insert-test-data.sql

Ce script ajoute des données de démonstration :

- clients ;
- catégories ;
- matériels ;
- locations ;
- détails de locations ;
- maintenance.

---

# 15. Configuration de MySQL

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

Si MySQL possède un mot de passe pour l'utilisateur `root`, modifier :

```text
Password=;
```

par exemple :

```text
Password=monMotDePasse;
```

---

# 16. Configuration JWT

La configuration JWT se trouve également dans :

```text
backend/HortiLoc.API/appsettings.json
```

Configuration utilisée pour le développement :

```json
{
  "Jwt": {
    "Key": "HortiLoc-Development-Jwt-Key-2026-Change-Me-123456789",
    "Issuer": "HortiLoc.API",
    "Audience": "HortiLoc.Angular"
  }
}
```

Cette clé est destinée uniquement à l'environnement local et scolaire.

Dans une application en production, une clé JWT ne devrait pas être stockée directement dans le dépôt Git.

---

# 17. Installation du frontend

Ouvrir PowerShell dans le projet :

```powershell
cd frontend\hortiloc-web
```

Installer les dépendances :

```powershell
npm install
```

---

# 18. Compilation du backend

Depuis la racine du projet :

```powershell
dotnet build
```

Le backend doit compiler sans erreur.

---

# 19. Compilation du frontend

Depuis :

```powershell
cd frontend\hortiloc-web
```

lancer :

```powershell
ng build
```

Le résultat attendu contient notamment :

```text
Application bundle generation complete.
```

---

# 20. Lancement du backend

Depuis la racine du projet :

```powershell
dotnet run --project backend\HortiLoc.API
```

L'API utilisée pendant le développement est accessible sur :

```text
http://localhost:5177
```

---

# 21. Lancement du frontend

Ouvrir un deuxième terminal :

```powershell
cd frontend\hortiloc-web
ng serve
```

L'application Angular est accessible sur :

```text
http://localhost:4200
```

La page de connexion est :

```text
http://localhost:4200/login
```

---

# 22. Ordre de lancement recommandé

Pour lancer complètement le projet :

```text
1. Démarrer MySQL
2. Exécuter les scripts SQL si nécessaire
3. Lancer l'API ASP.NET Core
4. Lancer Angular
5. Ouvrir http://localhost:4200
6. Se connecter avec un compte de démonstration
```

---

# 23. Pages Angular

## Pages ADMIN

```text
/login
/clients
/categories
/materiels
/locations
/maintenances
```

## Pages CLIENT

```text
/login
/mes-locations
```

---

# 24. Routes principales de l'API

## Authentification

```text
POST /api/auth/login
```

---

## Clients

```text
GET    /api/clients
GET    /api/clients/{id}
POST   /api/clients
PUT    /api/clients/{id}
DELETE /api/clients/{id}
PATCH  /api/clients/{id}/reactiver
```

Ces routes sont réservées au rôle `ADMIN`.

---

## Catégories

```text
GET    /api/categories
GET    /api/categories/{id}
POST   /api/categories
PUT    /api/categories/{id}
DELETE /api/categories/{id}
PATCH  /api/categories/{id}/reactiver
```

Ces routes sont réservées au rôle `ADMIN`.

---

## Matériel

```text
GET    /api/materiels
GET    /api/materiels/{id}
POST   /api/materiels
PUT    /api/materiels/{id}
DELETE /api/materiels/{id}
PATCH  /api/materiels/{id}/reactiver
```

Ces routes sont réservées au rôle `ADMIN`.

---

## Locations ADMIN

```text
GET   /api/locations
GET   /api/locations/{id}
POST  /api/locations
PATCH /api/locations/{id}/retour
```

---

## Locations CLIENT

```text
GET /api/locations/mes-locations
```

Cette route récupère le `clientId` directement depuis le JWT.

---

## Maintenances

```text
GET    /api/maintenances
GET    /api/maintenances/{id}
POST   /api/maintenances
PUT    /api/maintenances/{id}
PATCH  /api/maintenances/{id}/statut
DELETE /api/maintenances/{id}
```

Ces routes sont réservées au rôle `ADMIN`.

---

# 25. Règles métier

## Clients

- le nom est obligatoire ;
- le prénom est obligatoire ;
- l'adresse e-mail doit être unique ;
- un client peut être désactivé puis réactivé.

---

## Catégories

- le nom est obligatoire ;
- le nom doit être unique ;
- une catégorie peut être désactivée puis réactivée.

---

## Matériel

- le matériel doit être associé à une catégorie ;
- le prix journalier ne peut pas être négatif ;
- la quantité totale doit être supérieure à zéro ;
- la quantité totale ne peut pas devenir inférieure à la quantité actuellement louée.

---

## Locations

- le client doit exister ;
- le client doit être actif ;
- une location doit contenir au moins un matériel ;
- le matériel doit exister ;
- le matériel doit être actif ;
- le stock doit être suffisant ;
- la date de fin ne peut pas être antérieure à la date de début ;
- le montant total est calculé côté backend.

Le nombre de jours facturés inclut la date de début et la date de fin.

---

## Retour

- une location déjà retournée ne peut pas être retournée une seconde fois ;
- une location annulée ne peut pas être retournée ;
- le retour remet le matériel dans le stock disponible.

---

## Maintenances

- le matériel doit exister ;
- le matériel doit être actif ;
- une maintenance terminée ne peut plus être modifiée ;
- seule une maintenance `PLANIFIEE` peut être supprimée ;
- une maintenance future ne peut pas être démarrée avant sa date prévue ;
- lorsqu'une maintenance passe à `TERMINEE`, la date de fin est enregistrée.

---

# 26. Transactions Dapper

La création d'une location utilise une transaction.

```text
BEGIN TRANSACTION
        ↓
Création de la location
        ↓
Vérification des stocks
        ↓
Création des détails
        ↓
Mise à jour du stock
        ↓
COMMIT
```

En cas d'erreur :

```text
ROLLBACK
```

Le retour d'une location utilise également une transaction :

```text
BEGIN TRANSACTION
        ↓
Lecture des détails
        ↓
Remise du matériel en stock
        ↓
Mise à jour du statut
        ↓
Enregistrement de la date de retour
        ↓
COMMIT
```

---

# 27. Sécurité

Le projet utilise plusieurs niveaux de sécurité :

- hash des mots de passe ;
- authentification JWT ;
- expiration du token ;
- validation de la signature ;
- validation de l'émetteur ;
- validation de l'audience ;
- rôles `ADMIN` et `CLIENT` ;
- guards Angular ;
- interceptor HTTP ;
- autorisation côté ASP.NET Core ;
- récupération sécurisée du `clientId` depuis le JWT.

Un utilisateur `CLIENT` ne peut pas accéder aux endpoints réservés aux administrateurs.

---

# 28. Test des rôles

Exemple de comportement attendu :

```text
CLIENT
→ GET /api/clients
→ 403 Forbidden
```

```text
ADMIN
→ GET /api/clients
→ 200 OK
```

```text
CLIENT
→ GET /api/locations/mes-locations
→ 200 OK
```

---

# 29. Données de démonstration

La base de données contient notamment :

- 3 clients ;
- 5 catégories ;
- 9 matériels horticoles ;
- des locations ;
- des détails de locations ;
- une maintenance.

Les comptes utilisateur de démonstration sont ensuite créés automatiquement au lancement de l'API en environnement de développement.

---

# 30. Vérification rapide du projet

Backend :

```powershell
cd HortiLoc
dotnet build
```

Frontend :

```powershell
cd frontend\hortiloc-web
ng build
```

Lancer le backend :

```powershell
dotnet run --project backend\HortiLoc.API
```

Lancer Angular :

```powershell
cd frontend\hortiloc-web
ng serve
```

Puis ouvrir :

```text
http://localhost:4200/login
```

Compte ADMIN :

```text
admin@hortiloc.be
Admin123!
```

Compte CLIENT :

```text
client@hortiloc.be
Client123!
```

---

# 31. Dossiers à ne pas inclure dans la remise ZIP

Les dossiers générés automatiquement ne doivent pas être inclus :

```text
node_modules
bin
obj
dist
.angular
.git
.vs
```

Le ZIP doit notamment contenir :

```text
backend
frontend
database
README.md
HortiLoc.slnx
```

---

# 32. Auteur

Projet réalisé dans le cadre du cours :

```text
Angular & .NET
```

Projet :

```text
HortiLoc
```

Sujet :

```text
Gestion de location de matériel horticole
```