using System.Diagnostics;
using GestionSalle.Models;
using GestionSalle.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionSalle.Controllers
{
    public class HomeController : Controller
    {
        private readonly SalleDbContext _context;

        public HomeController(SalleDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Si l'utilisateur est admin, charger les statistiques
            if (User?.IsInRole("Admin") ?? false)
            {
                var dashboard = await GetDashboardStatisticsAsync();
                return View(dashboard);
            }

            return View();
        }

        private async Task<DashboardViewModel> GetDashboardStatisticsAsync()
        {
            var now = DateTime.Now;
            var debutMois = new DateTime(now.Year, now.Month, 1);
            var finMois = debutMois.AddMonths(1);
            var debutMoisPrecedent = debutMois.AddMonths(-1);
            var debutAnnee = new DateTime(now.Year, 1, 1);
            var debutSemaine = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);

            var dashboard = new DashboardViewModel();

            // Statistiques des membres
            var membres = await _context.Membres.ToListAsync();
            dashboard.TotalMembres = membres.Count;
            dashboard.MembresActifs = membres.Count(m => m.Statut == "Actif");
            dashboard.MembresInactifs = membres.Count(m => m.Statut != "Actif");
            dashboard.NouveauxMembresCeMois = membres.Count(m => m.DateInscription >= debutMois);
            dashboard.NouveauxMembresSemaineActuelle = membres.Count(m => m.DateInscription >= debutSemaine);

            // Statistiques des entraîneurs
            var entraineurs = await _context.Entraineurs.Include(e => e.Membres).Include(e => e.Seances).ToListAsync();
            dashboard.TotalEntraineurs = entraineurs.Count;
            dashboard.EntraineursActifs = entraineurs.Count(e => e.Statut == "Actif");

            // Statistiques financières
            var paiements = await _context.Paiements.Include(p => p.IdMembreNavigation).ThenInclude(m => m.IdPlanNavigation).ToListAsync();
            dashboard.RevenuMoisActuel = paiements.Where(p => p.DatePaiement >= debutMois && p.DatePaiement < finMois).Sum(p => p.Montant);
            dashboard.RevenuMoisPrecedent = paiements.Where(p => p.DatePaiement >= debutMoisPrecedent && p.DatePaiement < debutMois).Sum(p => p.Montant);
            dashboard.RevenuAnnuel = paiements.Where(p => p.DatePaiement >= debutAnnee).Sum(p => p.Montant);
            dashboard.NombrePaiementsMois = paiements.Count(p => p.DatePaiement >= debutMois && p.DatePaiement < finMois);

            // Statistiques des séances
            var seances = await _context.Seances.ToListAsync();
            dashboard.TotalSeances = seances.Count;
            dashboard.SeancesCeMois = seances.Count(s => s.Date >= debutMois);

            // Membres par plan
            var plans = await _context.PlanAbonnements.Include(p => p.Membres).ToListAsync();
            foreach (var plan in plans)
            {
                dashboard.MembresParPlan[plan.Nom] = plan.Membres.Count;
                var revenuPlan = paiements.Where(p => p.IdMembreNavigation?.IdPlan == plan.IdPlan && p.DatePaiement >= debutMois).Sum(p => p.Montant);
                dashboard.RevenusParPlan[plan.Nom] = revenuPlan;
            }

            // Top entraîneurs
            dashboard.TopEntraineurs = entraineurs
                .OrderByDescending(e => e.Membres.Count)
                .Take(5)
                .Select(e => new TopEntraineurDto
                {
                    NomComplet = e.NomComplet,
                    NombreMembres = e.Membres.Count,
                    NombreSeances = e.Seances.Count,
                    Specialite = e.Specialite ?? "Non spécifié"
                })
                .ToList();

            // Activités récentes
            var recentActivities = new List<RecentActivityDto>();

            // Derniers membres inscrits
            var derniersMembres = membres.OrderByDescending(m => m.DateInscription).Take(5);
            foreach (var membre in derniersMembres)
            {
                recentActivities.Add(new RecentActivityDto
                {
                    Type = "Membre",
                    Description = $"Nouveau membre : {membre.NomComplet}",
                    Date = membre.DateInscription,
                    Icon = "fa-user-plus",
                    BadgeClass = "bg-success"
                });
            }

            // Derniers paiements
            var derniersPaiements = paiements.OrderByDescending(p => p.DatePaiement).Take(5);
            foreach (var paiement in derniersPaiements)
            {
                recentActivities.Add(new RecentActivityDto
                {
                    Type = "Paiement",
                    Description = $"Paiement de {paiement.Montant:N2} DH par {paiement.IdMembreNavigation?.NomComplet}",
                    Date = paiement.DatePaiement,
                    Icon = "fa-money-bill-wave",
                    BadgeClass = "bg-primary"
                });
            }

            dashboard.ActivitesRecentes = recentActivities.OrderByDescending(a => a.Date).Take(10).ToList();

            // Calcul des taux de croissance
            var membresDebutMois = membres.Count(m => m.DateInscription < debutMois);
            if (membresDebutMois > 0)
            {
                dashboard.TauxCroissanceMembres = ((decimal)dashboard.NouveauxMembresCeMois / membresDebutMois) * 100;
            }

            if (dashboard.RevenuMoisPrecedent > 0)
            {
                dashboard.TauxCroissanceRevenu = ((dashboard.RevenuMoisActuel - dashboard.RevenuMoisPrecedent) / dashboard.RevenuMoisPrecedent) * 100;
            }

            return dashboard;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
