using ENTITIES.Enums;
using ENTITIES.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configs
{
    public abstract class BaseConfig<T> : IEntityTypeConfiguration<T> where T : class, IEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey("ID");
            builder.Property("ID").IsRequired();

            // Enum mapping
            builder.Property("Status")
                   .HasConversion<int>() // Enum'ı int olarak sakla
                   .IsRequired()      // Enum alanını zorunlu yap
                   .HasDefaultValue(DataStatus.Created);
        }
    }
}
