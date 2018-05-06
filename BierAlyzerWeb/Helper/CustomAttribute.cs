using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BierAlyzerWeb.Helper
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Validates wether an mail address is already registered or not</summary>
    /// <remarks>   Andre Beging, 03.05.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class MailNotRegisteredAttribute : ValidationAttribute
    {
        #region IsValid

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Is valid. </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        /// <param name="value">                The value. </param>
        /// <param name="validationContext">    Context for the validation. </param>
        /// <returns>   A ValidationResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is string mailAdress)) return null;

            using (var context = ContextHelper.OpenContext())
            {
                if (context.User.Any(u => u.Mail.ToLower() == mailAdress.ToLower()))
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }

        #endregion
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Validates wether a string can be successfull parsed to double or not </summary>
    /// <remarks>   Andre Beging, 03.05.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class IsStringDoubleAttribute : ValidationAttribute
    {
        #region IsValid

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Is valid. </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        /// <param name="value">                The value. </param>
        /// <param name="validationContext">    Context for the validation. </param>
        /// <returns>   A ValidationResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            if (!double.TryParse(value.ToString(), out var _))
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return null;
        }

        #endregion
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Validates wether a string is valid mail address or not </summary>
    /// <remarks>   Andre Beging, 03.05.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ValidMailAttribute : ValidationAttribute
    {
        #region IsValid

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Is valid. </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        /// <param name="value">                The value. </param>
        /// <param name="validationContext">    Context for the validation. </param>
        /// <returns>   A ValidationResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            var email = value.ToString();

            if (new EmailAddressAttribute().IsValid(email))
                return null;

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        #endregion
    }

    public class DateLaterThanAttribute : ValidationAttribute
    {
        public string EarlierDateProperty;

        public DateLaterThanAttribute() { }

        public DateLaterThanAttribute(string earlierDateProperty)
        {
            EarlierDateProperty = earlierDateProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Default error return value
            var errorReturnValue = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            try
            {
                if (value == null) return errorReturnValue;
                if (!(value is DateTime targetDateTime)) return errorReturnValue;

                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(EarlierDateProperty);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);

                if (extensionValue == null) return errorReturnValue;
                if (!(extensionValue is DateTime earlierDate)) return errorReturnValue;

                if (targetDateTime > earlierDate)
                    return null;
            }
            catch (Exception)
            {
                // ignored
            }

            return errorReturnValue;
        }
    }
}