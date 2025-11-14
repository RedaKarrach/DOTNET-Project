using System;
using System.Collections.Generic;

namespace GestionSalle.Models;

public partial class Paiement
{
    public int IdPaiement { get; set; }

    public int IdMembre { get; set; }

    public decimal Montant { get; set; }

    public DateTime DatePaiement { get; set; }

    public string? Methode { get; set; }

    public virtual Membre IdMembreNavigation { get; set; } = null!;
}
