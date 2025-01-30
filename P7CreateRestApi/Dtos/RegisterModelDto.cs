using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dtos
{
    public class RegisterModelDto
    {

        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        [StringLength(255, ErrorMessage = "L'email ne peut pas dépasser 255 caractères.")]

        public string? Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Le mot de passe doit comporter au moins {2} caractères.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
        ErrorMessage = "Le mot de passe doit contenir au moins une lettre majuscule, une lettre minuscule, un chiffre et un caractère spécial")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire.")]

        public string? FullName { get; set; }
    }
}
