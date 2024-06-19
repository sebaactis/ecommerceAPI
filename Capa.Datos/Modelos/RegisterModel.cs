using System.ComponentModel.DataAnnotations;

namespace Capa.Datos.Modelos
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "El username es obligatorio.")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "El username debe tener minimo 8 caracteres y maximo 15 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene el formato correcto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres, conetener 1 minuscula, 1 mayuscula, y 1 caracter alfanumerico")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
