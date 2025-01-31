using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rastreador_de_despesa.Models;
using Syncfusion.EJ2.Charts;
using System.Globalization;

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
            CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Equilibrio = String.Format(culture, "{0:C0}", Equilibrio);

            //Grafico chart - Despesas i Categorias
            ViewBag.DoughnutChartData = SelecionarTrasacao
                .Where(i => i.Categoria.Tipo == "Despesa")
                .GroupBy(j => j.Categoria.CategoriaId)
                .Select(k => new
                {
                    categoriaTituloComIcone = k.First().Categoria.Icone+" "+ k.First().Categoria.Titulo,
                    quantia = k.Sum(j => j.Quantia),
                    formattedQuantia = k.Sum(j => j.Quantia).ToString("C0"),
                })
                .OrderByDescending(l=>l.quantia)
                .ToList();

            // soline chart - Renda vs Despesa
            //quantia
            List<SplineChartData> RendaSummary = SelecionarTrasacao
            .Where(i => i.Categoria.Tipo == "Renda")
            .GroupBy(j => j.Data)
            .Select(k => new SplineChartData()
            {
                dia = k.First().Data.ToString("dd-MMM"),
                renda = k.Sum(l => l.Quantia)
            })
            .ToList();

            //Despesa

            List<SplineChartData> DespesaSummary = SelecionarTrasacao
            .Where(i => i.Categoria.Tipo == "Despesa")
            .GroupBy(j => j.Data)
            .Select(k => new SplineChartData()
            {
                dia = k.First().Data.ToString("dd-MMM"),
                despesa = k.Sum(l => l.Quantia)
            })
            .ToList();


            //combinando Renda & Despesa
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from dia in Last7Days
                                      join renda in RendaSummary on dia equals renda.dia
                                      into diaRendaJoined
                                      from renda in diaRendaJoined.DefaultIfEmpty()
                                      join despesa in DespesaSummary on dia equals despesa.dia into despesaJoined
                                      from despesa in despesaJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          dia = dia,
                                          renda = renda == null ? 0 : renda.renda,
                                          despesa = despesa == null ? 0 : despesa.despesa,

                                      };

            //transacao recente
            ViewBag.TransacaoRecente = await _context.Transacao
                .Include(i => i.Categoria)
                .OrderByDescending(j => j.Data)
                .Take(5)
                .ToListAsync();



            return View();
        }
    }

    public class SplineChartData
    {
        public string dia;
        public int renda;
        public int despesa;
    }
}
