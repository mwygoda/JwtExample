using System.Linq;
using JWTExample.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTExample.DataAccess
{
    public class ExampleContext : IdentityDbContext<User>
    {
        public ExampleContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
                
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<IdentityUserLogin<string>>()
                .HasKey(x => new {x.UserId});

            builder.Entity<IdentityUserRole<string>>()
                .HasKey(x => new {x.UserId});

            builder.Entity<IdentityUserToken<string>>()
                .HasKey(x => new {x.UserId});
        }
    }
}
