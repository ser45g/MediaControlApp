using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Authors;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.MediaTypes
{
   
    [Description("Add an author.")]
    public sealed class AddAuthorCommand : AsyncCommand<AddAuthorCommand.Settings>
    {
        private readonly AuthorService _authorService;

        public AddAuthorCommand(AuthorService authorService)
        {
            _authorService = authorService;
        }
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<AUTHORNAME>")]
            [Description("The auhtor's name")]
            public required string Name { get; init; }

            [CommandArgument(1, "[COMPANYNAME]")]
            [Description("The company name")]
            public string? CompanyName { get; init; }

            [CommandArgument(2, "[EMAIL]")]
            [Description("The author's email")]
            public string? Email { get; init; }
        }
        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            var authorNameValidationTask = AuthorValidationUtils.ValidateName(_authorService, settings.Name);

            authorNameValidationTask.Wait();

            var authorNameValidationResult = authorNameValidationTask.Result;
           
            if (!authorNameValidationResult.Successful)
                return authorNameValidationResult;

            return base.Validate(context, settings);
        }
  
        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            try
            {
                await _authorService.Add(settings.Name, settings.CompanyName, settings.Email);
                AnsiConsole.MarkupLine($"[green]Author was successfully added![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }
            return 0;
        }
    }
}
