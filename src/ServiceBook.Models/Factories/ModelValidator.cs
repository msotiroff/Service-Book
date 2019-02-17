using ServiceBook.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceBook.Models.Factories
{
    public class ModelValidator : IModelValidator
    {
        public void Validate(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator
                .TryValidateObject(model, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new InvalidOperationException(
                    string.Join(" ", validationResults.Select(vr => vr.ErrorMessage)));
            }
        }
    }
}
