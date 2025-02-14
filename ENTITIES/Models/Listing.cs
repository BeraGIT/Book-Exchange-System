using ENTITIES.Enums;

namespace ENTITIES.Models
{
    public class Listing : BaseEntity
    {
        public Listing()
        {
                LStatus = ListingStatus.Open;
        }
        public string BookID { get; set; }
        public string UserID { get; set; }
        public string FacultyID { get; set; }
        public string? ListingDescription { get; set; }
        public ListingStatus LStatus { get; set; }

        //Relations 
        public virtual User User { get; set; }  //usercfg
        public virtual Book Book { get; set; }   //lstcfg
        public virtual Faculty Faculty { get; set; } //lstcfg
        public virtual List<Offer>? Offers { get; set; }  //offercfg
          
            
            
    }
}
