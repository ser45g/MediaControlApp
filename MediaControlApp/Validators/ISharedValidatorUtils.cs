using Spectre.Console;

namespace MediaControlApp.Validators
{
    public interface ISharedValidatorUtils
    {
        ValidationResult ValidateDate(string? date);
        ValidationResult ValidateGuid(string? guid, string? emptyMessage = null, string? invalidFormatMessage = null);
    }
}