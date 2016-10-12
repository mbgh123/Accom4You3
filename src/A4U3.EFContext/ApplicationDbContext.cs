using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using A4U3.Domain.Models;
using A4U3.Domain.Interfaces;
using Microsoft.Extensions.Options;
//using Microsoft.Data.Entity.Infrastructure;

namespace A4U3.EFContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private IOptions<ConfigOptions> _options;

        public ApplicationDbContext(IOptions<ConfigOptions> options)
        {
            _options = options;
        }

        public DbSet<Audit> Audit { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        /// <summary>
        /// The Sql conection string is actually being set in startup. But its not working.
        /// So lets do it here.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_options.Value.UseInMemoryStore)
            {
                optionsBuilder.UseInMemoryDatabase();
            }
            else
            {
                optionsBuilder.UseSqlServer(_options.Value.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // entity pluralizing
            // https://docs.efproject.net/en/latest/miscellaneous/rc1-rc2-upgrade.html

            //foreach (var entity in builder.Model.GetEntityTypes())
            //{
            //    entity.Relational().TableName = entity.ClrType.Name;
            //}
        }
    }
}
