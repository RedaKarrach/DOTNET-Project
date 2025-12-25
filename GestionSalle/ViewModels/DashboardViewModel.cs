namespace GestionSalle.ViewModels
{
    public class DashboardViewModel
    {
        // Statistiques principales
      public int TotalMembres { get; set; }
      public int MembresActifs { get; set; }
        public int MembresInactifs { get; set; }
        public int TotalEntraineurs { get; set; }
        public int EntraineursActifs { get; set; }
        
        // Statistiques financières
        public decimal RevenuMoisActuel { get; set; }
        public decimal RevenuMoisPrecedent { get; set; }
        public decimal RevenuAnnuel { get; set; }
 public int NombrePaiementsMois { get; set; }
 
        // Statistiques des plans
        public Dictionary<string, int> MembresParPlan { get; set; } = new();
     public Dictionary<string, decimal> RevenusParPlan { get; set; } = new();
  
 // Statistiques temporelles
  public int NouveauxMembresCeMois { get; set; }
        public int NouveauxMembresSemaineActuelle { get; set; }
        public int TotalSeances { get; set; }
        public int SeancesCeMois { get; set; }
        
        // Top performers
      public List<TopEntraineurDto> TopEntraineurs { get; set; } = new();
        public List<RecentActivityDto> ActivitesRecentes { get; set; } = new();
  
   // Tendances
        public decimal TauxCroissanceMembres { get; set; }
   public decimal TauxCroissanceRevenu { get; set; }
    }
    
    public class TopEntraineurDto
    {
        public string NomComplet { get; set; } = string.Empty;
        public int NombreMembres { get; set; }
public int NombreSeances { get; set; }
   public string Specialite { get; set; } = string.Empty;
    }
 
    public class RecentActivityDto
    {
  public string Type { get; set; } = string.Empty; // "Membre", "Paiement", "Seance"
        public string Description { get; set; } = string.Empty;
  public DateTime Date { get; set; }
   public string Icon { get; set; } = string.Empty;
        public string BadgeClass { get; set; } = string.Empty;
    }
}
