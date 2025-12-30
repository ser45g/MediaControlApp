using MediaControlApp.Application.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Validators
{
    public class MediaTypeValidationUtils : IMediaTypeValidationUtils
    {
        private readonly ISharedValidatorUtils _sharedValidatorUtils;
        private readonly IMediaTypeService _mediaTypeService;

        public MediaTypeValidationUtils(ISharedValidatorUtils sharedValidatorUtils, IMediaTypeService mediaTypeService)
        {
            _sharedValidatorUtils = sharedValidatorUtils;
            _mediaTypeService = mediaTypeService;
        }

        public async Task<ValidationResult> ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ValidationResult.Error("Media Name can't be empty");
            }

            var checkNameIsUniqueFunc = async () =>
            {
                var res = await _mediaTypeService.GetByName(name.ToUpper());
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

        public ValidationResult ValidateMediaTypeId(string? mediaTypeId)
        {
            return _sharedValidatorUtils.ValidateGuid(mediaTypeId);
        }
    }
}
