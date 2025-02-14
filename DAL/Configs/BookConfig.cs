using ENTITIES.Enums;
using ENTITIES.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configs
{
    internal class BookConfig : BaseConfig<Book>
    {
        public override void Configure(EntityTypeBuilder<Book> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Books)
                   .HasForeignKey(x => x.UserID)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Listing)
                   .WithOne(x => x.Book)
                   .HasForeignKey<Listing>(x => x.BookID)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(x => x.Offer)
                   .WithOne(x => x.OfferedBook)
                   .HasForeignKey<Offer>(x => x.OfferedBookID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(b => b.BStatus)
                  .HasConversion<int>() // Enum'ı int olarak sakla
                  .IsRequired()      // Zorunlu bir alan olarak işaretle
                  .HasDefaultValue(BookStatus.Usable);
        }

    }
}
