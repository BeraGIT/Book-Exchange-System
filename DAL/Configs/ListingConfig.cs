using ENTITIES.Enums;
using ENTITIES.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configs
{
    internal class ListingConfig : BaseConfig<Listing>
    {
        public override void Configure(EntityTypeBuilder<Listing> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Listings)
                   .HasForeignKey(x => x.UserID)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(x => x.Faculty)
                   .WithMany(x => x.Listings)
                   .HasForeignKey(x => x.FacultyID)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(x => x.Book)
                   .WithOne(x => x.Listing)
                   .HasForeignKey<Listing>(x => x.BookID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(l => l.LStatus)
                  .HasConversion<int>() // Enum'ı int olarak sakla
                  .IsRequired()
                  .HasDefaultValue(ListingStatus.Open);
        }

    }
}

    
