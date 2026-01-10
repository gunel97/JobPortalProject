using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Attributes
{
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public GreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // 1. Get the value of the field the user typed (MaxSalary)
            var currentValue = (double?)value;

            // 2. Find the "MinSalary" property on the object
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            // 3. Get the value of "MinSalary"
            var comparisonValue = (double?)property.GetValue(validationContext.ObjectInstance);

            // 4. Perform the comparison
            if (currentValue < comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? $"Value must be greater than {_comparisonProperty}");
            }

            return ValidationResult.Success;
        }
    }
}
