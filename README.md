# HortiLoc

HortiLoc est une application web de gestion de location de matériel horticole.

Le projet utilise :

- Angular pour le frontend ;
- ASP.NET Core pour l'API ;
- Dapper pour l'accès aux données ;
- MySQL pour la base de données ;
- JWT pour l'authentification.

Le backend respecte une Clean Architecture avec une séparation entre :

- API ;
- Core ;
- Infrastructure.

Deux rôles sont disponibles :

- `ADMIN`
- `CLIENT`

---

# 1. Prérequis et versions

Avant de lancer le projet, installer :

## Backend

- .NET SDK : `10.0.301`
- ASP.NET Core
- C#
- Dapper
- MySql.Data

## Frontend

- Angular CLI : `21.2.21`
- Angular : `21.2.20`
- Node.js : `24.14.1`
- npm : `11.11.0`
- TypeScript : `5.9.3`
- RxJS : `7.8.2`

## Base de données

- MySQL Community Server : `9.5.0`

## Outils utilisés

- Windows 64 bits
- Visual Studio Code
- Git / GitHub

Les versions installées peuvent être vérifiées avec :

```powershell
dotnet --version
node --version
npm --version
ng version
mysql --version
git --version
```

---

# 2. Récupération du projet

Le projet peut être récupéré depuis GitHub :

```powershell
git clone https://github.com/BenjaminVanderBeken/HortiLoc.git
cd HortiLoc
```

Il est également possible d'utiliser directement l'archive ZIP fournie sur Moodle.

---

# 3. Création de la base de données

Démarrer MySQL puis ouvrir un terminal :

```powershell
mysql --default-character-set=utf8mb4 -u root -p
```

Entrer le mot de passe MySQL si nécessaire.

Les scripts SQL se trouvent dans :

```text
database/
```

Ils doivent être exécutés dans cet ordre :

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

Les scripts ont les rôles suivants :

- `01-create-database.sql` : création de la base `hortiloc` ;
- `02-create-tables.sql` : création des tables, relations et contraintes ;
- `03-insert-test-data.sql` : ajout des données de démonstration.

Les principales tables sont :

```text
clients
utilisateurs
categories
materiels
locations
details_locations
maintenances
```

---

# 4. Configuration de MySQL

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

Si l'utilisateur MySQL `root` possède un mot de passe, modifier la propriété `Password`.

Exemple :

```text
Password=monMotDePasse;
```

---

# 5. Installation du frontend

Depuis la racine du projet :

```powershell
cd frontend\hortiloc-web
npm install
```

Cette commande installe les dépendances Angular nécessaires.

---

# 6. Lancement du backend

Ouvrir un premier terminal PowerShell à la racine du projet :

```powershell
cd C:\chemin\vers\HortiLoc
dotnet run --project backend\HortiLoc.API
```

L'API utilisée par le projet est accessible sur :

```text
http://localhost:5177
```

Laisser ce terminal ouvert pendant l'utilisation de l'application.

---

# 7. Lancement du frontend

Ouvrir un deuxième terminal PowerShell :

```powershell
cd C:\chemin\vers\HortiLoc\frontend\hortiloc-web
ng serve
```

L'application Angular est ensuite accessible sur :

```text
http://localhost:4200
```

Page de connexion :

```text
http://localhost:4200/login
```

---

# 8. Ordre de lancement recommandé

Pour démarrer complètement HortiLoc :

```text
1. Démarrer MySQL
2. Exécuter les scripts SQL lors de la première installation
3. Vérifier la chaîne de connexion MySQL
4. Lancer l'API ASP.NET Core
5. Lancer Angular
6. Ouvrir http://localhost:4200
7. Se connecter avec un compte de démonstration
```

---

# 9. Comptes de démonstration

Les comptes de démonstration sont créés automatiquement au démarrage de l'API en environnement de développement s'ils n'existent pas encore.

## Administrateur

```text
Email : admin@hortiloc.be
Mot de passe : Admin123!
Rôle : ADMIN
```

L'administrateur dispose des fonctionnalités de gestion de l'application.

## Client

```text
Email : client@hortiloc.be
Mot de passe : Client123!
Rôle : CLIENT
Client associé : id 1
```

Le client peut consulter uniquement ses propres locations.

Les mots de passe sont enregistrés sous forme hashée dans la base de données.

---

# 10. Fonctionnalités principales

HortiLoc propose plusieurs fonctionnalités distinctes.

## Gestion des clients

L'administrateur peut :

- consulter les clients ;
- ajouter un client ;
- modifier un client ;
- désactiver un client ;
- réactiver un client.

L'adresse e-mail d'un client doit être unique.

## Gestion des catégories

L'administrateur peut :

- consulter les catégories ;
- ajouter une catégorie ;
- modifier une catégorie ;
- désactiver une catégorie ;
- réactiver une catégorie.

## Gestion du matériel

L'administrateur peut :

- consulter le matériel ;
- ajouter un matériel ;
- modifier un matériel ;
- désactiver un matériel ;
- réactiver un matériel ;
- associer un matériel à une catégorie ;
- définir un prix journalier ;
- gérer les quantités disponibles ;
- associer une image à un matériel.

## Gestion des locations

L'administrateur peut créer une location avec :

- un client ;
- une ou plusieurs lignes de matériel ;
- une quantité pour chaque matériel ;
- une date de début ;
- une date de fin prévue ;
- des notes.

Le backend :

- vérifie le stock ;
- vérifie les règles métier ;
- calcule le montant total ;
- crée les détails de location ;
- diminue automatiquement le stock disponible.

## Retour du matériel

Lors du retour :

