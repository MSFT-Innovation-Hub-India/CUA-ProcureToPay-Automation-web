using System;
using System.Collections.Generic;

namespace ERPWeb.Models;

public partial class PurchaseInvoiceLine
{
    public int PurchaseId { get; set; }

    public int SequenceId { get; set; }

    public string ItemId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Description { get; set; }

    public virtual PurchaseInvoiceHeader Purchase { get; set; } = null!;
}
