using System;
using System.Collections.Generic;
using ERPWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ERPWeb.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContractHeader> ContractHeaders { get; set; }

    public virtual DbSet<ContractLine> ContractLines { get; set; }

    public virtual DbSet<ContractsById> ContractsByIds { get; set; }

    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:az-sqldb-common.database.windows.net,1433;Initial Catalog=cdcsampledb;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.Entity<ContractHeader>(entity =>
        {
            entity.HasKey(e => e.ContractId);

            entity.ToTable("contract_headers");

            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .HasColumnName("ContractID");
            entity.Property(e => e.Currency).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .HasColumnName("SupplierID");
        });

        modelBuilder.Entity<ContractLine>(entity =>
        {
            entity.HasKey(e => new { e.ContractId, e.LineId });

            entity.ToTable("contract_lines");

            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .HasColumnName("ContractID");
            entity.Property(e => e.LineId)
                .HasMaxLength(50)
                .HasColumnName("LineID");
            entity.Property(e => e.ItemDescription).HasMaxLength(50);
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
        });

        modelBuilder.Entity<ContractsById>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("contracts_by_id");

            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .HasColumnName("ContractID");
            entity.Property(e => e.Currency).HasMaxLength(50);
            entity.Property(e => e.ItemDescription).HasMaxLength(50);
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.LineId)
                .HasMaxLength(50)
                .HasColumnName("LineID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .HasColumnName("SupplierID");
        });

        
        modelBuilder.Entity<ContractLine>()
            .HasOne(cl => cl.ContractHeader)
            .WithMany(ch => ch.ContractLines)
            .HasForeignKey(cl => cl.ContractId)
            .HasConstraintName("FK_ContractLine_ContractHeader");

        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
