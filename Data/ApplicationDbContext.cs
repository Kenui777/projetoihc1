using Microsoft.EntityFrameworkCore;
using projetoihc.Models;

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
            .WithOne()
            .HasForeignKey<Clientes>(c => c.EnderecoId)
            .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata
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
