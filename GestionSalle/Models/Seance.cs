using System;
using System.Collections.Generic;

namespace GestionSalle.Models;

public partial class Seance
{
    public int IdSeance { get; set; }

    public int? IdMembre { get; set; }

    public int? IdEntraineur { get; set; }

    public string? Description { get; set; }

    public string Type { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual Entraineur? IdEntraineurNavigation { get; set; }

    public virtual Membre? IdMembreNavigation { get; set; }
}
