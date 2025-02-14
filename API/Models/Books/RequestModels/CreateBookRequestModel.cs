using ENTITIES.Enums;

namespace API.Models.Books.RequestModels
{
    public class CreateBookRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
     
        
    }
}
