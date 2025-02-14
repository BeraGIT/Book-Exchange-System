using System.ComponentModel.DataAnnotations.Schema;
using ENTITIES.Enums;
using ENTITIES.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ENTITIES.Models
{
    public class User : IdentityUser, IEntity
    {
        public User()
        {
            if (CreatedDate == null)
            {
                CreatedDate = DateTime.Now;
                Status = DataStatus.Created;
            }
        }

      
        
        public string ID { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }

        //Relations
        public virtual UserProfile Profile { get; set; } //usercfg
        public virtual List<Book>? Books { get; set; } //usercg
        public virtual List<Listing>? Listings { get; set; } //usercfg
        public virtual List<Offer>? Offers { get; set; }  //offercfg

    }
}
