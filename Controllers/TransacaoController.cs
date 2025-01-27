using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rastreador_de_despesa.Models;

namespace rastreador_de_despesa.Controllers
{
    public class TransacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transacao
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transacao.Include(t => t.Categoria);
            return View(await applicationDbContext.ToListAsync());
        }

        

        // GET: Transacao/Create
        public IActionResult AddOrEdit(int id=0)
        {
            PopulateCategories();
            if(id == 0)
                return View(new Transacao());
            else
                return View(_context.Transacao.Find(id));

        }

        // POST: Transacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TrasacaoId,CategoriaId,Quantia,Nota,Data")] Transacao transacao)
        {
            if (ModelState.IsValid)
            {
                if(transacao.TrasacaoId == 0)
                _context.Add(transacao);
                else
                    _context.Update(transacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCategories();
            return View(transacao);
        }


        // POST: Transacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transacao = await _context.Transacao.FindAsync(id);
            if (transacao != null)
            {
                _context.Transacao.Remove(transacao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public void PopulateCategories()
        {
            var CategoryCollection = _context.Categorias.ToList();
            Categoria DefaultCategory = new Categoria() { CategoriaId = 0, Titulo = "Escolha a categoria" };
            CategoryCollection.Insert(0, DefaultCategory);
            ViewBag.Categorias = CategoryCollection; 
        }
    }
}
