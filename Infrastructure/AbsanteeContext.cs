using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AbsanteeContext : DbContext
    {
        public virtual DbSet<CollaboratorDataModel> Collaborators { get; set; }
        public virtual DbSet<UserDataModel> Users { get; set; }

        public AbsanteeContext(DbContextOptions<AbsanteeContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CollaboratorDataModel>().OwnsOne(a => a.PeriodDateTime);

            modelBuilder.Entity<UserDataModel>().OwnsOne(a => a.PeriodDateTime);

            base.OnModelCreating(modelBuilder);
        }
    }
}