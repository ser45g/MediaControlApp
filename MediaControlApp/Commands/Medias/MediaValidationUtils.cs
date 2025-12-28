using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Authors;
using MediaControlApp.Commands.Ganres;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Commands.Medias
{
    public static class MediaValidationUtils
    {
        public static async Task<ValidationResult> Validate(IMediaService mediaService, string? title,
            string? ganreId, string? authorId, string? publishedDate, string? lastConsumedDate, string? rating)
        {
            var titleValidationResult = await ValidateTitle(mediaService, title);
            if (!titleValidationResult.Successful) { 
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


        public static async Task<ValidationResult> ValidateTitle(IMediaService mediaService, string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return ValidationResult.Error("Media title can't be empty");
            }

            var checkNameIsUniqueFunc = async () =>
            {
                var res = await mediaService.GetByTitle(title.ToUpper());
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
        public static ValidationResult ValidateGanreId(string? ganreId)
        {
            return GanreValidationUtils.ValidateGanreId(ganreId);
        }

        public static ValidationResult ValidateRating(string? rating)
        {
            if(rating != null)
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
                }catch(Exception ex){
                    return ValidationResult.Error(ex.Message);
                }

            }
            else
            {
                return ValidationResult.Error("Rating can't be empty");
            }
        }

        public static ValidationResult ValidateAuthorId(string? authorId)
        {
            return AuthorValidationUtils.ValidateAuthorId(authorId);
        }


        public static ValidationResult ValidatePublishedDate(string? publishedDate)
        {
            if (publishedDate != null)
            {
                bool isValidDate = DateTime.TryParse(publishedDate,out DateTime val);
                if (!isValidDate)
                {
                    return ValidationResult.Error("Can't parse publicashion date value");
                }
                return ValidationResult.Success();
            }
            else
            {
                return ValidationResult.Error("Published date cannot be empty");
            }
        }


        public static ValidationResult ValidateLastConsumedDate(string? lastConsumedDate)
        {
            if (lastConsumedDate != null)
            {
                bool isValidDate = DateTime.TryParse(lastConsumedDate, out DateTime val);
                if (!isValidDate)
                {
                    return ValidationResult.Error("Can't parse last consumed date value");
                }
                return ValidationResult.Success();
            }
            else
            {
                return ValidationResult.Error("Last consumed date cannot be empty");
            }
        }

        public static ValidationResult ValidateMediaId(string? mediaId)
        {
            if (mediaId != null)
            {
                bool isValidGuid = Guid.TryParse(mediaId, out Guid res);
                if (!isValidGuid)
                    return ValidationResult.Error("Invalid format for Media Id");

            }
            else
            {
                return ValidationResult.Error("Id can't be empty");
            }
            return ValidationResult.Success();
        }
    }
}
