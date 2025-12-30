using Spectre.Console;

namespace MediaControlApp.Validators
{
    public interface IMediaValidationUtils
    {
        Task<ValidationResult> Validate(string? title, string? ganreId, string? authorId, string? publishedDate, string? lastConsumedDate, string? rating);
        ValidationResult ValidateAuthorId(string? authorId);
        ValidationResult ValidateGanreId(string? ganreId);
        ValidationResult ValidateLastConsumedDate(string? lastConsumedDate);
        ValidationResult ValidateMediaId(string? mediaId);
        ValidationResult ValidatePublishedDate(string? publishedDate);
        ValidationResult ValidateRating(string? rating);
        Task<ValidationResult> ValidateTitle(string? title);
    }
}