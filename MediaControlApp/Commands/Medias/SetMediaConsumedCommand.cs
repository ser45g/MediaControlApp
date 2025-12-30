using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.SharedSettings;
using MediaControlApp.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.Medias
{
    public class SetMediaConsumedCommand:AsyncCommand<SetMediaConsumedCommand.Settings>
    {
        private readonly IMediaService _mediaService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IMediaValidationUtils _mediaValidationUtils;


        public SetMediaConsumedCommand(IMediaService mediaService, IAnsiConsole ansiConsole, IMediaValidationUtils mediaValidationUtils)
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

            [CommandArgument(6, "[LASTCONSUMEDDATE]")]
            [Description("The date the specified media was consumed")]
            public string? LastConsumedDateUtc { get; init; }
        }

        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {

            if (settings.ShowSelect)
            {
                await HandleSetConsumed(cancellationToken);
            }
            else
            {
                await HandleSetConsumedWithShowSelect(settings.MediaId, settings.LastConsumedDateUtc, cancellationToken);
            }

            return 0;
        }

        private async Task HandleSetConsumed(CancellationToken cancellationToken = default)
        {
            var medias = await _mediaService.GetAll();

            if (!medias.Any())
            {
                throw new Exception("No medias available");
            }

            var media = _ansiConsole.Prompt(new SelectionPrompt<Media>().Title("Please select the media you want to mark as consumed").PageSize(10).MoreChoicesText("Move up and down to reveal more medias").AddChoices(medias).UseConverter(x => x.Title));

            if (media == null)
            {
                throw new ArgumentNullException(nameof(media));
            }
            Guid selectedMediaId = media.Id;

            var lastConsumedDate = _ansiConsole.Prompt(new TextPrompt<string?>("Enter the last consumed date: ").DefaultValue(null).Validate(x =>
            {
                var lastConsumedDateValidationResult = _mediaValidationUtils.ValidateLastConsumedDate(x);
                return lastConsumedDateValidationResult.Successful;
            }));

            await _mediaService.SetConsumed(selectedMediaId, DateTime.Parse(lastConsumedDate), cancellationToken);
            _ansiConsole.MarkupLine($"[green]Media [[{media.Title}]] was marked as consumed![/]");
        }

        private async Task HandleSetConsumedWithShowSelect(string? mediaId, string? lastConsumedDateUtc, CancellationToken cancellationToken = default)
        {

            var mediaIdValidationResult = _mediaValidationUtils.ValidateMediaId(mediaId);

            if (!mediaIdValidationResult.Successful)
            {
                throw new Exception(mediaIdValidationResult.Message);
            }

            var lastConsumedDateUtcValidationResult = _mediaValidationUtils.ValidateLastConsumedDate(lastConsumedDateUtc);

            if (!lastConsumedDateUtcValidationResult.Successful)
            {
                throw new Exception(lastConsumedDateUtcValidationResult.Message);
            }
            
            Guid mediaIdGuid = Guid.Parse(mediaId!);
            DateTime lastConsumedDateUtcDateTime = DateTime.Parse(lastConsumedDateUtc);

            await _mediaService.SetConsumed(mediaIdGuid, lastConsumedDateUtcDateTime, cancellationToken);
            _ansiConsole.MarkupLine($"[green]Media with Id [[{mediaId}]] was marked as consumed![/]");
        }
    }
}
