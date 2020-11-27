using BaseRepository;

namespace ProductService.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}