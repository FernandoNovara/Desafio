using Microsoft.EntityFrameworkCore;

namespace Desafio{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Business> business { get; set; }
        public DbSet<Employee> employee { get; set; }
        public DbSet<Register> register { get; set; }
    }
}