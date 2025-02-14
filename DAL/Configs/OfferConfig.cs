using ENTITIES.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configs
{
    public class OfferConfig : BaseConfig<Offer>
    {
        public override void Configure(EntityTypeBuilder<Offer> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Listing)
                   .WithMany(x => x.Offers)
                   .HasForeignKey(x => x.ListingID)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(x => x.OfferedBook)
                   .WithOne(x => x.Offer)
                   .HasForeignKey<Offer>(x => x.OfferedBookID)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(o => o.OStatus)
                  .HasConversion<int>() // Enum'ı int olarak sakla
                  .IsRequired();       // Zorunlu bir alan olarak işaretle


        }
    }
}
