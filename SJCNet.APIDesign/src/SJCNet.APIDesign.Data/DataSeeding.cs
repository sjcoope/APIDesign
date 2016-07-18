using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SJCNet.APIDesign.Data
{
    public class DataSeeder
    {
        public bool SeedData(DataContext dataContext)
        {
            dataContext.Products.Add(new Model.Product { Id = 1, Name = "Cricket Bat", Price = 22.99m });
            dataContext.Products.Add(new Model.Product { Id = 2, Name = "Suncream (not needed in UK)", Price = 9.99m });
            dataContext.Products.Add(new Model.Product { Id = 3, Name = "Cap", Price = 3.99m });
            dataContext.Products.Add(new Model.Product { Id = 4, Name = "Ball", Price = 20.00m });
            dataContext.Products.Add(new Model.Product { Id = 5, Name = "Gloves", Price = 8.76m });
            return dataContext.SaveChanges() != 0;
        }
    }
}