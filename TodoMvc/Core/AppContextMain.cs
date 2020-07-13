using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TodoMvc.Models;

namespace TodoMvc.Core
{
    public class AppContextMain : DbContext
    {
        public AppContextMain() : base("DefaultConnection")
        {

        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}