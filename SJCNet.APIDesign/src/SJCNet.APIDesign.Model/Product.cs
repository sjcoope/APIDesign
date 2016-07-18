using System.Security.Cryptography.X509Certificates;

namespace SJCNet.APIDesign.Model
{
    public class Product : IEntity
    {
        public Product()
        {}

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}