using System.ComponentModel.DataAnnotations;
using GestionSalle.Enums;

namespace GestionSalle.DTOs
{
    public class EntraineurCreateDto
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

[Required(ErrorMessage = "La spécialité est requise")]
     [Display(Name = "Spécialité")]
        public SpecialiteEntraineur Specialite { get; set; }
    }

 public class EntraineurUpdateDto : EntraineurCreateDto { }

    public record EntraineurListDto(int IdEntraineur, string NomComplet, string Email, string? Telephone, string Specialite, string? Statut);
}
