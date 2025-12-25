-- Script rapide pour vérifier et insérer les plans d'abonnement
-- À exécuter dans la console Package Manager de Visual Studio:
-- Tools > NuGet Package Manager > Package Manager Console
-- Puis tapez: Invoke-Sqlcmd -InputFile "GestionSalle\SeedPlans.sql"

-- OU exécutez ces commandes SQL directement:

-- 1. Vérifier la base de données actuelle
SELECT DB_NAME() AS CurrentDatabase;

-- 2. Vérifier les plans existants
SELECT COUNT(*) AS NombreDePlans FROM PlanAbonnement;

-- 3. Si la table est vide, insérer les plans
IF NOT EXISTS (SELECT 1 FROM PlanAbonnement)
BEGIN
    INSERT INTO PlanAbonnement (nom, description, prix, dureeEnMois)
 VALUES 
        (N'Plan Basique', N'Accès à la salle de sport avec équipements de base', 200, 1),
        (N'Plan Standard', N'Accès à la salle + cours collectifs', 500, 3),
        (N'Plan Premium', N'Accès complet + entraîneur personnel + cours collectifs', 1500, 6),
     (N'Plan Annuel', N'Accès complet pour toute l''année avec tous les services', 2500, 12);
    
    SELECT 'Plans insérés avec succès!' AS Message;
END
ELSE
BEGIN
    SELECT 'Des plans existent déjà.' AS Message;
END

-- 4. Afficher tous les plans avec les prix en DH
SELECT 
    idPlan AS [ID],
    nom AS [Nom du Plan],
    description AS [Description],
    CAST(prix AS VARCHAR) + ' DH' AS [Prix],
    dureeEnMois AS [Durée (mois)]
FROM PlanAbonnement
ORDER BY prix;
