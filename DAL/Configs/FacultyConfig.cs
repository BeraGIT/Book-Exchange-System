using ENTITIES.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configs
{
    public class FacultyConfig : BaseConfig<Faculty>
    {
        public override void Configure(EntityTypeBuilder<Faculty> builder)
        {
            base.Configure(builder);

            builder.HasMany(x => x.UserProfiles)
                   .WithOne(x => x.Faculty)
                   .HasForeignKey(x => x.FacultyID)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Listings)
                    .WithOne(x => x.Faculty)
                    .HasForeignKey(x => x.FacultyID)
                    .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
