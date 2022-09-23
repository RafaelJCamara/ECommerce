﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(order => order.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull().WithMessage("{UserName} is required.")
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");
            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required.");
            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
        }
    }
}
