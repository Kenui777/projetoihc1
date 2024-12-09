using Microsoft.EntityFrameworkCore;
using projetoihc.Models;


public class ApplicationDbContext : DbContext
{
    public DbSet<Clientes> Clientes { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Agora que o Endereco é parte de Clientes, não há mais necessidade de um relacionamento separado.
        // Não é necessário mais o mapeamento de relacionamento 1:1 entre Clientes e Endereco.
    }
}
