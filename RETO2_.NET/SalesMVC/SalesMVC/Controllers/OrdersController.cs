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
    public class OrdersController : Controller
    {
        private readonly BDSALESContext _context;

        public OrdersController(BDSALESContext context)
        {
            _context = context;
        }

        // GET: Ordenes
        public async Task<IActionResult> Index()
        {
            var bDSALESContext = _context.Orders.Include(o => o.CustomerFkNavigation);
            return View(await bDSALESContext.ToListAsync());
        }

        // GET: Ordenes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.CustomerFkNavigation)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            //se va realizar una anidacion atravez de una cadena
            var customers = _context.Customers
                                    .Select(c => new
                                    {
                                        CustomerId = c.CustomerId,
                                        FullName = c.FirstName + " " + c.LastName + " (" + c.Email + ")"
                                    })
                                    .ToList();
            ViewData["CustomerFk"] = new SelectList(customers, "CustomerId", "FullName");
            return View();
        }


        // POST: Ordenes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerFk,OrderD")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var customers = _context.Customers
                                    .Select(c => new
                                    {
                                        CustomerId = c.CustomerId,
                                        FullName = c.FirstName + " " + c.LastName + " (" + c.Email + ")"
                                    })
                                    .ToList();
            ViewData["CustomerFk"] = new SelectList(customers, "CustomerId", "FullName", order.CustomerFk);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerFk"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerFk);
            return View(order);
        }

        // POST: Orden/Edit
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerFk,OrderD")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerFk"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerFk);
            return View(order);
        }

        // GET: Orden/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.CustomerFkNavigation)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'BDSALESContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //establece el orden
        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
