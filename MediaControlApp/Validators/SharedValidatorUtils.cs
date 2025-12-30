using MediaControlApp.Domain.Models.Media;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Validators
{
    public class SharedValidatorUtils : ISharedValidatorUtils
    {
        public ValidationResult ValidateGuid(string? guid, string? emptyMessage = null, string? invalidFormatMessage = null)
        {
            if (guid != null)
            {
                bool isValidGuid = Guid.TryParse(guid, out Guid res);
                if (!isValidGuid)
                    return ValidationResult.Error(invalidFormatMessage ?? "Invalid format for the GUID value");

            }
            else
            {
                return ValidationResult.Error(emptyMessage ?? "Value cannot be empty");
            }
            return ValidationResult.Success();
        }

        public ValidationResult ValidateDate(string? date)
        {
            if (date != null)
            {
                bool isValidDate = DateTime.TryParse(date, out DateTime val);
                if (!isValidDate)
                {
                    return ValidationResult.Error("Can't parse the date value");
                }
                return ValidationResult.Success();
            }
            else
            {
                return ValidationResult.Error("Date cannot be empty");
            }
        }
    }
}
