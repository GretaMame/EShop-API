﻿using eshopAPI.Requests.Categories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshopAPI.Validators.Request
{
    public class CategoryUpdateValidator : AbstractValidator<SubcategoryCreateRequest>
    {
        public CategoryUpdateValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(50);

            RuleFor(r => r.ParentID)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
