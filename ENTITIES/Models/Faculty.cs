namespace ENTITIES.Models
{
    public class Faculty : BaseEntity
    {
        public string FacultyName { get; set; }
        public string FacultyAddress { get; set; }

        //Relations
        public virtual List<UserProfile>? UserProfiles { get; set; } //UserPcfg
        public virtual List<Listing>? Listings { get; set; } //Listcfg

    }
}
