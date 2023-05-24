using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Mag.DAL
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext СоздатьDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(serverVersion: new MySqlServerVersion("8.0.32"),
                             connectionString: "Server=localhost; Port=3306; Database=mag; Uid=root; Pwd=root;");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
