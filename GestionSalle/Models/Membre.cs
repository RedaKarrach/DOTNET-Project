using System;
using System.Collections.Generic;

namespace GestionSalle.Models;

public partial class Membre
{
    public int IdMembre { get; set; }

    public string NomComplet { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telephone { get; set; }

    public DateTime DateInscription { get; set; }

    public string? Sexe { get; set; }

    public string? Adresse { get; set; }

    public int? IdPlan { get; set; }

    public int? IdEntraineur { get; set; }

    public string? Statut { get; set; }

    public int IdUtilisateur { get; set; }

    public virtual Entraineur? IdEntraineurNavigation { get; set; }

    public virtual PlanAbonnement? IdPlanNavigation { get; set; }

    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;

    public virtual ICollection<Paiement> Paiements { get; set; } = new List<Paiement>();

    public virtual ICollection<Seance> Seances { get; set; } = new List<Seance>();
}
