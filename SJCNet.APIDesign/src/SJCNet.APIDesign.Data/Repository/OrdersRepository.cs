using SJCNet.APIDesign.Model;
using Microsoft.EntityFrameworkCore;

namespace SJCNet.APIDesign.Data.Repository
{
    public class OrdersRepository : EFRepository<Order>
    {
        public OrdersRepository(DataContext context) : base(context)
        {}

        protected override DbSet<Order> DbSet
        {
            get
            {
                return base.Context.Orders;
            }
        }
    }
}
