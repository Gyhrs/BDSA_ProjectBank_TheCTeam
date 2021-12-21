namespace MyApp.Infrastructure.Model;
public interface IStudyBankContext : IDisposable
{
    DbSet<StudyBankUser> Users { get; }
    DbSet<Project> Projects { get; }
    DbSet<Tag> Tags { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

