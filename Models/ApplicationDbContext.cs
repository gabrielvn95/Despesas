using Microsoft.EntityFrameworkCore;

namespace rastreador_de_despesa.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options) 
        {

        }

        public DbSet<Transacao>  Transacao{ get; set; }
        public DbSet<Categoria>  Categorias{ get; set; }
    }
}
