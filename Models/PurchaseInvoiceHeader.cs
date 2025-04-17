using System;
using System.Collections.Generic;

namespace ERPWeb.Models;

public partial class PurchaseInvoiceHeader
{
    public int PurchaseId { get; set; }

    public string PurchaseInvoiceNo { get; set; } = null!;

    public string? ContractReference { get; set; }

    public string? SupplierId { get; set; }

    public decimal? TotalInvoiceValue { get; set; }

    public DateOnly? InvoiceDate { get; set; }

    public string? Status { get; set; }

    public string? Remarks { get; set; }

    public virtual ICollection<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; } = new List<PurchaseInvoiceLine>();
}
