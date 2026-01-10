using System.ComponentModel.DataAnnotations;

namespace JobPortalProject.BL.Attributes
{
    public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime date)
            {
                if (date >= DateTime.Now.AddYears(-16))
                {
                    return new ValidationResult(ErrorMessage ?? "Minimum age is 16.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
