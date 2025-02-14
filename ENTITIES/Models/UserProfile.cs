namespace ENTITIES.Models
{
    public class UserProfile : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FacultyID { get; set; }

        //Relations
        public virtual string UserId { get; set; }

        //Nvigation 
        public virtual User User { get; set; } //usercfg
        public virtual Faculty Faculty { get; set; } //userprfcfg
    }
}
