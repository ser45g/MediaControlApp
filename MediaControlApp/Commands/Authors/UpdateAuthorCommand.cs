using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.SharedSettings;
using MediaControlApp.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.MediaTypes
{
    [Description("Update an author")]
    public class UpdateAuthorCommand : AsyncCommand<UpdateAuthorCommand.Settings>
    {
        private readonly IAuthorService _authorService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IAuthorValidationUtils _authorValidationUtils;

        public UpdateAuthorCommand(IAuthorService authorService, IAnsiConsole ansiConsole, IAuthorValidationUtils authorValidationUtils)
        {
            _authorService = authorService;
            _ansiConsole = ansiConsole;
            _authorValidationUtils = authorValidationUtils;
        }

        public sealed class Settings : SelectableSettings
        {

            [CommandArgument(0, "[AUTHORID]")]
            [Description("The media type's id to delete it")]
            public string? Id { get; init; }


            [CommandArgument(0, "[AUTHORNAME]")]
            [Description("The new name. It must be unique")]
            public string? Name { get; init; }

            [CommandArgument(0, "[COMPANYNAME]")]
            [Description("The new description")]
            public string? CompanyName { get; init; }

            [CommandArgument(0, "[EMAIL]")]
            [Description("The new email")]
            public string? Email { get; init; }
        }

        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (!settings.ShowSelect)
            {
                var authorIdValidationResult = _authorValidationUtils.ValidateAuthorId(settings.Id);

                var authorNameValidationTask = _authorValidationUtils.ValidateName(settings.Name);

                authorNameValidationTask.Wait();

                var authorNameValidationResult = authorNameValidationTask.Result;

                if (!authorIdValidationResult.Successful)
                    return authorIdValidationResult;

                if (!authorNameValidationResult.Successful)
                    return authorNameValidationResult;
            }

            return base.Validate(context, settings);
        }

        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {

            if (settings.ShowSelect)
            {
                await HandleUpdate(cancellationToken);
            }
            else
            {
                await HandleUpdateWithShowSelect(settings.Id!, settings.Name!, settings.CompanyName, settings.Email, cancellationToken);
            }

            return 0;
        }

        private async Task HandleUpdateWithShowSelect(string authorId, string authorName, string? companyName, string? email, CancellationToken cancellationToken = default)
        {
            Guid authorIdGuid = Guid.Parse(authorId!);

            await _authorService.Update(authorIdGuid, authorName!, companyName, email, cancellationToken);
            _ansiConsole.MarkupLine($"[green]Author with Id [[{authorIdGuid}]] was successfully updated![/]");
        }

        private async Task HandleUpdate(CancellationToken cancellationToken = default)
        {
            var authors = await _authorService.GetAll(cancellationToken);

            if (!authors.Any())
            {
                throw new Exception("No authors available");
            }

            var author = _ansiConsole.Prompt(new SelectionPrompt<Author>().Title("Please select the author you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(authors).UseConverter(x => x.Name));

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            var newName = _ansiConsole.Prompt(new TextPrompt<string>("Enter a new name: ").DefaultValue(author.Name).Validate(x =>
            {
                var authorNameValidationTask = _authorValidationUtils.ValidateName(x);
                authorNameValidationTask.Wait();

                var authorNameValidationResult = authorNameValidationTask.Result;

                return authorNameValidationResult.Successful;
            }));

            var newCompanyName = _ansiConsole.Prompt(new TextPrompt<string?>("Enter a new company name: ").DefaultValue(author.CompanyName).AllowEmpty());

            var newEmail = _ansiConsole.Prompt(new TextPrompt<string?>("Enter a new email: ").DefaultValue(author.Email).AllowEmpty());

            await _authorService.Update(author.Id, newName, companyName: newCompanyName, email: newEmail, cancellationToken);
            _ansiConsole.MarkupLine($"[green]Author was successfully updated![/]");
        }


    }
}
