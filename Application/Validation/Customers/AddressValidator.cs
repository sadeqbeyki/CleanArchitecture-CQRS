using Domain.Entities.CustomerAgg;
using FluentValidation;

namespace Application.Validation.Customers
{
    public class AddressValidator:AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(address => address.Postcode).NotNull();

        }
    }
}
