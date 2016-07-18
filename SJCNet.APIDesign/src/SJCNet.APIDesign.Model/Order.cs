using System;
using System.Collections.Generic;
using System.Linq;

namespace SJCNet.APIDesign.Model
{
    public class Order : IEntity
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public List<Product> Products { get; set; }

        public decimal Value
        {
            get
            {
                return this.Products.Sum(product => product.Price);
            }
        }
    }
}