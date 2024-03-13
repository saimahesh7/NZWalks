using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerId = "f12a95c2-4856-4577-a51c-24b0a3fc3801";
            var writerId = "cc73bd89-dbc0-4f7d-87e1-f519fc58584c";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id=readerId,
                    ConcurrencyStamp=readerId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper()
                },
                new IdentityRole()
                {
                    Id=writerId,
                    ConcurrencyStamp=writerId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
