namespace MediaControlApp.Commands.MediaTypes
{
    using MediaControlApp.Application.Services.Interfaces;
    using MediaControlApp.Domain.Models.Media;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Defines the <see cref="RemoveMediaTypeCommand" />
    /// </summary>
    public class RemoveMediaTypeCommand : AsyncCommand<RemoveMediaTypeCommand.Settings>
    {
        /// <summary>
        /// Defines the _mediaTypeRepo
        /// </summary>
        private readonly IMediaTypeRepo _mediaTypeRepo;

        /// <summary>
        /// Defines the <see cref="Settings" />
        /// </summary>
        public sealed class Settings : CommandSettings
        {
            /// <summary>
            /// Gets or sets the MediaTypeId
            /// </summary>
            [CommandArgument(0, "<MEDIATYPEID>")]
            [Description("The media type's id to delete if.")]
            public string? MediaTypeId { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether ShowSelect
            /// </summary>
            [CommandOption("-s|--show-select")]
            [DefaultValue(false)]
            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool ShowSelect { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveMediaTypeCommand"/> class.
        /// </summary>
        /// <param name="mediaTypeRepo">The mediaTypeRepo<see cref="IMediaTypeRepo"/></param>
        public RemoveMediaTypeCommand(IMediaTypeRepo mediaTypeRepo)
        {
            _mediaTypeRepo = mediaTypeRepo;
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
                    await HandleRemoveWithShowSelect(settings.MediaTypeId);
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

        /// <summary>
        /// The HandleRemove
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task HandleRemove()
        {
            var mediaTypes = await _mediaTypeRepo.GetAll();

            var mediaType = AnsiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x => x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }
            Guid selectedMediaTypeId = mediaType.Id;

            await _mediaTypeRepo.Remove(selectedMediaTypeId);
            AnsiConsole.MarkupLine($"[green]Media Type [[{mediaType.Name}]] was successfully deleted![/]");
        }

        /// <summary>
        /// The HandleRemoveWithShowSelect
        /// </summary>
        /// <param name="mediaTypeId">The mediaTypeId<see cref="string?"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task HandleRemoveWithShowSelect(string? mediaTypeId)
        {
            if (mediaTypeId != null)
            {
                Guid mediaTypeIdGuid = Guid.Parse(mediaTypeId);

                await _mediaTypeRepo.Remove(mediaTypeIdGuid);
                AnsiConsole.MarkupLine($"[green]Media Type with Id [[{mediaTypeIdGuid}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception("Media type id must be provided");
            }
        }
    }
}
