using Application.Validation.Orders;
using Domain.Entities.CustomerAgg;
using FluentValidation;

namespace Application.Validation.Customers;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Surname).NotNull();
        RuleFor(customer => customer.Surname).NotNull()
            .WithMessage("Please ensure you have entered your {PropertyName}");
        //Using constant in a custom message:
        RuleFor(customer => customer.Surname)
          .NotNull()
          .WithMessage(customer => string.Format("This message references some constant values: {0} {1}", "hello", 5));
            //Result would be "This message references some constant values: hello 5"

        //Referencing other property values:
        RuleFor(customer => customer.Surname).NotNull()
            .WithMessage(customer => $"This message references some other properties: Forename: {customer.Forename} Discount: {customer.Discount}");
                //Result would be: "This message references some other properties: Forename: Jeremy Discount: 100"
        

        RuleFor(customer => customer.Address).SetValidator(new AddressValidator());
        RuleFor(x => x.Age).InclusiveBetween(18, 60);
        //RuleFor(customer => customer.Address.Postcode).NotNull().When(customer => customer.Address != null)

        RuleForEach(x => x.Orders).SetValidator(new OrderValidator());
        RuleForEach(x => x.Orders).ChildRules(order =>
        {
            order.RuleFor(x => x.Total).GreaterThan(0);
        });
        RuleForEach(x => x.Orders).Where(x => x.Cost != 0)
            .SetValidator(new OrderValidator());

        // This rule acts on the whole collection (using RuleFor)
        RuleFor(x => x.Orders)
          .Must(x => x.Count <= 10).WithMessage("No more than 10 orders are allowed");

        // This rule acts on each individual element (using RuleForEach)
        RuleForEach(x => x.Orders)
          .Must(order => order.Total > 0).WithMessage("Orders must have a total of more than 0");

        RuleFor(x => x.Orders)
          .Must(x => x.Count <= 10).WithMessage("No more than 10 orders are allowed")
          .ForEach(orderRule =>
          {
              orderRule.Must(order => order.Total > 0).WithMessage("Orders must have a total of more than 0");
          });
    }
}
