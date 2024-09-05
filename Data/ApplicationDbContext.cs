using Microsoft.EntityFrameworkCore;
using CadastroFornecedor.Models;

namespace CadastroFornecedor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Fornecedor> Fornecedores { get; set; }
    }
}
