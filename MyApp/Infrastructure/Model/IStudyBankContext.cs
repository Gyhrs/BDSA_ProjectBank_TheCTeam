using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure
{
    public interface IStudyBankContext : IDisposable
    {
        DbSet<StudyBankUser> Users { get; }
        DbSet<Project> Projects { get; }
        DbSet<Tag> Tags { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
