namespace MediaControlApp.Commands.MediaTypes
{
    using MediaControlApp.Application.Services;
    using MediaControlApp.Commands.Authors;
    using MediaControlApp.Domain.Models.Media;
    using MediaControlApp.SharedSettings;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System;
    using System.ComponentModel;

    [Description("Remove an author")]
    public class RemoveAuthorCommand : AsyncCommand<RemoveAuthorCommand.Settings>
    {
        private readonly AuthorService _authorService;
        private readonly IAnsiConsole _ansiConsole;

        public RemoveAuthorCommand(AuthorService authorService, IAnsiConsole ansiConsole)
        {
            _authorService = authorService;
            _ansiConsole = ansiConsole;
        }

        public sealed class Settings : SelectableSettings
        {
            [CommandArgument(0, "[AUTHORID]")]
            [Description("The author's id to delete it.")]
            public string? AuthorId { get; init; }

        }

        private async Task HandleRemove()
        {
            var authors = await _authorService.GetAll();

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

            await _authorService.Remove(selectedAuthorId);

            _ansiConsole.MarkupLine($"[green]Author [[{author.Name}]] was successfully deleted![/]");
        }


        private async Task HandleRemoveWithShowSelect(string authorId)
        {
            var mediaTypeIdValidationResult = AuthorValidationUtils.ValidateAuthorId(authorId);

            if (mediaTypeIdValidationResult.Successful)
            {
                Guid authorIdGuid = Guid.Parse(authorId);

                await _authorService.Remove(authorIdGuid);
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
                await HandleRemove();
            }
            else
            {
                await HandleRemoveWithShowSelect(settings.AuthorId);
            }

            return 0;
        }
    }
}
