using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ERPWeb.Models;

public partial class CdcsampledbContext : DbContext
{
    public CdcsampledbContext()
    {
    }

    public CdcsampledbContext(DbContextOptions<CdcsampledbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PurchaseInvoiceHeader> PurchaseInvoiceHeaders { get; set; }

    public virtual DbSet<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:az-sqldb-common.database.windows.net,1433;Initial Catalog=cdcsampledb;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseInvoiceHeader>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__purchase__6B0A6BDEBC47D34D");

            entity.ToTable("purchase_invoice_header");

            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.ContractReference)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PurchaseInvoiceNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.TotalInvoiceValue).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<PurchaseInvoiceLine>(entity =>
        {
            entity.HasKey(e => new { e.PurchaseId, e.SequenceId }).HasName("PK_PurchaseInvoiceLines");

            entity.ToTable("purchase_invoice_lines");

            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.SequenceId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SequenceID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ItemID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseInvoiceLines)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
