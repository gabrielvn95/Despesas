using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rastreador_de_despesa.Models
{
    public class Transacao
    {
        [Key]
        public int TrasacaoId { get; set; }

        [Range(1, int.MaxValue,ErrorMessage = "Por favor selecione uma categoria")]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantia deve ser maior que 0")]
        public int Quantia { get; set; }

         [Column(TypeName = "nvarchar(75)")]
        public string? Nota { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;

        [NotMapped]
        public string? CategoriaTituloComIcone  {
            get
            {
                return Categoria == null ? "" : Categoria.Icone + " " + Categoria.Titulo;
            }
        }

        [NotMapped]

        public string? FormatandoQuantia
        {
            get
            {
                return ((Categoria == null || Categoria.Tipo== "Despesa" )? "- " : "+ ") + Quantia.ToString("C0");
            }
        }
    }
}
