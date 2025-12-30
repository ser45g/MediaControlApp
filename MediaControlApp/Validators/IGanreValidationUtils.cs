using Spectre.Console;

namespace MediaControlApp.Validators
{
    public interface IGanreValidationUtils
    {
        ValidationResult ValidateGanreId(string? ganreId);
        ValidationResult ValidateMediaTypeId(string? mediaTypeId);
        Task<ValidationResult> ValidateName(string? name);
    }
}