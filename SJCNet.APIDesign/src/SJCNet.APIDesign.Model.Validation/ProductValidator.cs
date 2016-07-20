using FluentValidation;

namespace SJCNet.APIDesign.Model.Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Id).NotEqual(0);
            RuleFor(product => product.Name).NotEmpty();
            RuleFor(product => product.Price).NotEqual(0);
        }
    }
}
