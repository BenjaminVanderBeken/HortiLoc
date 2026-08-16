USE hortiloc;

INSERT INTO clients (nom, prenom, email, telephone, adresse)
VALUES
('Dupont', 'Marc', 'marc.dupont@test.be', '0470/11.22.33', 'Rue des Jardins 12, Namur'),
('Martin', 'Julie', 'julie.martin@test.be', '0471/22.33.44', 'Avenue Verte 8, Gembloux'),
('Lambert', 'Pierre', 'pierre.lambert@test.be', '0472/33.44.55', 'Rue du Parc 25, Wavre');

INSERT INTO categories (nom, description)
VALUES
('Tonte', 'Matériel destiné à la tonte des pelouses'),
('Coupe', 'Matériel destiné à la coupe et à la taille'),
('Travail du sol', 'Matériel destiné au travail et à la préparation du sol'),
('Nettoyage', 'Matériel destiné au nettoyage extérieur'),
('Broyage', 'Matériel destiné au broyage des végétaux');

INSERT INTO materiels (
    categorie_id,
    nom,
    description,
    prix_journalier,
    quantite_totale,
    quantite_disponible
)
VALUES
(1, 'Tondeuse thermique Honda', 'Tondeuse thermique autotractée', 35.00, 3, 2),
(2, 'Débroussailleuse Stihl', 'Débroussailleuse thermique professionnelle', 30.00, 2, 2),
(2, 'Taille-haie Stihl', 'Taille-haie thermique', 25.00, 2, 1),
(2, 'Tronçonneuse Husqvarna', 'Tronçonneuse thermique', 40.00, 2, 2),
(3, 'Motoculteur Honda', 'Motoculteur pour préparation du terrain', 55.00, 2, 2),
(3, 'Scarificateur', 'Scarificateur thermique pour pelouse', 40.00, 1, 0),
(4, 'Nettoyeur haute pression Kärcher', 'Nettoyeur haute pression', 30.00, 2, 2),
(4, 'Souffleur Stihl', 'Souffleur thermique pour feuilles', 25.00, 3, 3),
(5, 'Broyeur de branches', 'Broyeur thermique pour branches et déchets verts', 60.00, 1, 1);

INSERT INTO locations (
    client_id,
    date_debut,
    date_fin_prevue,
    statut,
    montant_total,
    notes
)
VALUES
(1, '2026-08-12', '2026-08-15', 'EN_COURS', 180.00, 'Location pour entretien du jardin'),
(2, '2026-08-05', '2026-08-07', 'RETOURNEE', 80.00, 'Matériel retourné en bon état');

INSERT INTO details_locations (
    location_id,
    materiel_id,
    quantite,
    prix_journalier,
    sous_total
)
VALUES
(1, 1, 1, 35.00, 105.00),
(1, 3, 1, 25.00, 75.00),
(2, 6, 1, 40.00, 80.00);

INSERT INTO materiels (
    categorie_id,
    nom,
    description,
    image_url,
    prix_journalier,
    quantite_totale,
    quantite_disponible
)
VALUES
(1, 'Tondeuse thermique Honda', 'Tondeuse thermique autotractée', '/images/materiels/tondeuse.jpg', 35.00, 3, 2),
(2, 'Débroussailleuse Stihl', 'Débroussailleuse thermique professionnelle', '/images/materiels/debroussailleuse.jpg', 30.00, 2, 2),
(2, 'Taille-haie Stihl', 'Taille-haie thermique', '/images/materiels/taille-haie.jpg', 25.00, 2, 1),
(2, 'Tronçonneuse Husqvarna', 'Tronçonneuse thermique', '/images/materiels/tronconneuse.jpg', 40.00, 2, 2),
(3, 'Motoculteur Honda', 'Motoculteur pour préparation du terrain', '/images/materiels/motoculteur.jpg', 55.00, 2, 2),
(3, 'Scarificateur', 'Scarificateur thermique pour pelouse', '/images/materiels/scarificateur.jpg', 40.00, 1, 0),
(4, 'Nettoyeur haute pression Kärcher', 'Nettoyeur haute pression', '/images/materiels/nettoyeur-haute-pression.jpg', 30.00, 2, 2),
(4, 'Souffleur Stihl', 'Souffleur thermique pour feuilles', '/images/materiels/souffleur.jpg', 25.00, 3, 3),
(5, 'Broyeur de branches', 'Broyeur thermique pour branches et déchets verts', '/images/materiels/broyeur.jpg', 60.00, 1, 1);