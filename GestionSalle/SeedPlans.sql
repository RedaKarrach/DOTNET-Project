-- Script pour insérer les plans d'abonnement manuellement si besoin
-- Exécutez ce script dans SQL Server Management Studio ou via la console Package Manager

USE salle_db;
GO

-- Vérifier si la table est vide
IF NOT EXISTS (SELECT 1 FROM PlanAbonnement)
BEGIN
    -- Insérer les plans par défaut
    INSERT INTO PlanAbonnement (nom, description, prix, dureeEnMois)
    VALUES 
        (N'Plan Basique', N'Accès à la salle de sport avec équipements de base', 200, 1),
        (N'Plan Standard', N'Accès à la salle + cours collectifs', 500, 3),
        (N'Plan Premium', N'Accès complet + entraîneur personnel + cours collectifs', 1500, 6),
        (N'Plan Annuel', N'Accès complet pour toute l''année avec tous les services', 2500, 12);
    
 PRINT 'Plans d''abonnement insérés avec succès';
END
ELSE
BEGIN
    PRINT 'Des plans existent déjà dans la base de données';
END
GO

-- Afficher tous les plans avec les prix en DH
SELECT 
    idPlan AS [ID],
    nom AS [Nom du Plan],
    description AS [Description],
    CAST(prix AS VARCHAR) + ' DH' AS [Prix],
    dureeEnMois AS [Durée (mois)]
FROM PlanAbonnement
ORDER BY prix;
GO
