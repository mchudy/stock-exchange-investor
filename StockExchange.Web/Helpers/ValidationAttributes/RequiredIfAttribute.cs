using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Helpers.ValidationAttributes
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly RequiredAttribute innerAttribute = new RequiredAttribute();
        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }

        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
        }

        public override bool IsValid(object value)
        {
            return innerAttribute.IsValid(value);
        }
    }
}