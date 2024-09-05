using Microsoft.AspNetCore.Mvc;
using CadastroFornecedor.Data;
using CadastroFornecedor.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace CadastroFornecedor.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FornecedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Fornecedores.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,CNPJ,Segmento,CEP,Endereco")] Fornecedor fornecedor, IFormFile Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null && Imagem.Length > 0)
                {
                    if (!Imagem.ContentType.Equals("image/png", StringComparison.OrdinalIgnoreCase) ||
                        Path.GetExtension(Imagem.FileName).ToLower() != ".png")
                    {
                        ModelState.AddModelError("Imagem", "O arquivo deve ser um PNG com extensão .png.");
                        return View(fornecedor);
                    }
                    var caminhoImagem = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Imagem.FileName);
                    using (var stream = new FileStream(caminhoImagem, FileMode.Create))
                    {
                        await Imagem.CopyToAsync(stream);
                    }
                    fornecedor.ImagemUrl = "/images/" + Imagem.FileName;
                }

                _context.Add(fornecedor);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fornecedor cadastrado com sucesso!";
                return RedirectToAction(nameof(Success));
            }
            return View(fornecedor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
            {
                return NotFound();
            }
            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,CNPJ,Segmento,CEP,Endereco,ImagemUrl")] Fornecedor fornecedor, IFormFile Imagem)
        {
            if (id != fornecedor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Imagem != null && Imagem.Length > 0)
                    {
                        if (!Imagem.ContentType.Equals("image/png", StringComparison.OrdinalIgnoreCase) ||
                            Path.GetExtension(Imagem.FileName).ToLower() != ".png")
                        {
                            ModelState.AddModelError("Imagem", "O arquivo deve ser um PNG com extensão .png.");
                            return View(fornecedor);
                        }
                        var caminhoImagem = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Imagem.FileName);
                        using (var stream = new FileStream(caminhoImagem, FileMode.Create))
                        {
                            await Imagem.CopyToAsync(stream);
                        }
                        fornecedor.ImagemUrl = "/images/" + Imagem.FileName;
                    }

                    _context.Update(fornecedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FornecedorExists(fornecedor.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores
                .FirstOrDefaultAsync(m => m.ID == id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool FornecedorExists(int id)
        {
            return _context.Fornecedores.Any(e => e.ID == id);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}