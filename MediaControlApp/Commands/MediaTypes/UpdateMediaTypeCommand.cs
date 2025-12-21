namespace MediaControlApp.Commands.MediaTypes
{
    using MediaControlApp.Application.Services;
    using MediaControlApp.Domain.Models.Media;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System;
    using System.ComponentModel;


    public class UpdateMediaTypeCommand : AsyncCommand<UpdateMediaTypeCommand.Settings>
    {
 
        private readonly MediaTypeService _mediaTypeService;

        public UpdateMediaTypeCommand(MediaTypeService mediaTypeService)
        {
            _mediaTypeService = mediaTypeService;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<MEDIATYPEID>")]
            [Description("The media type's id to delete if.")]
            public string? MediaTypeId { get; set; }

            [CommandArgument(0, "<MEDIATYPENAME>")]
            [Description("The media type name to add. It must be unique")]
            public string? MediaTypeName { get; set; }

            [CommandOption("-s|--show-select")]
            [DefaultValue(false)]
            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool ShowSelect { get; set; }
        }



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

        private async Task HandleUpdateWithShowSelect(string? mediaTypeId, string? mediaTypeName)
        {
            if (mediaTypeId != null && mediaTypeName != null)
            {
                Guid mediaTypeIdGuid = Guid.Parse(mediaTypeId);

                await _mediaTypeService.Update(mediaTypeIdGuid, mediaTypeName);
                AnsiConsole.MarkupLine($"[green]Media Type with Id [[{mediaTypeIdGuid}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception("Media type id and Media type name must be provided");
            }
        }

        private async Task HandleUpdate()
        {
            var mediaTypes = await _mediaTypeService.GetAll();

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

            await _mediaTypeService.Update(mediaType.Id, newName);
            AnsiConsole.MarkupLine($"[green]Media Type was successfully updated![/]");
        }
    }
}
