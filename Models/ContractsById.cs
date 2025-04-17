using System;
using System.Collections.Generic;

namespace ERPWeb.Models;

public partial class ContractsById
{
    public string ContractId { get; set; } = null!;

    public string LineId { get; set; } = null!;

    public string SupplierId { get; set; } = null!;

    public DateOnly ContractDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public double TotalAmount { get; set; }

    public string Currency { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string ItemId { get; set; } = null!;

    public byte Quantity { get; set; }

    public double UnitPrice { get; set; }

    public double TotalPrice { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public string ItemDescription { get; set; } = null!;
}
