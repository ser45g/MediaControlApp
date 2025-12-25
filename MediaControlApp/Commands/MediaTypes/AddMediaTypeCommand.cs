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
            try
            {
                await _mediaTypeService.Add(settings.Name!.ToUpper());
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
