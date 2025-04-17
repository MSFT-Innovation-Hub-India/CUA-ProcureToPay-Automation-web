using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERPWeb.Data;
using ERPWeb.Models;

namespace ERPWeb.Controllers
{
    public class ContractHeadersController : Controller
    {
        private readonly AppDbContext _context;

        public ContractHeadersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ContractHeaders
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContractHeaders.ToListAsync());
        }

        // GET: ContractHeaders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractHeader = await _context.ContractHeaders
                .Include(ch => ch.ContractLines) // Include related ContractLine entries
                .FirstOrDefaultAsync(m => m.ContractId == id);

            if (contractHeader == null)
            {
                return NotFound();
            }

            return View(contractHeader);
        }

        // GET: ContractHeaders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContractHeaders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,SupplierId,ContractDate,ExpirationDate,TotalAmount,Currency,Status")] ContractHeader contractHeader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contractHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contractHeader);
        }

        // GET: ContractHeaders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractHeader = await _context.ContractHeaders.FindAsync(id);
            if (contractHeader == null)
            {
                return NotFound();
            }
            return View(contractHeader);
        }

        // POST: ContractHeaders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ContractId,SupplierId,ContractDate,ExpirationDate,TotalAmount,Currency,Status")] ContractHeader contractHeader)
        {
            if (id != contractHeader.ContractId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contractHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractHeaderExists(contractHeader.ContractId))
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
            return View(contractHeader);
        }

        // GET: ContractHeaders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractHeader = await _context.ContractHeaders
                .FirstOrDefaultAsync(m => m.ContractId == id);
            if (contractHeader == null)
            {
                return NotFound();
            }

            return View(contractHeader);
        }

        // POST: ContractHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var contractHeader = await _context.ContractHeaders.FindAsync(id);
            if (contractHeader != null)
            {
                _context.ContractHeaders.Remove(contractHeader);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractHeaderExists(string id)
        {
            return _context.ContractHeaders.Any(e => e.ContractId == id);
        }        // GET: ContractHeaders/ContractLines/{id}
        public async Task<IActionResult> ContractLines(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get the contract header first
            var contractHeader = await _context.ContractHeaders
                .Include(ch => ch.ContractLines) // Include all related lines
                .FirstOrDefaultAsync(ch => ch.ContractId == id);

            if (contractHeader == null)
            {
                return NotFound();
            }

            // Now we're passing the contract header which includes its lines
            return View(contractHeader);
        }
    }
}
