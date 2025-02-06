using System.ComponentModel.DataAnnotations;

namespace rastreador_de_despesa.Dto
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "Digite a Email!")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Digite a Senha!")]
        public string Senha { get; set; }
    }
}
