using ENTITIES.Enums;

namespace ENTITIES.Models
{
    public class Offer : BaseEntity
    {
        public Offer()
        {
            OStatus = OfferStatus.Pending;
        }
        public string ListingID { get; set; }
        public string UserID { get; set; }
        public string OfferedBookID { get; set; }
        public OfferStatus OStatus { get; set; }

        //Relations
        public virtual User User { get; set; }  //usercfg
        public virtual Listing Listing { get; set; } //offercfg
        public virtual Book OfferedBook { get; set; } //offercfg


    }
}
