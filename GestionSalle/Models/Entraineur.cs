using System;
using System.Collections.Generic;

namespace GestionSalle.Models;

public partial class Entraineur
{
    public int IdEntraineur { get; set; }

    public string NomComplet { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Specialite { get; set; }

    public string? Telephone { get; set; }

    public string? Statut { get; set; }

    public int IdUtilisateur { get; set; }

    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;

    public virtual ICollection<Membre> Membres { get; set; } = new List<Membre>();

    public virtual ICollection<Seance> Seances { get; set; } = new List<Seance>();
}
