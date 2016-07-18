using SJCNet.APIDesign.Model;
using Microsoft.EntityFrameworkCore;

namespace SJCNet.APIDesign.Data.Repository
{
    public class ProductsRepository : EFRepository<Product>
    {
        public ProductsRepository(DataContext context) : base(context)
        {}

        protected override DbSet<Product> DbSet
        {
            get
            {
                return base.Context.Products;
            }
        }
    }
}
