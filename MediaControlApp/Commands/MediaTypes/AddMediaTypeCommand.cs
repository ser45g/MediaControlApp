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
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IAnsiConsole _ansiConsole;


        public AddMediaTypeCommand(MediaTypeService mediaTypeService, IAnsiConsole ansiConsole)
        {
            _mediaTypeService = mediaTypeService;
            _ansiConsole = ansiConsole;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<MEDIATYPENAME>")]
            [Description("The media type to add. Its name must be unique")]
            public required string Name { get; init; }
           
        }

        protected override  ValidationResult Validate(CommandContext context, Settings settings)
        {
            var mediaTypeNameValidationTask = MediaTypeValidationUtils.ValidateName(_mediaTypeService, settings.Name);

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
