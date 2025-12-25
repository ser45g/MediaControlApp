using MediaControlApp.Application.Services;
using MediaControlApp.Commands.MediaTypes;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Commands.Ganres
{
    public static class GanreValidationUtils
    {
        public static async Task<ValidationResult> ValidateName(GanreService ganreService, string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ValidationResult.Error("Ganre Name can't be empty");
            }

            var checkNameIsUniqueFunc = async () =>
            {
                var res = await ganreService.GetByName(name.ToUpper());
                return res == null;
            };
            var checkNameIsUniqueTask = checkNameIsUniqueFunc();

            checkNameIsUniqueTask.Wait();

            if (!checkNameIsUniqueTask.Result)
            {
                return ValidationResult.Error("Ganre Name must be unique");
            }

            return ValidationResult.Success();
        }
        public static ValidationResult ValidateGanreId(string? ganreId)
        {
            if (ganreId!= null)
            {
                bool isValidGuid = Guid.TryParse(ganreId, out Guid res);
                if (!isValidGuid)
                    return ValidationResult.Error("Invalid format for Ganre Id");

            }
            else
            {
                return ValidationResult.Error("Id can't be empty");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateMediaTypeId(string? mediaTypeId)
        {
            return MediaTypeValidationUtils.ValidateMediaTypeId(mediaTypeId);
        }
    }
}
