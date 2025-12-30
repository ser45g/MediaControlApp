using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.SharedSettings;
using MediaControlApp.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.Ganres
{
    [Description("Update a ganre")]
    public class UpdateGanreCommand : AsyncCommand<UpdateGanreCommand.Settings>
    {
        private readonly IGanreService _ganreService;
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IGanreValidationUtils _ganreValidationUtils;


        public UpdateGanreCommand(IGanreService ganreService, IMediaTypeService mediaTypeService, IAnsiConsole ansiConsole, IGanreValidationUtils ganreValidationUtils)
        {
            _ganreService = ganreService;
            _mediaTypeService = mediaTypeService;
            _ansiConsole = ansiConsole;
            _ganreValidationUtils = ganreValidationUtils;
        }

        public sealed class Settings : SelectableSettings
        {
            [CommandArgument(0, "[GANREID]")]
            [Description("The ganre's id to delete.")]
            public string? GanreId { get; init; }

            [CommandArgument(1, "[GANRENAME]")]
            [Description("The ganre name to add. It must be unique")]
            public string? Name { get; init; }

            [CommandArgument(2, "[DESCRIPTION]")]
            [Description("The description of the specified ganre")]
            public string? Description { get; init; }

            [CommandArgument(3, "[MEDIATYPEID]")]
            [Description("The ganre's media type id")]
            public string? MediaTypeId { get; init; }

        }

        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (!settings.ShowSelect)
            {
                var ganreIdValidationResult = _ganreValidationUtils.ValidateGanreId(settings.GanreId);

                var ganreNameValidationTask = _ganreValidationUtils.ValidateName(settings.Name);
                ganreNameValidationTask.Wait();

                var mediaTypeIdValidationTask = _ganreValidationUtils.ValidateMediaTypeId(settings.MediaTypeId);


                var ganreNameValidationResult = ganreNameValidationTask.Result;

                if (!ganreIdValidationResult.Successful)
                    return ganreIdValidationResult;

                if (!mediaTypeIdValidationTask.Successful)
                    return mediaTypeIdValidationTask;

                if (!ganreNameValidationResult.Successful)
                    return ganreNameValidationResult;
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
                await HandleUpdateWithShowSelect(settings.GanreId, settings.Name, settings.MediaTypeId, settings.Description);
            }

            return 0;
        }


        private async Task HandleUpdateWithShowSelect(string ganreId, string ganreName, string mediaTypeId, string? description)
        {
            Guid ganreIdGuid = Guid.Parse(ganreId);
            Guid mediaTypeIdGuid = Guid.Parse(mediaTypeId);

            await _ganreService.Update(ganreIdGuid, ganreName, mediaTypeIdGuid, description);
            _ansiConsole.MarkupLine($"[green]Ganre with Id [[{ganreIdGuid}]] was successfully updated![/]");
        }

        private async Task HandleUpdate()
        {
            var ganres = await _ganreService.GetAll();

            var mediaTypes = await _mediaTypeService.GetAll();

            if (!ganres.Any())
            {
                throw new Exception("No ganres available");
            }

            if (!mediaTypes.Any())
            {
                throw new Exception("No media types available");
            }

            var ganre = _ansiConsole.Prompt(new SelectionPrompt<Ganre>().Title("Please select the ganre you want to update").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(ganres).UseConverter(x => x.Name));

            if (ganre == null)
            {
                throw new ArgumentNullException(nameof(ganre));
            }

            var newName = _ansiConsole.Prompt(new TextPrompt<string>("Enter a new name: ").DefaultValue(ganre.Name).Validate(x =>
            {
                var ganreNameValidationTask = _ganreValidationUtils.ValidateName(x);
                ganreNameValidationTask.Wait();

                var ganreNameValidationResult = ganreNameValidationTask.Result;

                return ganreNameValidationResult.Successful;
            }));

            var newDescription = _ansiConsole.Prompt(new TextPrompt<string?>("Enter a new description: ").DefaultValue(ganre.Description));

            var mediaType = _ansiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x => x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }
            Guid selectedMediaTypeId = mediaType.Id;

            await _ganreService.Update(ganre.Id, newName, selectedMediaTypeId, newDescription);
            _ansiConsole.MarkupLine($"[green]Ganre was successfully updated![/]");
        }
    }
}
