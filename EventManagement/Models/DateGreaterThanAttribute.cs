
using System;
using System.ComponentModel.DataAnnotations;
namespace EventManagement.Models
{
  

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var comparisonValue = validationContext.ObjectType.GetProperty(_comparisonProperty)
                .GetValue(validationContext.ObjectInstance);

            if (value is DateTime dateTimeValue && comparisonValue is DateTime comparisonDateTime)
            {
                if (dateTimeValue <= comparisonDateTime)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
