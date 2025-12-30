using Spectre.Console;

namespace MediaControlApp.Validators
{
    public interface IMediaTypeValidationUtils
    {
        ValidationResult ValidateMediaTypeId(string? mediaTypeId);
        Task<ValidationResult> ValidateName(string? name);
    }
}