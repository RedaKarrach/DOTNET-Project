using System.ComponentModel.DataAnnotations;

namespace GestionSalle.DTOs
{
 public class UtilisateurCreateDto
 {
 [Required]
 public string NomUtilisateur { get; set; } = string.Empty;
 [Required]
 [MinLength(8)]
 public string MotDePasse { get; set; } = string.Empty;
 [Required]
 public string Role { get; set; } = string.Empty;
 }

 public record UtilisateurDto(int IdUtilisateur, string NomUtilisateur, string Role);
}
