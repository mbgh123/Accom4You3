using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using A4U3.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace A4U3.Domain.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Audit> Audit { get; set; }
        DbSet<Feature> Features { get; set; }
        DbSet<Picture> Pictures { get; set; }
        DbSet<Property> Properties { get; set; }
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<ApplicationUser> Users { get; set; }
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);

        EntityEntry Entry( object entity);
        void Dispose();
    }
}