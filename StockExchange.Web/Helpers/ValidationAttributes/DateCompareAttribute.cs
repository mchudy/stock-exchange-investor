using System;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Helpers
{
    public class DateCompareAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly DateTimeComparer _comparerType;

        public DateCompareAttribute(string propertyName, DateTimeComparer comparerType)
        {
            _propertyName = propertyName;
            _comparerType = comparerType;
        }

        protected override ValidationResult IsValid(object firstValue, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(_propertyName);
            if (propertyInfo == null)
                return new ValidationResult($"Property {_propertyName} does not exist.");

            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

            switch (_comparerType)
            {
                case DateTimeComparer.IsEqualTo:
                    if ((DateTime)propertyValue == (DateTime)firstValue)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DateTimeComparer.IsGreaterThan:
                    if ((DateTime)propertyValue > (DateTime)firstValue)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DateTimeComparer.IsGreaterThanOrEqualTo:
                    if ((DateTime)propertyValue >= (DateTime)firstValue)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DateTimeComparer.IsLessThan:
                    if ((DateTime)propertyValue < (DateTime)firstValue)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DateTimeComparer.IsLessThanOrEqualTo:
                    if ((DateTime)propertyValue <= (DateTime)firstValue)
                    {
                        return ValidationResult.Success;
                    }
                    break;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }


    public enum DateTimeComparer
    {
        IsEqualTo,
        IsLessThan,
        IsGreaterThan,
        IsLessThanOrEqualTo,
        IsGreaterThanOrEqualTo
    }
}