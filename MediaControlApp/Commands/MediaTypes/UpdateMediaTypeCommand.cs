namespace MediaControlApp.Commands.MediaTypes
{
    using MediaControlApp.Application.Services.Interfaces;
    using MediaControlApp.Domain.Models.Media;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Defines the <see cref="UpdateMediaTypeCommand" />
    /// </summary>
    public class UpdateMediaTypeCommand : AsyncCommand<UpdateMediaTypeCommand.Settings>
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
            /// Gets or sets the MediaTypeName
            /// </summary>
            [CommandArgument(0, "<MEDIATYPENAME>")]
            [Description("The media type name to add. It must be unique")]
            public string? MediaTypeName { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether ShowSelect
            /// </summary>
            [CommandOption("-s|--show-select")]
            [DefaultValue(false)]
            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool ShowSelect { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMediaTypeCommand"/> class.
        /// </summary>
        /// <param name="mediaTypeRepo">The mediaTypeRepo<see cref="IMediaTypeRepo"/></param>
        public UpdateMediaTypeCommand(IMediaTypeRepo mediaTypeRepo)
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
                    await HandleUpdateWithShowSelect(settings.MediaTypeId, settings.MediaTypeName);
                }
                else
                {
                    await HandleUpdate();
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
        /// The HandleRemoveWithShowSelect
        /// </summary>
        /// <param name="mediaTypeId">The mediaTypeId<see cref="string?"/></param>
        /// <param name="mediaTypeName">The mediaTypeName<see cref="string?"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task HandleUpdateWithShowSelect(string? mediaTypeId, string? mediaTypeName)
        {
            if (mediaTypeId != null && mediaTypeName != null)
            {
                Guid mediaTypeIdGuid = Guid.Parse(mediaTypeId);

                await _mediaTypeRepo.Update(mediaTypeIdGuid, mediaTypeName);
                AnsiConsole.MarkupLine($"[green]Media Type with Id [[{mediaTypeIdGuid}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception("Media type id and Media type name must be provided");
            }
        }

        /// <summary>
        /// The HandleRemove
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task HandleUpdate()
        {
            var mediaTypes = await _mediaTypeRepo.GetAll();

            var mediaType = AnsiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x => x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            var newName = AnsiConsole.Prompt(new TextPrompt<string>("Enter a new name: ").Validate(x =>
            {
                if (string.IsNullOrWhiteSpace(x))
                    return false;

                return true;
            }));

            await _mediaTypeRepo.Update(mediaType.Id, newName);
            AnsiConsole.MarkupLine($"[green]Media Type was successfully updated![/]");
        }
    }
}
