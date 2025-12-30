using MediaControlApp.Application.Services;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Threading.Tasks;


namespace MediaControlApp.Commands.MediaTypes
{
    [Description("Add a media type.")]
    public sealed class AddMediaTypeCommand : AsyncCommand<AddMediaTypeCommand.Settings>
    {
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IMediaTypeValidationUtils _mediaTypeValidationUtils;

        public AddMediaTypeCommand(IMediaTypeService mediaTypeService, IAnsiConsole ansiConsole, IMediaTypeValidationUtils mediaTypeValidationUtils)
        {
            _mediaTypeService = mediaTypeService;
            _ansiConsole = ansiConsole;
            _mediaTypeValidationUtils = mediaTypeValidationUtils;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<MEDIATYPENAME>")]
            [Description("The media type to add. Its name must be unique")]
            public required string Name { get; init; }
           
        }

        protected override  ValidationResult Validate(CommandContext context, Settings settings)
        {
            var mediaTypeNameValidationTask = _mediaTypeValidationUtils.ValidateName(settings.Name);

            mediaTypeNameValidationTask.Wait();

            if (!mediaTypeNameValidationTask.Result.Successful)
                return mediaTypeNameValidationTask.Result;

            return base.Validate(context, settings);
        }
  
        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
          
            await _mediaTypeService.Add(settings.Name!.ToUpper());
            _ansiConsole.MarkupLine($"[green]Media Type was successfully added![/]");
           
            return 0;
        }
    }
}
