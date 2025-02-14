using ENTITIES.Enums;

namespace ENTITIES.Models
{
    public class Book : BaseEntity
    {

        public Book() 
        {
            if (BStatus == null) 
            {
                BStatus=BookStatus.Usable;
            }
        }
        
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Author { get; set; }
        public string UserID { get; set; }
        public BookStatus BStatus { get; set; }

        //Relations
        public virtual User User { get; set; } //UserConfg
        public virtual Listing? Listing { get; set; } //LstConfg
        public virtual Offer? Offer { get; set; } //OfferCfg   

    }
}
