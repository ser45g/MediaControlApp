using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.SharedSettings;
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


        public RemoveMediaCommand(IMediaService mediaService, IAnsiConsole ansiConsole)
        {
            _mediaService = mediaService;
            _ansiConsole = ansiConsole;
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
                await HandleRemove();
            }
            else
            {
                await HandleRemoveWithShowSelect(settings.MediaId);
            }

            return 0;
        }

        private async Task HandleRemove()
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

            await _mediaService.Remove(selectedMediaId);
            _ansiConsole.MarkupLine($"[green]Media [[{media.Title}]] was successfully deleted![/]");
        }

        private async Task HandleRemoveWithShowSelect(string? mediaId)
        {

            var mediaIdValidationResult = MediaValidationUtils.ValidateMediaId(mediaId);

            if (mediaIdValidationResult.Successful)
            {
                Guid mediaIdGuid = Guid.Parse(mediaId!);

                await _mediaService.Remove(mediaIdGuid);
                _ansiConsole.MarkupLine($"[green]Media with Id [[{mediaId}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception(mediaIdValidationResult.Message);
            }
        }
    }
}
