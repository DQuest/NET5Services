using Repository;

namespace Homework2.ProductService.Repository
{
    public class ProductDbModel : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}