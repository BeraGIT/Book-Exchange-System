using ENTITIES.Enums;
using ENTITIES.Interfaces;

namespace ENTITIES.Models
{
    public abstract class BaseEntity : IEntity
    {
        public BaseEntity()
        {
                ID=Guid.NewGuid().ToString();
                Status = DataStatus.Created;
                CreatedDate = DateTime.UtcNow;
        }
            
       
        public string ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }
    }
}
