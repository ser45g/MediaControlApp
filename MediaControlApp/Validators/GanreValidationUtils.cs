using MediaControlApp.Application.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Validators
{
    public class GanreValidationUtils : IGanreValidationUtils
    {
        private readonly ISharedValidatorUtils _sharedValidationUtils;
        private readonly IMediaTypeValidationUtils _mediaTypeValidationUtils;
        private readonly IGanreService _ganreService;

        public GanreValidationUtils(ISharedValidatorUtils sharedValidationUtils, IGanreService ganreService, IMediaTypeValidationUtils mediaTypeValidationUtils)
        {
            _sharedValidationUtils = sharedValidationUtils;
            _ganreService = ganreService;
            _mediaTypeValidationUtils = mediaTypeValidationUtils;
        }

        public async Task<ValidationResult> ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ValidationResult.Error("Ganre Name can't be empty");
            }

            var checkNameIsUniqueFunc = async () =>
            {
                var res = await _ganreService.GetByName(name.ToUpper());
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
        public ValidationResult ValidateGanreId(string? ganreId)
        {
            return _sharedValidationUtils.ValidateGuid(ganreId);
        }

        public ValidationResult ValidateMediaTypeId(string? mediaTypeId)
        {
            return _mediaTypeValidationUtils.ValidateMediaTypeId(mediaTypeId);
        }
    }
}
