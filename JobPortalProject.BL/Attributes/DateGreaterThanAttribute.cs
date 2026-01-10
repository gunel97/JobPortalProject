using System.ComponentModel.DataAnnotations;

namespace JobPortalProject.BL.Attributes
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Get the value of the property we are validating (EndDate)
            var currentValue = (DateTime?)value;

            // Find the property info of the other date (StartDate)
            var propertyInfo = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (propertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonProperty}");
            }

            // Get the value of the other date
            var comparisonValue = (DateTime?)propertyInfo.GetValue(validationContext.ObjectInstance);

            // Compare: If EndDate is earlier than StartDate, return error
            if (currentValue < comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? "End date must be later than start date.");
            }

            return ValidationResult.Success;
        }
    }


}
