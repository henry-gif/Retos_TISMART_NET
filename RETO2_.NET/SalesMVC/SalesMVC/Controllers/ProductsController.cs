using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesMVC.Models;

namespace SalesMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BDSALESContext _context;

        public ProductsController(BDSALESContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string buscar, string filtro)
        {

            var productos = _context.Products.Include(p => p.CategoryIdfkNavigation).AsQueryable();

           
            if (!string.IsNullOrEmpty(buscar))
            {
                buscar = buscar.ToLower(); 

                productos = productos.Where(p => p.ProductName.ToLower().Contains(buscar) ||
                                                  p.CategoryIdfkNavigation.CategoryDescription.ToLower().Contains(buscar) ||
                                                  p.Price.ToString().Contains(buscar));
            }

            // Aplicar filtro para cada campo
            ViewData["CurrentSort"] = filtro;
            ViewData["FiltroNombre"] = string.IsNullOrEmpty(filtro) ? "NameDesc" : "";
            ViewData["FiltroPrice"] = filtro == "PriceAsc" ? "PriceDesc" : "PriceAsc";
            ViewData["FiltroCategoria"] = filtro == "CategoryAsc" ? "CategoryDesc" : "CategoryAsc";

            switch (filtro)
            {
                case "NameDesc":
                    productos = productos.OrderByDescending(p => p.ProductName);
                    break;
                case "PriceAsc":
                    productos = productos.OrderBy(p => p.Price);
                    break;
                case "PriceDesc":
                    productos = productos.OrderByDescending(p => p.Price);
                    break;
                case "CategoryAsc":
                    productos = productos.OrderBy(p => p.CategoryIdfkNavigation.CategoryDescription);
                    break;
                case "CategoryDesc":
                    productos = productos.OrderByDescending(p => p.CategoryIdfkNavigation.CategoryDescription);
                    break;
                default:
                    productos = productos.OrderBy(p => p.ProductId);
                    break;
            }

            return View(await productos.ToListAsync());
        }

        // GET: Productos/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.CategoryIdfkNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            // Usar CategoryDescription en lugar de CategoryId para mostrar en el dropdown
            ViewData["CategoryIdfk"] = new SelectList(_context.Categories, "CategoryId", "CategoryDescription");
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Price,CategoryIdfk")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Volver a cargar las categorías para que el dropdown funcione después de un error de validación
            ViewData["CategoryIdfk"] = new SelectList(_context.Categories, "CategoryId", "CategoryDescription", product.CategoryIdfk);
            return View(product);
        }

        // GET: Productos/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            // Cargar CategoryDescription en lugar de CategoryId en el dropdown
            ViewData["CategoryIdfk"] = new SelectList(_context.Categories, "CategoryId", "CategoryDescription", product.CategoryIdfk);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Price,CategoryIdfk")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            // Volver a cargar las categorías si hay un error de validación
            ViewData["CategoryIdfk"] = new SelectList(_context.Categories, "CategoryId", "CategoryDescription", product.CategoryIdfk);
            return View(product);
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.CategoryIdfkNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'BDSALESContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
