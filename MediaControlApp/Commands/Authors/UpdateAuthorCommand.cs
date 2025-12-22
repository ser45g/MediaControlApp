using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Authors;
using MediaControlApp.Domain.Models.Media;

using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;


namespace MediaControlApp.Commands.MediaTypes
{
    [Description("Update an author")]
    public class UpdateAuthorCommand : AsyncCommand<UpdateAuthorCommand.Settings>
    {
        private readonly AuthorService _authorService;

        public UpdateAuthorCommand(AuthorService authorService)
        {
            _authorService = authorService;
        }

        public sealed class Settings : CommandSettings
        {
            
            [CommandArgument(0, "[AUTHORID]")]
            [Description("The media type's id to delete if.")]
            public string? AuthorId { get; set; }

           
            [CommandArgument(0, "[AUTHORNAME]")]
            [Description("The media type name to add. It must be unique")]
            public string? AuthorName { get; set; }

            [CommandArgument(0, "[COMPANYNAME]")]
            [Description("The media type name to add. It must be unique")]
            public string? CompanyName { get; set; }

            [CommandArgument(0, "[EMAIL]")]
            [Description("The media type name to add. It must be unique")]
            public string? Email { get; set; }

            
            [CommandOption("-s|--show-select")]
            [DefaultValue(false)]
            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool ShowSelect { get; set; }
        }

        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (!settings.ShowSelect)
            {
                var authorIdValidationResult = AuthorValidationUtils.ValidateAuthorId(settings.AuthorId);

                var authorNameValidationTask = AuthorValidationUtils.ValidateName(_authorService, settings.AuthorName);

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
            try
            {
                if (settings.ShowSelect)
                {
                    await HandleUpdate();
                }
                else
                {
                    await HandleUpdateWithShowSelect(settings.AuthorId!, settings.AuthorName!, settings.CompanyName, settings.Email);
                }

            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }

            return 0;
        }


        private async Task HandleUpdateWithShowSelect(string authorId, string authorName, string? companyName, string? email)
        {
            Guid authorIdGuid = Guid.Parse(authorId!);

            await _authorService.Update(authorIdGuid, authorName!, companyName, email);
            AnsiConsole.MarkupLine($"[green]Author with Id [[{authorIdGuid}]] was successfully deleted![/]");           
        }

        private async Task HandleUpdate()
        {
            var authors = await _authorService.GetAll();

            var author = AnsiConsole.Prompt(new SelectionPrompt<Author>().Title("Please select the author you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(authors).UseConverter(x => x.Name));

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            var newName = AnsiConsole.Prompt(new TextPrompt<string>("Enter a new name: ").DefaultValue(author.Name).Validate(x =>
            {
                var authorNameValidationTask = AuthorValidationUtils.ValidateName(_authorService, x);
                authorNameValidationTask.Wait();

                var authorNameValidationResult = authorNameValidationTask.Result;

                return authorNameValidationResult.Successful;
            }));

            var newCompanyName = AnsiConsole.Prompt(new TextPrompt<string?>("Enter a new company name: ").DefaultValue(author.CompanyName));

            var newEmail = AnsiConsole.Prompt(new TextPrompt<string?>("Enter a new email: ").DefaultValue(author.Email));

            await _authorService.Update(author.Id, newName, companyName:newCompanyName, email:newEmail);
            AnsiConsole.MarkupLine($"[green]Author was successfully updated![/]");
        }


    }
}
