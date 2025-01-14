using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace sustAInableEducation_backend.Models.Validation
{
    public class ValidEnumAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success!;
            }

            var type = value.GetType();
            
            if(type.GetInterface(nameof(ICollection<Object>)) != null)
            {
                var collection = (System.Collections.ICollection)value;
                var genericType = type.GetGenericArguments()[0];
                foreach (var item in collection)
                {
                    if (!(genericType.IsEnum && Enum.IsDefined(genericType, item)))
                    {
                        return new ValidationResult(ErrorMessage ?? $"{item} is not a valid value for type {genericType.Name}");
                    }
                }
                return ValidationResult.Success!;
            }

            if (!(type.IsEnum && Enum.IsDefined(type, value)))
            {
                return new ValidationResult(ErrorMessage ?? $"{value} is not a valid value for type {type.Name}");
            }

            return ValidationResult.Success!;
        }
    }
}
