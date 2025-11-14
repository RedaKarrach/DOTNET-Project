using System;
using System.Collections.Generic;

namespace GestionSalle.Models;

public partial class Utilisateur
{
    public int IdUtilisateur { get; set; }

    public string NomUtilisateur { get; set; } = null!;

    public string MotDePasse { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime? DateCreation { get; set; }

    public virtual ICollection<Entraineur> Entraineurs { get; set; } = new List<Entraineur>();

    public virtual ICollection<Membre> Membres { get; set; } = new List<Membre>();
}
