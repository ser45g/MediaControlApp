using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using Spectre.Console;

namespace MediaControlApp.Validators
{
    public class MediaValidationUtils : IMediaValidationUtils
    {

        private readonly IMediaService _mediaService;
        private readonly IGanreValidationUtils _ganreValidationUtils;
        private readonly IAuthorValidationUtils _authorValidationUtils;
        private readonly ISharedValidatorUtils _sharedValidationUtils;

        public MediaValidationUtils(IMediaService mediaService, IGanreValidationUtils ganreValidationUtils, IAuthorValidationUtils authorValidationUtils, ISharedValidatorUtils sharedValidationUtils)
        {
            _mediaService = mediaService;
            _ganreValidationUtils = ganreValidationUtils;
            _authorValidationUtils = authorValidationUtils;
            _sharedValidationUtils = sharedValidationUtils;
        }

        public async Task<ValidationResult> Validate(string? title,
            string? ganreId, string? authorId, string? publishedDate, string? lastConsumedDate, string? rating)
        {
            var titleValidationResult = await ValidateTitle(title);
            if (!titleValidationResult.Successful)
            {
                return titleValidationResult;
            }

            var ganreIdValidationResult = ValidateGanreId(ganreId);
            if (!ganreIdValidationResult.Successful)
            {
                return ganreIdValidationResult;
            }

            var authorIdValidationResult = ValidateAuthorId(authorId);
            if (!authorIdValidationResult.Successful)
            {
                return authorIdValidationResult;
            }

            var ratingValidationResult = ValidateRating(rating);
            if (!ratingValidationResult.Successful)
            {
                return ratingValidationResult;
            }

            var publishedDateValidationResult = ValidatePublishedDate(publishedDate);
            if (!publishedDateValidationResult.Successful)
            {
                return publishedDateValidationResult;
            }

            var lastConsumedDateValidationResult = ValidateLastConsumedDate(lastConsumedDate);
            if (!publishedDateValidationResult.Successful)
            {
                return lastConsumedDateValidationResult;
            }


            return ValidationResult.Success();
        }


        public async Task<ValidationResult> ValidateTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return ValidationResult.Error("Media title can't be empty");
            }

            var checkNameIsUniqueFunc = async () =>
            {
                var res = await _mediaService.GetByTitle(title.ToUpper());
                return res == null;
            };
            var checkNameIsUniqueTask = checkNameIsUniqueFunc();

            checkNameIsUniqueTask.Wait();

            if (!checkNameIsUniqueTask.Result)
            {
                return ValidationResult.Error("Title must be unique");
            }

            return ValidationResult.Success();
        }
        public ValidationResult ValidateGanreId(string? ganreId)
        {
            return _ganreValidationUtils.ValidateGanreId(ganreId);
        }

        public ValidationResult ValidateRating(string? rating)
        {
            if (rating != null)
            {
                bool isValidDouble = double.TryParse(rating, out double val);
                if (!isValidDouble)
                {
                    return ValidationResult.Error("Can't parse the rating value");
                }
                try
                {
                    Rating ratingObj = new Rating(val);
                    return ValidationResult.Success();
                }
                catch (Exception ex)
                {
                    return ValidationResult.Error(ex.Message);
                }

            }
            else
            {
                return ValidationResult.Error("Rating can't be empty");
            }
        }

        public ValidationResult ValidateAuthorId(string? authorId)
        {
            return _authorValidationUtils.ValidateAuthorId(authorId);
        }

        public ValidationResult ValidatePublishedDate(string? publishedDate)
        {
            return _sharedValidationUtils.ValidateDate(publishedDate);
        }

        public ValidationResult ValidateLastConsumedDate(string? lastConsumedDate)
        {
            return _sharedValidationUtils.ValidateDate(lastConsumedDate);
        }

        public ValidationResult ValidateMediaId(string? mediaId)
        {
            return _sharedValidationUtils.ValidateGuid(mediaId);
        }
    }
}
