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
        private readonly IAuthorService _authorService;
        private readonly IAnsiConsole _ansiConsole;

        public AddAuthorCommand(IAuthorService authorService, IAnsiConsole ansiConsole)
        {
            _authorService = authorService;
            _ansiConsole = ansiConsole;
        }
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<AUTHORNAME>")]
            [Description("The auhtor's name")]
            public required string Name { get; init; }

            [CommandArgument(1, "[COMPANYNAME]")]
            [Description("The company name")]
            public string? CompanyName { get; init; }

            [CommandArgument(1, "[EMAIL]")]
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
            await _authorService.Add(settings.Name, settings.CompanyName, settings.Email);
            _ansiConsole.MarkupLine($"[green]Author was successfully added![/]");

            return 0;
        }
    }
}
