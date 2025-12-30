using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.SharedSettings;
using MediaControlApp.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.Ganres
{
    [Description("Add a ganre.")]
    public sealed class AddGanreCommand : AsyncCommand<AddGanreCommand.Settings>
    {
        private readonly IGanreService _ganreService;
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IGanreValidationUtils _ganreValidationUtils;



        public AddGanreCommand(IGanreService ganreService, IMediaTypeService mediaTypeService, IAnsiConsole ansiConsole, IGanreValidationUtils ganreValidationUtils)
        {
            _ganreService = ganreService;
            _mediaTypeService = mediaTypeService;
            _ansiConsole = ansiConsole;
            _ganreValidationUtils = ganreValidationUtils;
        }

        public class Settings : SelectableSettings
        {
            [CommandArgument(0, "[GANRENAME]")]
            [Description("The ganre name to add. It must be unique")]
            public string? Name { get; init; }
            [CommandArgument(1, "[DESCRIPTION]")]
            [Description("The description of the specified ganre")]
            public string? Description { get; init; }

            [CommandArgument(2, "[MEDIATYPEID]")]
            [Description("The ganre's media type id")]
            public string? MediaTypeId { get; init; }

         

        }

        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (!settings.ShowSelect)
            {
                var mediaTypeIdValidationResult = _ganreValidationUtils.ValidateMediaTypeId(settings.MediaTypeId);

                var mediaTypeNameValidationTask = _ganreValidationUtils.ValidateName(settings.Name);

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
                await HandleAdd(cancellationToken);
            }
            else
            {
                await HandleAddWithShowSelect(settings.Name, settings.MediaTypeId, settings.Description!, cancellationToken);
            }

            return 0;
        }

        private async Task HandleAddWithShowSelect(string name, string mediaTypeId, string? description, CancellationToken cancellationToken = default)
        {
            Guid mediaTypeIdGuid = Guid.Parse(mediaTypeId);

            await _ganreService.Add(name, mediaTypeIdGuid, description, cancellationToken);
            _ansiConsole.MarkupLine($"[green]Ganre was successfully added![/]");
        }

        private async Task HandleAdd(CancellationToken cancellationToken = default)
        {
            var mediaTypes = await _mediaTypeService.GetAll(cancellationToken);

            if (!mediaTypes.Any())
            {
                throw new Exception("No media types available");
            }

            var name = _ansiConsole.Prompt(new TextPrompt<string>("Enter a ganre name: ").Validate(x =>
            {
                var task = _ganreValidationUtils.ValidateName(x);
                task.Wait();

                return task.Result.Successful;

            }));

            var description = _ansiConsole.Prompt(new TextPrompt<string>("Enter a description: ").AllowEmpty());

            var mediaType = _ansiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type you want to add a ganre to").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x => x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            await _ganreService.Add(name, mediaType.Id, description, cancellationToken);
            _ansiConsole.MarkupLine($"[green]Ganre was successfully added![/]");
        }
    }
}
