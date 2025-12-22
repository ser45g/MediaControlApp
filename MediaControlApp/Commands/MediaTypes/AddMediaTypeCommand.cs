using MediaControlApp.Application.Services;
using MediaControlApp.Application.Services.Interfaces;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Threading.Tasks;


namespace MediaControlApp.Commands.MediaTypes
{
    [Description("Add a media type.")]
    public sealed class AddMediaTypeCommand : AsyncCommand<AddMediaTypeCommand.Settings>
    {
        private readonly MediaTypeService _mediaTypeService;

        public AddMediaTypeCommand(MediaTypeService mediaTypeService)
        {
            _mediaTypeService = mediaTypeService;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<MEDIATYPENAME>")]
            [Description("The media type to add. It must be unique")]
            public required string MediaTypeName { get; set; }
           
        }

        protected override  ValidationResult Validate(CommandContext context, Settings settings)
        {
            var mediaTypeNameValidationTask = MediaTypeValidationUtils.ValidateName(_mediaTypeService, settings.MediaTypeName);

            mediaTypeNameValidationTask.Wait();

            var mediaTypeNameValidationResult = mediaTypeNameValidationTask.Result;

            if (!mediaTypeNameValidationResult.Successful)
                return mediaTypeNameValidationResult;

            return base.Validate(context, settings);
        }
  
        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            try
            {
                await _mediaTypeService.Add(settings.MediaTypeName!.ToUpper());
                AnsiConsole.MarkupLine($"[green]Media Type was successfully added![/]");
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
