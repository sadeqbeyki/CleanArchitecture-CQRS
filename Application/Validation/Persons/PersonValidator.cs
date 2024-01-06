using Domain.Entities.PersonAgg;
using FluentValidation;

namespace Application.Validation.Persons
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            //RuleForEach(x => x.AddressLines).NotNull();
            RuleForEach(x => x.AddressLines).NotNull()
                .WithMessage("Address {CollectionIndex} is required.");

        }
    }
}
