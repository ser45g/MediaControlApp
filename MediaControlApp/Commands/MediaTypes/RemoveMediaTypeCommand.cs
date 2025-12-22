namespace MediaControlApp.Commands.MediaTypes
{
    using MediaControlApp.Application.Services;
    using MediaControlApp.Domain.Models.Media;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System;
    using System.ComponentModel;

    [Description("Remove a media type.")]
    public class RemoveMediaTypeCommand : AsyncCommand<RemoveMediaTypeCommand.Settings>
    {
       
        private readonly MediaTypeService _mediaTypeService;

       
        public RemoveMediaTypeCommand(MediaTypeService mediaTypeService)
        {
            _mediaTypeService = mediaTypeService;
        }

    
        public sealed class Settings : CommandSettings
        {
        
            [CommandArgument(0, "[MEDIATYPEID]")]
            [Description("The media type's id to delete if.")]
            public string? MediaTypeId { get; set; }

         
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
                     await HandleRemove();
                }
                else
                {
                    await HandleRemoveWithShowSelect(settings.MediaTypeId);
                }

            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }

            return 0;
        }

        private async Task HandleRemove()
        {
            var mediaTypes = await _mediaTypeService.GetAll();

            var mediaType = AnsiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x => x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }
            Guid selectedMediaTypeId = mediaType.Id;

            await _mediaTypeService.Remove(selectedMediaTypeId);
            AnsiConsole.MarkupLine($"[green]Media Type [[{mediaType.Name}]] was successfully deleted![/]");
        }

        private async Task HandleRemoveWithShowSelect(string? mediaTypeId)
        {

            var mediaTypeIdValidationResult = MediaTypeValidationUtils.ValidateMediaTypeId(mediaTypeId);

            if (mediaTypeIdValidationResult.Successful)
            {
                Guid mediaTypeIdGuid = Guid.Parse(mediaTypeId!);

                await _mediaTypeService.Remove(mediaTypeIdGuid);
                AnsiConsole.MarkupLine($"[green]Media Type with Id [[{mediaTypeIdGuid}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception(mediaTypeIdValidationResult.Message);
            }
        }
    }
}
