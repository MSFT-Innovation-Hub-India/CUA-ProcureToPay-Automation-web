using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERPWeb.Models;

namespace ERPWeb.Views_PurchaseInvoiceHeaders
{
    public class PurchaseInvoiceHeadersView : Controller
    {
        private readonly CdcsampledbContext _context;

        public PurchaseInvoiceHeadersView(CdcsampledbContext context)
        {
            _context = context;
        }

        // GET: PurchaseInvoiceHeaders
        public async Task<IActionResult> Index()
        {
            return View(await _context.PurchaseInvoiceHeaders.ToListAsync());
        }

        // GET: PurchaseInvoiceHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseInvoiceHeader = await _context.PurchaseInvoiceHeaders
                .FirstOrDefaultAsync(m => m.PurchaseId == id);
            if (purchaseInvoiceHeader == null)
            {
                return NotFound();
            }

            return View(purchaseInvoiceHeader);
        }

        // GET: PurchaseInvoiceHeaders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PurchaseInvoiceHeaders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseId,PurchaseInvoiceNo,ContractReference,SupplierId,TotalInvoiceValue,InvoiceDate,Status,Remarks")] PurchaseInvoiceHeader purchaseInvoiceHeader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseInvoiceHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseInvoiceHeader);
        }

        // GET: PurchaseInvoiceHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseInvoiceHeader = await _context.PurchaseInvoiceHeaders.FindAsync(id);
            if (purchaseInvoiceHeader == null)
            {
                return NotFound();
            }
            return View(purchaseInvoiceHeader);
        }

        // POST: PurchaseInvoiceHeaders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseId,PurchaseInvoiceNo,ContractReference,SupplierId,TotalInvoiceValue,InvoiceDate,Status,Remarks")] PurchaseInvoiceHeader purchaseInvoiceHeader)
        {
            if (id != purchaseInvoiceHeader.PurchaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseInvoiceHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseInvoiceHeaderExists(purchaseInvoiceHeader.PurchaseId))
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
            return View(purchaseInvoiceHeader);
        }

        // GET: PurchaseInvoiceHeaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseInvoiceHeader = await _context.PurchaseInvoiceHeaders
                .FirstOrDefaultAsync(m => m.PurchaseId == id);
            if (purchaseInvoiceHeader == null)
            {
                return NotFound();
            }

            return View(purchaseInvoiceHeader);
        }

        // POST: PurchaseInvoiceHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseInvoiceHeader = await _context.PurchaseInvoiceHeaders.FindAsync(id);
            if (purchaseInvoiceHeader != null)
            {
                _context.PurchaseInvoiceHeaders.Remove(purchaseInvoiceHeader);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseInvoiceHeaderExists(int id)
        {
            return _context.PurchaseInvoiceHeaders.Any(e => e.PurchaseId == id);
        }
    }
}
