using System.Reflection.Emit;
using DAL.Configs;
using ENTITIES.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class MyContext : IdentityDbContext<User, IdentityRole, string>
    {
        public MyContext(DbContextOptions<MyContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new UserProfileConfig());
            builder.ApplyConfiguration(new BookConfig());
            builder.ApplyConfiguration(new FacultyConfig());
            builder.ApplyConfiguration(new ListingConfig());
            builder.ApplyConfiguration(new OfferConfig());

          



            //todo: kaldırılacak
            builder.Entity<Faculty>().HasData(
                new Faculty { ID = "1", FacultyName = "Tıp Fakültesi", FacultyAddress = "Cumhuriyet Üniversitesi Kampüsü" },
                new Faculty { ID = "2", FacultyName = "Mühendislik Fakültesi", FacultyAddress = "Cumhuriyet Üniversitesi Kampüsü" },
                new Faculty { ID = "3", FacultyName = "Edebiyat Fakültesi", FacultyAddress = "Cumhuriyet Üniversitesi Kampüsü" },
                new Faculty { ID = "4", FacultyName = "İktisadi ve İdari Bilimler Fakültesi", FacultyAddress = "Cumhuriyet Üniversitesi Kampüsü" },
                new Faculty { ID = "5", FacultyName = "Fen Fakültesi", FacultyAddress = "Cumhuriyet Üniversitesi Kampüsü" });

        }

      



public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Offer> Offers { get; set; }
    }
}

