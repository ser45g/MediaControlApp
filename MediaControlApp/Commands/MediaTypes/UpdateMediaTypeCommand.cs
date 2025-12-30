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

    [Description("Update a media type.")]
    public class UpdateMediaTypeCommand : AsyncCommand<UpdateMediaTypeCommand.Settings>
    {
 
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IMediaTypeValidationUtils _mediaTypeValidationUtils;


        public UpdateMediaTypeCommand(IMediaTypeService mediaTypeService, IAnsiConsole ansiConsole, IMediaTypeValidationUtils mediaTypeValidationUtils)
        {
            _mediaTypeService = mediaTypeService;
            _ansiConsole = ansiConsole;
            _mediaTypeValidationUtils = mediaTypeValidationUtils;
        }

        public sealed class Settings : SelectableSettings
        {
            [CommandArgument(0, "[MEDIATYPEID]")]
            [Description("The media type's id to delete if.")]
            public string? Id { get; init; }

            [CommandArgument(0, "[MEDIATYPENAME]")]
            [Description("The media type name to add. It must be unique")]
            public string? Name { get; init; }
        }
        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (!settings.ShowSelect)
            {
                var mediaTypeIdValidationResult = _mediaTypeValidationUtils.ValidateMediaTypeId(settings.Id);

                var mediaTypeNameValidationTask = _mediaTypeValidationUtils.ValidateName(settings.Name);

                mediaTypeNameValidationTask.Wait();

                var mediaTypeNameValidationResult = mediaTypeNameValidationTask.Result;

                if (!mediaTypeIdValidationResult.Successful)
                    return mediaTypeIdValidationResult;

                if (!mediaTypeNameValidationResult.Successful)
                    return mediaTypeNameValidationResult;
            }

            return base.Validate(context, settings);
        }


        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
           
            if (settings.ShowSelect)
            {
                await HandleUpdate();
            }
            else
            {
                await HandleUpdateWithShowSelect(settings.Id, settings.Name);
            }
            
            return 0;
        }

        private async Task HandleUpdateWithShowSelect(string mediaTypeId, string mediaTypeName)
        {
            Guid mediaTypeIdGuid = Guid.Parse(mediaTypeId);

            await _mediaTypeService.Update(mediaTypeIdGuid, mediaTypeName);
            _ansiConsole.MarkupLine($"[green]Media Type with Id [[{mediaTypeIdGuid}]] was successfully updated![/]");      
        }

        private async Task HandleUpdate()
        {
            var mediaTypes = await _mediaTypeService.GetAll();

            if (!mediaTypes.Any())
            { 
                throw new Exception("No media types available");
            }

            var mediaType = _ansiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type you want to update").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x => x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            var newName = _ansiConsole.Prompt(new TextPrompt<string>("Enter a new name: ").DefaultValue(mediaType.Name).Validate(x =>
            {
                var task = _mediaTypeValidationUtils.ValidateName(x);
                task.Wait();

                return task.Result.Successful;
              
            }));

            await _mediaTypeService.Update(mediaType.Id, newName);
            _ansiConsole.MarkupLine($"[green]Media Type was successfully updated![/]");
        }      
    }
}
