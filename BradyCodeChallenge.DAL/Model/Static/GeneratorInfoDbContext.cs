// Importing necessary namespaces
using Microsoft.EntityFrameworkCore;


namespace BradyCodeChallenge.DAL.Model.Static
{
    // Definition of the GeneratorInfoDbContext class, inheriting from DbContext
    public class GeneratorInfoDbContext : DbContext
    {
        // DbSet property representing a collection of GeneratorInfo entities in the database
        public DbSet<GeneratorInfo> GeneratorInfos { get; set; }

        // Add this constructor to accept DbContextOptions
        public GeneratorInfoDbContext(DbContextOptions<GeneratorInfoDbContext> options)
            : base(options) 
        {
        }
    }
}
