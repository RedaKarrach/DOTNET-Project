using System.ComponentModel.DataAnnotations;

namespace GestionSalle.DTOs
{
 public record MembreListDto(int IdMembre, string NomComplet, string Email, string? Telephone, string? Statut);

 public class MembreCreateDto
 {
 [Required(ErrorMessage = "Le nom complet est requis")]
 [StringLength(200, ErrorMessage = "Le nom ne peut pas dépasser 200 caractères")]
 public string NomComplet { get; set; } = string.Empty;
 
 [Required(ErrorMessage = "L'email est requis")]
 [EmailAddress(ErrorMessage = "Format d'email invalide")]
 [StringLength(100, ErrorMessage = "L'email ne peut pas dépasser 100 caractères")]
 public string Email { get; set; } = string.Empty;
 
 [Phone(ErrorMessage = "Format de téléphone invalide")]
 [StringLength(20, ErrorMessage = "Le téléphone ne peut pas dépasser 20 caractères")]
 public string? Telephone { get; set; }
 
 [StringLength(300, ErrorMessage = "L'adresse ne peut pas dépasser 300 caractères")]
 public string? Adresse { get; set; }
 
 [StringLength(10, ErrorMessage = "Le sexe ne peut pas dépasser 10 caractères")]
 public string? Sexe { get; set; }
 
 [Display(Name = "Plan d'abonnement")]
 public int? IdPlan { get; set; }
 
 [Display(Name = "Entraîneur")]
 public int? IdEntraineur { get; set; }
 }

 public class MembreUpdateDto : MembreCreateDto { }

 public record MembreDetailsDto(int IdMembre, string NomComplet, string Email, string? Telephone, string? Adresse, string? Statut);
}
