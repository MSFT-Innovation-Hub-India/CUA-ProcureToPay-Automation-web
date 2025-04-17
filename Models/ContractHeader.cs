using System;
using System.Collections.Generic;

namespace ERPWeb.Models;

public partial class ContractHeader
{
    public string ContractId { get; set; } = null!;

    public string SupplierId { get; set; } = null!;

    public DateOnly ContractDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public double TotalAmount { get; set; }

    public string Currency { get; set; } = null!;

    public string Status { get; set; } = null!;
    public virtual ICollection<ContractLine> ContractLines { get; set; } = new List<ContractLine>();
}
