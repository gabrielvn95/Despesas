using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rastreador_de_despesa.Models
{
    public class Transacao
    {
        [Key]
        public int TrasacaoId { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public int Quantia { get; set; }

         [Column(TypeName = "nvarchar(75)")]
        public string? Nota { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;
    }
}
