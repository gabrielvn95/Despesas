using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rastreador_de_despesa.Models;

namespace rastreador_de_despesa.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //Ultimos 7 dias
            DateTime StartDate = DateTime.Today.AddDays(-6);  
            DateTime EndDate = DateTime.Today;
            
            List<Transacao> SelecionarTrasacao = await _context.Transacao
                .Include(x => x.Categoria)
                .Where(y => y.Data>= StartDate && y.Data<=EndDate)
                .ToListAsync();

            //Total Renda
            int TotalRenda = SelecionarTrasacao
                .Where(i => i.Categoria.Tipo == "Renda")
                .Sum(j => j.Quantia);
            ViewBag.TotalRenda = TotalRenda.ToString("C0");

            //Total Despesa

            int TotalDespesa = SelecionarTrasacao
                .Where(i => i.Categoria.Tipo == "Despesa")
                .Sum(j => j.Quantia);
            ViewBag.TotalDespesa = TotalDespesa.ToString("C0");

            //Equilibrio
            int Equilibrio = TotalRenda - TotalDespesa;
            ViewBag.Equilibrio = Equilibrio.ToString("C0");

            return View();
        }
    }
}
