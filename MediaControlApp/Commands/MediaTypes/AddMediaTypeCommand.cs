using MediaControlApp.Application.Services.Interfaces;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;


namespace MediaControlApp.Commands.MediaTypes
{
   

    [Description("Add a media type.")]
    public sealed class AddMediaTypeCommand : AsyncCommand<AddMediaTypeCommand.Settings>
    {
        private readonly IMediaTypeRepo _mediaTypeRepo;
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<MEDIATYPENAME>")]
            [Description("The media type to add. It must be unique")]
            public required string MediaTypeName { get; set; }
           
        }

        public AddMediaTypeCommand(IMediaTypeRepo mediaTypeRepo)
        {
            _mediaTypeRepo = mediaTypeRepo;
        }

        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.MediaTypeName)) {
                return ValidationResult.Error("Media Name can't be empty");
            }
            //Must be unique
            return base.Validate(context, settings);
        }
  
        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            try
            {
                await _mediaTypeRepo.Add(settings.MediaTypeName!);
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
