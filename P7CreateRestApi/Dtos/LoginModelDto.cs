using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dtos
{
    public class LoginModelDto
    {
        //[Required(ErrorMessage ="L'email est obligatoire.")]
        //[EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        //public string? Email { get; set; }

        [Required(ErrorMessage = "Le champ est obligatoire.")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "Le champ est obligatoire.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
