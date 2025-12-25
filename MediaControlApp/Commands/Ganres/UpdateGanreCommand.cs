using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.Ganres
{
    [Description("Update a ganre")]
    public class UpdateGanreCommand : AsyncCommand<UpdateGanreCommand.Settings>
    {
        private readonly GanreService _ganreService;
        private readonly MediaTypeService _mediaTypeService;

        public UpdateGanreCommand(GanreService ganreService, MediaTypeService mediaTypeService)
        {
            _ganreService = ganreService;
            _mediaTypeService = mediaTypeService;
        }

        public sealed class Settings : GanreSettings
        {
            [CommandArgument(0, "[GANREID]")]
            [Description("The ganre's id to delete.")]
            public string? GanreId { get; init; }

        }

        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (!settings.ShowSelect)
            {
                var ganreIdValidationResult = GanreValidationUtils.ValidateGanreId(settings.GanreId);

                var ganreNameValidationTask = GanreValidationUtils.ValidateName(_ganreService, settings.Name);
                ganreNameValidationTask.Wait();

                var mediaTypeIdValidationTask = GanreValidationUtils.ValidateMediaTypeId(settings.MediaTypeId);


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
            AnsiConsole.MarkupLine($"[green]Ganre with Id [[{ganreIdGuid}]] was successfully updated![/]");
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

            var ganre = AnsiConsole.Prompt(new SelectionPrompt<Ganre>().Title("Please select the ganre you want to update").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(ganres).UseConverter(x => x.Name));

            if (ganre == null)
            {
                throw new ArgumentNullException(nameof(ganre));
            }

            var newName = AnsiConsole.Prompt(new TextPrompt<string>("Enter a new name: ").DefaultValue(ganre.Name).Validate(x =>
            {
                var ganreNameValidationTask = GanreValidationUtils.ValidateName(_ganreService, x);
                ganreNameValidationTask.Wait();

                var ganreNameValidationResult = ganreNameValidationTask.Result;

                return ganreNameValidationResult.Successful;
            }));

            var newDescription = AnsiConsole.Prompt(new TextPrompt<string?>("Enter a new description: ").DefaultValue(ganre.Description));

            var mediaType = AnsiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x => x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }
            Guid selectedMediaTypeId = mediaType.Id;

            await _ganreService.Update(ganre.Id, newName, selectedMediaTypeId, newDescription);
            AnsiConsole.MarkupLine($"[green]Ganre was successfully updated![/]");
        }


    }
}
