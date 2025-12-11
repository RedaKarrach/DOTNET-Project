using System.ComponentModel.DataAnnotations;

namespace GestionSalle.DTOs
{
 public record MembreListDto(int IdMembre, string NomComplet, string Email, string? Telephone, string? Statut);

 public class MembreCreateDto
 {
 [Required]
 public string NomComplet { get; set; } = string.Empty;
 [Required]
 [EmailAddress]
 public string Email { get; set; } = string.Empty;
 public string? Telephone { get; set; }
 public string? Adresse { get; set; }
 public int? IdPlan { get; set; }
 public int? IdEntraineur { get; set; }
 public int IdUtilisateur { get; set; }
 }

 public class MembreUpdateDto : MembreCreateDto { }

 public record MembreDetailsDto(int IdMembre, string NomComplet, string Email, string? Telephone, string? Adresse, string? Statut);
}
