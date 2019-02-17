using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceBook.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateAfterAttribute : ValidationAttribute
    {
        private readonly string comparisonMemberName;

        public DateAfterAttribute(string comparisonMemberName)
        {
            this.comparisonMemberName = comparisonMemberName;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var corePropertyValue = (DateTime)value;

            var comparedProperty = validationContext
                .ObjectType
                .GetProperty(this.comparisonMemberName);

            if (comparedProperty == null)
            {
                return new ValidationResult(
                    $"Unable to find a property with name: {this.comparisonMemberName}!");
            }

            var comparisonPropertyValue = comparedProperty
                .GetValue(validationContext.ObjectInstance) as DateTime?;
            
            if (corePropertyValue <= comparisonPropertyValue.Value)
            {
                var corePropertyDisplayName = validationContext.DisplayName;
                var comparisonPropertyDisplayAttribute = validationContext
                    .ObjectType
                    .GetProperty(this.comparisonMemberName)
                    .GetCustomAttributes(typeof(DisplayAttribute), true)
                    .FirstOrDefault() as DisplayAttribute;
                var comparisonPropertyDisplayName = comparisonPropertyDisplayAttribute?.Name 
                    ?? this.comparisonMemberName;

                return new ValidationResult(
                    $"{comparisonPropertyDisplayName} should be after {corePropertyDisplayName}!");
            }

            return ValidationResult.Success;
        }
    }
}
