using MediaControlApp.Application.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Commands.MediaTypes
{
    public static class MediaTypeValidationUtils
    {
        public static async Task<ValidationResult> ValidateName(MediaTypeService mediaTypeService, string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ValidationResult.Error("Media Name can't be empty");
            }

            var checkNameIsUniqueFunc = async () =>
            {
                var res = await mediaTypeService.GetByName(name.ToUpper());
                return res == null;
            };
            var checkNameIsUniqueTask = checkNameIsUniqueFunc();

            checkNameIsUniqueTask.Wait();

            if (!checkNameIsUniqueTask.Result)
            {
                return ValidationResult.Error("Media Name must be unique");
            }

            return ValidationResult.Success();
        }


        public static ValidationResult ValidateMediaTypeId(string? mediaTypeId) {
            if (mediaTypeId != null)
            {
                bool isValidGuid = Guid.TryParse(mediaTypeId, out Guid res);
                if (!isValidGuid)
                    return ValidationResult.Error("Invalid format for Media Type Id");

            }
            else
            {
                return ValidationResult.Error("Id can't be empty");
            }
            return ValidationResult.Success();
        }
    }
}
