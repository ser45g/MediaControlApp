using MediaControlApp.Application.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Validators
{
    public class AuthorValidationUtils : IAuthorValidationUtils
    {
        public readonly IAuthorService _authorService;
        public readonly ISharedValidatorUtils _sharedValidatorUtils;

        public AuthorValidationUtils(IAuthorService authorService, ISharedValidatorUtils sharedValidatorUtils)
        {
            _authorService = authorService;
            _sharedValidatorUtils = sharedValidatorUtils;
        }

        public async Task<ValidationResult> ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ValidationResult.Error("Media Name can't be empty");
            }

            var checkNameIsUniqueFunc = async () =>
            {
                var res = await _authorService.GetByName(name);
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


        public ValidationResult ValidateAuthorId(string? authorId)
        {
            return _sharedValidatorUtils.ValidateGuid(authorId);
        }
    }
}
