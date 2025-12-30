namespace MediaControlApp.Commands.MediaTypes
{
    using MediaControlApp.Application.Services;
    using MediaControlApp.Domain.Models.Media;
    using MediaControlApp.SharedSettings;
    using MediaControlApp.Validators;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System;
    using System.ComponentModel;

    [Description("Remove an author")]
    public class RemoveAuthorCommand : AsyncCommand<RemoveAuthorCommand.Settings>
    {
        private readonly IAuthorService _authorService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IAuthorValidationUtils _authorValidationUtils;

        public RemoveAuthorCommand(IAuthorService authorService, IAnsiConsole ansiConsole, IAuthorValidationUtils authorValidationUtils)
        {
            _authorService = authorService;
            _ansiConsole = ansiConsole;
            _authorValidationUtils = authorValidationUtils;
        }

        public sealed class Settings : SelectableSettings
        {
            [CommandArgument(0, "[AUTHORID]")]
            [Description("The author's id to delete it.")]
            public string? AuthorId { get; init; }

        }

        private async Task HandleRemove(CancellationToken cancellationToken=default)
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
            Guid selectedAuthorId = author.Id;

            await _authorService.Remove(selectedAuthorId, cancellationToken);

            _ansiConsole.MarkupLine($"[green]Author [[{author.Name}]] was successfully deleted![/]");
        }


        private async Task HandleRemoveWithShowSelect(string authorId, CancellationToken cancellationToken = default)
        {
            var mediaTypeIdValidationResult = _authorValidationUtils.ValidateAuthorId(authorId);

            if (mediaTypeIdValidationResult.Successful)
            {
                Guid authorIdGuid = Guid.Parse(authorId);

                await _authorService.Remove(authorIdGuid, cancellationToken);
                _ansiConsole.MarkupLine($"[green]Author with Id [[{authorIdGuid}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception(mediaTypeIdValidationResult.Message);
            }
        }

        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {

            if (settings.ShowSelect)
            {
                await HandleRemove(cancellationToken);
            }
            else
            {
                await HandleRemoveWithShowSelect(settings.AuthorId, cancellationToken);
            }

            return 0;
        }
    }
}
