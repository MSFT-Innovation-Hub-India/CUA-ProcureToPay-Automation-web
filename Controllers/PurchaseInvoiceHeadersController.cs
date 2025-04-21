using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERPWeb.Models;

namespace ERPWeb.Controllers
{
    public class PurchaseInvoiceHeadersController : Controller
    {
        private readonly CdcsampledbContext _context;

        public PurchaseInvoiceHeadersController(CdcsampledbContext context)
        {
            _context = context;
        }

        // GET: PurchaseInvoiceHeaders
        [Route("PurchaseInvoiceHeaders")] // Explicitly specify the route
        public async Task<IActionResult> Index()
        {
            return View("PurchaseInvoiceIndex", await _context.PurchaseInvoiceHeaders.ToListAsync());
        }

        // GET: PurchaseInvoiceHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PurchaseInvoiceHeaders == null)
            {
                return NotFound();
            }

            var purchaseInvoiceHeader = await _context.PurchaseInvoiceHeaders
                .Include(p => p.PurchaseInvoiceLines)
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
        }        // POST: PurchaseInvoiceHeaders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseId,ContractReference,PurchaseInvoiceNo,Status,SupplierId,TotalInvoiceValue,Remarks,InvoiceDate")] PurchaseInvoiceHeader purchaseInvoiceHeader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseInvoiceHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseInvoiceHeader);
        }// GET: PurchaseInvoiceHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PurchaseInvoiceHeaders == null)
            {
                return NotFound();
            }

            var purchaseInvoiceHeader = await _context.PurchaseInvoiceHeaders
                .Include(p => p.PurchaseInvoiceLines)  // Include invoice lines
                .FirstOrDefaultAsync(p => p.PurchaseId == id);
                
            if (purchaseInvoiceHeader == null)
            {
                return NotFound();
            }
            return View(purchaseInvoiceHeader);
        }        // POST: PurchaseInvoiceHeaders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseId,ContractReference,PurchaseInvoiceNo,Status,SupplierId,TotalInvoiceValue,Remarks,InvoiceDate")] PurchaseInvoiceHeader purchaseInvoiceHeader)
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
            if (id == null || _context.PurchaseInvoiceHeaders == null)
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
            if (_context.PurchaseInvoiceHeaders == null)
            {
                return Problem("Entity set 'CdcsampledbContext.PurchaseInvoiceHeaders'  is null.");
            }
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

        // GET: PurchaseInvoiceHeaders/InvoiceLines/5
        public async Task<IActionResult> InvoiceLines(int? id)
        {
            if (id == null || _context.PurchaseInvoiceLines == null)
            {
                return NotFound();
            }

            var purchaseInvoiceLines = await _context.PurchaseInvoiceLines
                .Where(line => line.PurchaseId == id)
                .ToListAsync();

            if (purchaseInvoiceLines == null || !purchaseInvoiceLines.Any())
            {
                return NotFound();
            }

            ViewBag.PurchaseId = id;
            return View(purchaseInvoiceLines);
        }        [HttpPost]
        public async Task<IActionResult> SaveInvoiceLine([FromBody] PurchaseInvoiceLine line)
        {
            if (line == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                // Check if the record already exists (sequenceId > 0)
                if (line.SequenceId > 0)
                {
                    // Find the existing line
                    var existingLine = await _context.PurchaseInvoiceLines
                        .FirstOrDefaultAsync(l => l.PurchaseId == line.PurchaseId && l.SequenceId == line.SequenceId);
                      if (existingLine != null)
                    {
                        // Update existing record
                        existingLine.Description = line.Description;
                        existingLine.ItemId = line.ItemId;
                        existingLine.Quantity = line.Quantity;
                        existingLine.UnitPrice = line.UnitPrice;
                        existingLine.TotalPrice = line.TotalPrice;
                        
                        _context.Update(existingLine);
                        await _context.SaveChangesAsync();
                        return Ok(new { success = true, message = "Line updated successfully." });
                    }
                }
                
                // Only reach this point if we're adding a new line (sequenceId = 0)
                // or if we couldn't find the record to update
                
                // Validate required fields for new records
                if (string.IsNullOrEmpty(line.ItemId))
                {
                    return BadRequest(new { success = false, message = "ItemId is required for new records." });
                }
                
                // For new lines, ensure SequenceId is 0 to let the database assign it
                line.SequenceId = 0;
                
                // Add the new record
                _context.PurchaseInvoiceLines.Add(line);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Line saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteInvoiceLine([FromBody] DeleteInvoiceLineModel model)
        {
            if (model == null || model.PurchaseId <= 0 || model.SequenceId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }

            try
            {
                var line = await _context.PurchaseInvoiceLines
                    .FirstOrDefaultAsync(l => l.PurchaseId == model.PurchaseId && l.SequenceId == model.SequenceId);

                if (line == null)
                {
                    return NotFound(new { success = false, message = "Invoice line not found." });
                }

                _context.PurchaseInvoiceLines.Remove(line);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Invoice line deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Simple model for delete operation
        public class DeleteInvoiceLineModel
        {
            public int PurchaseId { get; set; }
            public int SequenceId { get; set; }
        }
    }
}
