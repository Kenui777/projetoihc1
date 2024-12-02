using Microsoft.EntityFrameworkCore;
using projetoihc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ApplicationDbContext : DbContext
{
    public DbSet<Clientes> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

    // Construtor que aceita DbContextOptions<ApplicationDbContext> 
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração do relacionamento de 1 para 1 entre Clientes e Endereco
        modelBuilder.Entity<Clientes>()
            .HasOne(c => c.Endereco)
            .WithOne(e => e.Cliente)  // Agora especificando o Cliente como a navegação inversa
            .HasForeignKey<Clientes>(c => c.EnderecoId)
            .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

        // Caso queira garantir que o EnderecoId no Cliente seja único
        modelBuilder.Entity<Clientes>()
            .HasIndex(c => c.EnderecoId)
            .IsUnique(); // Garante que o EnderecoId seja único, ou seja, um endereço está vinculado a um único cliente

        // Configuração para o Endereco, caso precise de mais detalhes sobre o relacionamento
        modelBuilder.Entity<Endereco>()
            .HasOne(e => e.Cliente)
            .WithOne(c => c.Endereco)
            .HasForeignKey<Clientes>(c => c.EnderecoId)
            .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata também no lado do Endereco
    }

    // Se necessário, você pode configurar explicitamente o provedor de banco de dados aqui
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     if (!optionsBuilder.IsConfigured)
    //     {
    //         optionsBuilder.UseSqlServer("Sua string de conexão");
    //     }
    // }
}
