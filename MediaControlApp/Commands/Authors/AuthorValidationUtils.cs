using MediaControlApp.Application.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Commands.Authors
{
    public static class AuthorValidationUtils
    {
        public static async Task<ValidationResult> ValidateName(AuthorService authorService, string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ValidationResult.Error("Media Name can't be empty");
            }

            var checkNameIsUniqueFunc = async () =>
            {
                var res = await authorService.GetByName(name);
                return res == null;
            };
            var checkNameIsUniqueTask = checkNameIsUniqueFunc();

            checkNameIsUniqueTask.Wait();

            if (!checkNameIsUniqueTask.Result)
            {
                return ValidationResult.Error("Auhtor Name must be unique");
            }

            return ValidationResult.Success();
        }


        public static ValidationResult ValidateAuthorId(string? authorId)
        {
            if (authorId != null)
            {
                bool isValidGuid = Guid.TryParse(authorId, out Guid res);
                if (!isValidGuid)
                    return ValidationResult.Error("Invalid format for Author Id");

            }
            else
            {
                return ValidationResult.Error("Id can't be empty");
            }
            return ValidationResult.Success();
        }
    }
}
