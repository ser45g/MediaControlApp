using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.SharedSettings;
using MediaControlApp.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.Medias
{
    [Description("Remove a media")]
    public class RemoveMediaCommand : AsyncCommand<RemoveMediaCommand.Settings>
    {

        private readonly IMediaService _mediaService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IMediaValidationUtils _mediaValidationUtils;


        public RemoveMediaCommand(IMediaService mediaService, IAnsiConsole ansiConsole, IMediaValidationUtils mediaValidationUtils)
        {
            _mediaService = mediaService;
            _ansiConsole = ansiConsole;
            _mediaValidationUtils = mediaValidationUtils;
        }

        public sealed class Settings : SelectableSettings
        {

            [CommandArgument(0, "[MEDIAID]")]
            [Description("The media's id to delete.")]
            public string? MediaId { get; init; }

        }

        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
           
            if (settings.ShowSelect)
            {
                await HandleRemove(cancellationToken);
            }
            else
            {
                await HandleRemoveWithShowSelect(settings.MediaId, cancellationToken);
            }

            return 0;
        }

        private async Task HandleRemove(CancellationToken cancellationToken = default)
        {
            var medias = await _mediaService.GetAll();

            if (!medias.Any())
            {
                throw new Exception("No medias available");
            }

            var media = _ansiConsole.Prompt(new SelectionPrompt<Media>().Title("Please select the media you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(medias).UseConverter(x => x.Title));

            if (media == null)
            {
                throw new ArgumentNullException(nameof(media));
            }
            Guid selectedMediaId = media.Id;

            await _mediaService.Remove(selectedMediaId, cancellationToken);
            _ansiConsole.MarkupLine($"[green]Media [[{media.Title}]] was successfully deleted![/]");
        }

        private async Task HandleRemoveWithShowSelect(string? mediaId, CancellationToken cancellationToken = default)
        {

            var mediaIdValidationResult = _mediaValidationUtils.ValidateMediaId(mediaId);

            if (mediaIdValidationResult.Successful)
            {
                Guid mediaIdGuid = Guid.Parse(mediaId!);

                await _mediaService.Remove(mediaIdGuid, cancellationToken);
                _ansiConsole.MarkupLine($"[green]Media with Id [[{mediaId}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception(mediaIdValidationResult.Message);
            }
        }
    }
}
