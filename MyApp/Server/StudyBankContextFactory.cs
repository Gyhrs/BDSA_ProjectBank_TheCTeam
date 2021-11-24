using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MyApp.Infrastructure;

namespace MyApp.Server
{
    public class StudyBankContextFactory : IDesignTimeDbContextFactory<StudyBankContext>
    {
        public StudyBankContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("StudyBank");

            var optionsBuilder = new DbContextOptionsBuilder<StudyBankContext>()
                .UseSqlServer(connectionString);

            return new StudyBankContext(optionsBuilder.Options);
        }

    }
}