- le statut passe à `RETOURNEE` ;
- la date de retour est enregistrée ;
- les quantités louées sont remises dans le stock.

Le retour utilise une transaction afin de maintenir la cohérence des données.

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

## Authentification

L'application utilise une authentification JWT avec deux rôles :

```text
ADMIN
CLIENT
```

## Espace client

Le rôle `CLIENT` dispose de la page :

```text
Mes locations
```

Cette page affiche uniquement les locations liées au client connecté.

Le `clientId` est récupéré côté API depuis le token JWT.

---

# 11. Architecture du projet

Structure générale :

```text
HortiLoc
│
├── backend
│   │
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

## HortiLoc.API

Responsabilités :

- exposer les endpoints HTTP ;
- recevoir les requêtes du frontend ;
- appeler les services du Core ;
- gérer l'authentification et les autorisations ;
- retourner les réponses HTTP.

Les contrôleurs ne contiennent aucune requête SQL.

## HortiLoc.Core

Responsabilités :

- entités ;
- DTOs ;
- interfaces des repositories ;
- services métier ;
- règles métier.

Le Core ne contient pas les requêtes SQL.

## HortiLoc.Infrastructure

Responsabilités :

- connexion à MySQL ;
- implémentation des repositories ;
- requêtes SQL ;
- utilisation de Dapper ;
- services techniques.

La communication avec MySQL est réalisée avec Dapper.

Entity Framework n'est pas utilisé.

---

# 12. Flux applicatif

Le flux général d'une donnée est :

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

La réponse remonte ensuite jusqu'au service Angular et à l'interface utilisateur.

---

# 13. Angular

Le frontend utilise Angular moderne avec notamment :

- composants standalone ;
- `@if` ;
- `@for` ;
- Services Angular ;
- Signals ;
- Reactive Forms ;
- HttpClient ;
- Routing ;
- Guards ;
- Interceptor HTTP.

La gestion de l'état applicatif est réalisée exclusivement via les Services Angular.

Les signals modifiables sont privés dans les services et sont exposés en lecture seule aux composants.

Aucune bibliothèque externe de gestion d'état telle que NgRx ou Redux n'est utilisée.

---

# 14. Authentification et sécurité

L'application utilise JWT pour l'authentification.

Le flux de connexion est :

```text
Login Angular
      ↓
AuthService Angular
      ↓
POST /api/auth/login
      ↓
AuthController
      ↓
Core
      ↓
Repository
      ↓
Dapper / MySQL
      ↓
Vérification du mot de passe
      ↓
Création du JWT
      ↓
Retour à Angular
```

Un interceptor Angular ajoute ensuite le token aux requêtes protégées :

```text
Authorization: Bearer <token>
```

La sécurité repose également sur :

- le hash des mots de passe ;
- l'expiration du JWT ;
- la validation de la signature ;
- les rôles `ADMIN` et `CLIENT` ;
- les guards Angular ;
- les autorisations ASP.NET Core.

Les contrôles de sécurité sont réalisés côté backend et ne reposent donc pas uniquement sur l'interface Angular.

---

# 15. Principales règles métier

## Clients

- le nom est obligatoire ;
- le prénom est obligatoire ;
- l'adresse e-mail doit être unique ;
- un client peut être désactivé puis réactivé.

## Catégories

- le nom est obligatoire ;
- le nom doit être unique ;
- une catégorie peut être désactivée puis réactivée.

## Matériel

- un matériel doit être associé à une catégorie ;
- le prix journalier ne peut pas être négatif ;
- la quantité totale doit être supérieure à zéro ;
- la quantité totale ne peut pas devenir inférieure à la quantité actuellement louée.

## Locations

- le client doit exister et être actif ;
- une location contient au moins un matériel ;
- le matériel doit exister et être actif ;
- le stock disponible doit être suffisant ;
- la date de fin ne peut pas être antérieure à la date de début ;
- le montant total est calculé côté backend.

Le nombre de jours facturés inclut la date de début et la date de fin.

## Retour

- une location déjà retournée ne peut pas être retournée une seconde fois ;
- une location annulée ne peut pas être retournée ;
- le retour remet le matériel dans le stock disponible.

## Maintenances

- le matériel doit exister et être actif ;
- une maintenance terminée ne peut plus être modifiée ;
- seule une maintenance `PLANIFIEE` peut être supprimée ;
- une maintenance future ne peut pas être démarrée avant sa date prévue ;
- lorsque la maintenance passe à `TERMINEE`, sa date de fin est enregistrée.

---

# 16. Transactions Dapper

La création d'une location utilise une transaction.

```text
BEGIN TRANSACTION
        ↓
Création de la location
        ↓
Vérification du stock
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

Le retour d'une location utilise également une transaction afin de remettre le matériel en stock et de mettre à jour la location de manière cohérente.

---

# 17. Données de démonstration

Après exécution des scripts SQL, la base contient notamment :

- 3 clients ;
- 5 catégories ;
- 9 matériels horticoles avec images ;
- des locations ;
- des détails de locations ;
- une maintenance.

Les comptes ADMIN et CLIENT sont créés automatiquement au démarrage de l'API en environnement de développement.

---

# 18. Vérification du projet

Pour vérifier le backend depuis la racine :

```powershell
dotnet build
```

Pour vérifier le frontend :

```powershell
cd frontend\hortiloc-web
ng build
```

Résultat attendu pour Angular :

```text
Application bundle generation complete.
```

---

# 19. Projet

Projet réalisé dans le cadre du cours :

```text
Angular & .NET
```

Sujet :

```text
HortiLoc
Gestion de location de matériel horticole
```