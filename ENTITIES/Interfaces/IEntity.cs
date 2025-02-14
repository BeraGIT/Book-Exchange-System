using ENTITIES.Enums;

namespace ENTITIES.Interfaces
{
    public interface IEntity
    {
        string ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }

    }
}
