USE hortiloc;

CREATE TABLE clients (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prenom VARCHAR(100) NOT NULL,
    email VARCHAR(150) UNIQUE,
    telephone VARCHAR(30),
    adresse VARCHAR(255),
    actif BOOLEAN NOT NULL DEFAULT TRUE,
    date_creation DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modification DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE utilisateurs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    client_id INT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    mot_de_passe_hash VARCHAR(255) NOT NULL,
    role ENUM('ADMIN', 'CLIENT') NOT NULL,
    actif BOOLEAN NOT NULL DEFAULT TRUE,
    date_creation DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_utilisateurs_clients
        FOREIGN KEY (client_id)
        REFERENCES clients(id)
        ON DELETE CASCADE
);

CREATE TABLE categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(255),
    actif BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE materiels (
    id INT AUTO_INCREMENT PRIMARY KEY,
    categorie_id INT NOT NULL,
    nom VARCHAR(150) NOT NULL,
    description VARCHAR(500),
    prix_journalier DECIMAL(10,2) NOT NULL,
    quantite_totale INT NOT NULL DEFAULT 1,
    quantite_disponible INT NOT NULL DEFAULT 1,
    actif BOOLEAN NOT NULL DEFAULT TRUE,
    date_creation DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    date_modification DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT fk_materiels_categories
        FOREIGN KEY (categorie_id)
        REFERENCES categories(id)
        ON DELETE RESTRICT,

    CONSTRAINT chk_materiels_prix
        CHECK (prix_journalier >= 0),

    CONSTRAINT chk_materiels_quantite_totale
        CHECK (quantite_totale >= 0),

    CONSTRAINT chk_materiels_quantite_disponible
        CHECK (
            quantite_disponible >= 0
            AND quantite_disponible <= quantite_totale
        )
);

CREATE TABLE locations (
    id INT AUTO_INCREMENT PRIMARY KEY,
    client_id INT NOT NULL,
    date_debut DATE NOT NULL,
    date_fin_prevue DATE NOT NULL,
    date_retour DATE NULL,
    statut ENUM(
        'EN_ATTENTE',
        'EN_COURS',
        'RETOURNEE',
        'ANNULEE'
    ) NOT NULL DEFAULT 'EN_ATTENTE',
    montant_total DECIMAL(10,2) NOT NULL DEFAULT 0,
    notes VARCHAR(500),
    date_creation DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_locations_clients
        FOREIGN KEY (client_id)
        REFERENCES clients(id)
        ON DELETE RESTRICT,

    CONSTRAINT chk_locations_dates
        CHECK (date_fin_prevue >= date_debut),

    CONSTRAINT chk_locations_montant
        CHECK (montant_total >= 0)
);

CREATE TABLE details_locations (
    id INT AUTO_INCREMENT PRIMARY KEY,
    location_id INT NOT NULL,
    materiel_id INT NOT NULL,
    quantite INT NOT NULL,
    prix_journalier DECIMAL(10,2) NOT NULL,
    sous_total DECIMAL(10,2) NOT NULL,

    CONSTRAINT fk_details_locations_locations
        FOREIGN KEY (location_id)
        REFERENCES locations(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_details_locations_materiels
        FOREIGN KEY (materiel_id)
        REFERENCES materiels(id)
        ON DELETE RESTRICT,

    CONSTRAINT chk_details_locations_quantite
        CHECK (quantite > 0),

    CONSTRAINT chk_details_locations_prix
        CHECK (prix_journalier >= 0),

    CONSTRAINT chk_details_locations_sous_total
        CHECK (sous_total >= 0)
);

CREATE TABLE maintenances (
    id INT AUTO_INCREMENT PRIMARY KEY,
    materiel_id INT NOT NULL,
    date_debut DATE NOT NULL,
    date_fin DATE NULL,
    motif VARCHAR(500) NOT NULL,
    statut ENUM(
        'PLANIFIEE',
        'EN_COURS',
        'TERMINEE'
    ) NOT NULL DEFAULT 'PLANIFIEE',
    date_creation DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_maintenances_materiels
        FOREIGN KEY (materiel_id)
        REFERENCES materiels(id)
        ON DELETE RESTRICT
);