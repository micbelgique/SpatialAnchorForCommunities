using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Community> Communities { get; set; }
        public DbSet<Anchor> Anchors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Interaction> Interactions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
