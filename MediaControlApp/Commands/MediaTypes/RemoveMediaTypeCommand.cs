namespace MediaControlApp.Commands.MediaTypes
{
    using MediaControlApp.Application.Services;
    using MediaControlApp.Domain.Models.Media;
    using MediaControlApp.SharedSettings;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System;
    using System.ComponentModel;

    [Description("Remove a media type.")]
    public class RemoveMediaTypeCommand : AsyncCommand<RemoveMediaTypeCommand.Settings>
    {
       
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IAnsiConsole _ansiConsole;



        public RemoveMediaTypeCommand(IMediaTypeService mediaTypeService)
        {
            _mediaTypeService = mediaTypeService;
        }

    
        public sealed class Settings : SelectableSettings
        {
        
            [CommandArgument(0, "[MEDIATYPEID]")]
            [Description("The media type's id to delete it")]
            public string? Id { get; init; }

        }


        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
           
            if (settings.ShowSelect)
            {
                 await HandleRemove();
            }
            else
            {
                await HandleRemoveWithShowSelect(settings.Id);
            }

           
            return 0;
        }

        private async Task HandleRemove()
        {
            var mediaTypes = await _mediaTypeService.GetAll();

            if (!mediaTypes.Any())
            {
                throw new Exception("No media types available");
            }

            var mediaType = _ansiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x => x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }
            Guid selectedMediaTypeId = mediaType.Id;

            await _mediaTypeService.Remove(selectedMediaTypeId);
            _ansiConsole.MarkupLine($"[green]Media Type [[{mediaType.Name}]] was successfully deleted![/]");
        }

        private async Task HandleRemoveWithShowSelect(string? mediaTypeId)
        {

            var mediaTypeIdValidationResult = MediaTypeValidationUtils.ValidateMediaTypeId(mediaTypeId);

            if (mediaTypeIdValidationResult.Successful)
            {
                Guid mediaTypeIdGuid = Guid.Parse(mediaTypeId!);

                await _mediaTypeService.Remove(mediaTypeIdGuid);
                _ansiConsole.MarkupLine($"[green]Media Type with Id [[{mediaTypeIdGuid}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception(mediaTypeIdValidationResult.Message);
            }
        }
    }
}
