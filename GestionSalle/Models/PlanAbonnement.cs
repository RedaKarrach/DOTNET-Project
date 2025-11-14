using System;
using System.Collections.Generic;

namespace GestionSalle.Models;

public partial class PlanAbonnement
{
    public int IdPlan { get; set; }

    public string Nom { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Prix { get; set; }

    public int DureeEnMois { get; set; }

    public virtual ICollection<Membre> Membres { get; set; } = new List<Membre>();
}
