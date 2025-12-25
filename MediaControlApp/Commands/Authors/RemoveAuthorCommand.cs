namespace MediaControlApp.Commands.MediaTypes
{
    using MediaControlApp.Application.Services;
    using MediaControlApp.Commands.Authors;
    using MediaControlApp.Domain.Models.Media;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System;
    using System.ComponentModel;

    [Description("Remove an author")]
    public class RemoveAuthorCommand : AsyncCommand<RemoveAuthorCommand.Settings>
    {  
        private readonly AuthorService _authorService;

        public RemoveAuthorCommand(AuthorService authorService)
        {
            _authorService = authorService;
        }
       
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "[AUTHORID]")]
            [Description("The author's id to delete it.")]
            public string? AuthorId { get; init; }

            [CommandOption("-s|--show-select")]
            [DefaultValue(false)]
            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool ShowSelect { get; init; }
        }
    
        private async Task HandleRemove()
        {
            var authors = await _authorService.GetAll();

            if (!authors.Any())
            {
                throw new Exception("No authors available");
            }

            var author = AnsiConsole.Prompt(new SelectionPrompt<Author>().Title("Please select the author you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(authors).UseConverter(x => x.Name));

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            Guid selectedAuthorId = author.Id;

            await _authorService.Remove(selectedAuthorId);

            AnsiConsole.MarkupLine($"[green]Author [[{author.Name}]] was successfully deleted![/]");
        }

    
        private async Task HandleRemoveWithShowSelect(string authorId)
        {
            var mediaTypeIdValidationResult = AuthorValidationUtils.ValidateAuthorId(authorId);

            if (mediaTypeIdValidationResult.Successful)
            {
                 Guid authorIdGuid = Guid.Parse(authorId);

                await _authorService.Remove(authorIdGuid);
                AnsiConsole.MarkupLine($"[green]Author with Id [[{authorIdGuid}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception(mediaTypeIdValidationResult.Message);
            }       
        }
     
        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            try
            {
                if (settings.ShowSelect)
                {
                    await HandleRemove();
                }
                else
                {
                    await HandleRemoveWithShowSelect(settings.AuthorId);
                }
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
