using ENTITIES.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configs
{
    public class UserConfig : BaseConfig<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Ignore(x => x.ID);

            builder.HasOne(x => x.Profile)
                   .WithOne(x => x.User)
                   .HasForeignKey<UserProfile>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Books)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserID)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Listings)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Offers)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserID)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
