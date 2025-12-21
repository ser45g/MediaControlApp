namespace MediaControlApp.Commands.MediaTypes
{
    using MediaControlApp.Application.Services;
    using MediaControlApp.Domain.Models.Media;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Defines the <see cref="RemoveAuthorCommand" />
    /// </summary>
    public class RemoveAuthorCommand : AsyncCommand<RemoveAuthorCommand.Settings>
    {
        /// <summary>
        /// Defines the _authorService
        /// </summary>
        private readonly AuthorService _authorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveAuthorCommand"/> class.
        /// </summary>
        /// <param name="authorService">The authorService<see cref="AuthorService"/></param>
        public RemoveAuthorCommand(AuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Defines the <see cref="Settings" />
        /// </summary>
        public sealed class Settings : CommandSettings
        {
            /// <summary>
            /// Gets or sets the AuthorId
            /// </summary>
            [CommandArgument(0, "<AUTHORID>")]
            [Description("The author's id to delete if.")]
            public string? AuthorId { get; set; }

            [CommandOption("-s|--show-select")]
            [DefaultValue(false)]
            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool ShowSelect { get; set; }
        }

        /// <summary>
        /// The HandleRemove
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task HandleRemove()
        {
            var authors = await _authorService.GetAll();

            var author = AnsiConsole.Prompt(new SelectionPrompt<Author>().Title("Please select the author you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(authors).UseConverter(x => x.Name));

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            Guid selectedAuthorId = author.Id;

            await _authorService.Remove(selectedAuthorId);
            AnsiConsole.MarkupLine($"[green]Author [[{author.Name}]] was successfully deleted![/]");
        }

        /// <summary>
        /// The HandleRemoveWithShowSelect
        /// </summary>
        /// <param name="authorId">The authorId<see cref="string?"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task HandleRemoveWithShowSelect(string? authorId)
        {
            if (authorId != null)
            {
                Guid authorIdGuid = Guid.Parse(authorId);

                await _authorService.Remove(authorIdGuid);
                AnsiConsole.MarkupLine($"[green]Author with Id [[{authorIdGuid}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception("Author id must be provided");
            }
        }

        /// <summary>
        /// The ExecuteAsync
        /// </summary>
        /// <param name="context">The context<see cref="CommandContext"/></param>
        /// <param name="settings">The settings<see cref="Settings"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{int}"/></returns>
        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {

            try
            {
                if (settings.ShowSelect)
                {
                    await HandleRemoveWithShowSelect(settings.AuthorId);
                }
                else
                {
                    await HandleRemove();
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
