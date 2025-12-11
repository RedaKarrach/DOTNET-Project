using System.ComponentModel.DataAnnotations;

namespace GestionSalle.DTOs
{
 public class PaiementCreateDto
 {
 [Required]
 public int IdMembre { get; set; }

 [Required]
 [Range(0.01,1000000)]
 public decimal Montant { get; set; }

 [MaxLength(50)]
 public string? Methode { get; set; }
 }

 public record PaiementListDto(int IdPaiement, int IdMembre, string MembreNom, decimal Montant, System.DateTime DatePaiement, string? Methode);
}
