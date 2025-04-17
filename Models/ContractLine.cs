using System;
using System.Collections.Generic;

namespace ERPWeb.Models;

public partial class ContractLine
{
    public string ContractId { get; set; } = null!;

    public string LineId { get; set; } = null!;

    public string ItemId { get; set; } = null!;

    public byte Quantity { get; set; }

    public double UnitPrice { get; set; }

    public double TotalPrice { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public string ItemDescription { get; set; } = null!;

    public virtual ContractHeader ContractHeader { get; set; } = null!;
}
