using Microsoft.EntityFrameworkCore;
using projetoihc.Models;
using System;

public class ApplicationDbContext : DbContext
{
    public DbSet<Clientes> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relacionamento 1:1 entre Cliente e Endereco
        modelBuilder.Entity<Clientes>()
            .HasOne(c => c.Endereco)
            .WithOne(e => e.Cliente)
            .HasForeignKey<Clientes>(c => c.EnderecoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Clientes>()
            .HasIndex(c => c.EnderecoId)
            .IsUnique();

        modelBuilder.Entity<Endereco>()
            .HasOne(e => e.Cliente)
            .WithOne(c => c.Endereco)
            .HasForeignKey<Clientes>(c => c.EnderecoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
