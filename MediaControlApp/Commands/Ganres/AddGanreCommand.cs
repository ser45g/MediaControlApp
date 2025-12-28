using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.Ganres
{
    [Description("Add a ganre.")]
    public sealed class AddGanreCommand : AsyncCommand<GanreSettings>
    {
        private readonly GanreService _ganreService;
        private readonly MediaTypeService _mediaTypeService;
        private readonly IAnsiConsole _ansiConsole;


        public AddGanreCommand(GanreService ganreService, MediaTypeService mediaTypeService, IAnsiConsole ansiConsole)
        {
            _ganreService = ganreService;
            _mediaTypeService = mediaTypeService;
            _ansiConsole = ansiConsole;
        }



        protected override ValidationResult Validate(CommandContext context, GanreSettings settings)
        {
            if (!settings.ShowSelect)
            {
                var mediaTypeIdValidationResult = GanreValidationUtils.ValidateMediaTypeId(settings.MediaTypeId);

                var mediaTypeNameValidationTask = GanreValidationUtils.ValidateName(_ganreService, settings.Name);

                mediaTypeNameValidationTask.Wait();

                var mediaTypeNameValidationResult = mediaTypeNameValidationTask.Result;

                if (!mediaTypeIdValidationResult.Successful)
                    return mediaTypeIdValidationResult;

                if (!mediaTypeNameValidationResult.Successful)
                    return mediaTypeNameValidationResult;
            }

            return base.Validate(context, settings);
        }


        protected async override Task<int> ExecuteAsync(CommandContext context, GanreSettings settings, CancellationToken cancellationToken)
        {

            if (settings.ShowSelect)
            {
                await HandleAdd();
            }
            else
            {
                await HandleAddWithShowSelect(settings.Name, settings.MediaTypeId, settings.Description!);
            }

            return 0;
        }

        private async Task HandleAddWithShowSelect(string name, string mediaTypeId, string? description)
        {
            Guid mediaTypeIdGuid = Guid.Parse(mediaTypeId);

            await _ganreService.Add(name, mediaTypeIdGuid, description);
            _ansiConsole.MarkupLine($"[green]Ganre with Id [[{mediaTypeIdGuid}]] was successfully added![/]");
        }

        private async Task HandleAdd()
        {
            var mediaTypes = await _mediaTypeService.GetAll();

            if (!mediaTypes.Any())
            {
                throw new Exception("No media types available");
            }

            var name = _ansiConsole.Prompt(new TextPrompt<string>("Enter a ganre name: ").Validate(x =>
            {
                var task = GanreValidationUtils.ValidateName(_ganreService, x);
                task.Wait();

                return task.Result.Successful;

            }));

            var description = _ansiConsole.Prompt(new TextPrompt<string>("Enter a description: ").AllowEmpty());

            var mediaType = _ansiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type you want to add a ganre to").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x => x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            await _ganreService.Add(name, mediaType.Id, description);
            _ansiConsole.MarkupLine($"[green]Ganre was successfully added![/]");
        }
    }
}
