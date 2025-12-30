using Spectre.Console;

namespace MediaControlApp.Validators
{
    public interface IAuthorValidationUtils
    {
        ValidationResult ValidateAuthorId(string? authorId);
        Task<ValidationResult> ValidateName(string? name);
    }
}