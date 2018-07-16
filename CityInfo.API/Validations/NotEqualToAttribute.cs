using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CityInfo.API.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class NotEqualToAttribute : ValidationAttribute
    {
        public string OtherProperty { get; private set; }
        public string OtherPropertyDisplayName { get; internal set; }
        public override bool RequiresValidationContext => true;


        public NotEqualToAttribute(string otherProperty)
        {
            OtherProperty = otherProperty ?? throw new ArgumentNullException("otherProperty");
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, base.ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo runtimeProperty = RuntimeReflectionExtensions.GetRuntimeProperty(validationContext.ObjectType, OtherProperty);
            if (runtimeProperty == (PropertyInfo)null)
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "CompareAttribute_UnknownProperty - {0}", OtherProperty));
            }
            if (runtimeProperty.GetIndexParameters().Any())
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Common_PropertyNotFound", validationContext.ObjectType.FullName, OtherProperty));
            }

            object value2 = runtimeProperty.GetValue(validationContext.ObjectInstance, null);
            if (object.Equals(value, value2))
            {
                if (OtherPropertyDisplayName == null)
                {
                    OtherPropertyDisplayName = GetDisplayNameForProperty(validationContext.ObjectType, OtherProperty);
                }
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        private static string GetDisplayNameForProperty(Type containerType, string propertyName)
        {
            PropertyInfo propertyInfo = RuntimeReflectionExtensions.GetRuntimeProperties(containerType).SingleOrDefault(delegate (PropertyInfo prop)
            {
                if (IsPublic(prop))
                {
                    return string.Equals(propertyName, prop.Name, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            });
            if (propertyInfo == (PropertyInfo)null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Common_PropertyNotFound", containerType.FullName, propertyName));
            }
            IEnumerable<Attribute> customAttributes = CustomAttributeExtensions.GetCustomAttributes(propertyInfo, true);
            DisplayAttribute displayAttribute = customAttributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (displayAttribute != null)
            {
                return displayAttribute.GetName();
            }
            return propertyName;
        }

        private static bool IsPublic(PropertyInfo p)
        {
            if (!(p.GetMethod != (MethodInfo)null) || !p.GetMethod.IsPublic)
            {
                if (p.SetMethod != (MethodInfo)null)
                {
                    return p.SetMethod.IsPublic;
                }
                return false;
            }
            return true;
        }

    }
}
