using MediaControlApp.Application.Services;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Domain.Models.Media;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.Ganres
{
    [Description("Add a ganre.")]
    public sealed class AddGanreCommand : AsyncCommand<AddGanreCommand.Settings>
    {
        private readonly GanreService _ganreService;
        private readonly MediaTypeService _mediaTypeService;

        public AddGanreCommand(GanreService ganreService, MediaTypeService mediaTypeService)
        {
            _ganreService = ganreService;
            _mediaTypeService = mediaTypeService;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "[GANRENAME]")]
            [Description("The ganre name to add. It must be unique")]
            public string? Name { get; init; }

            [CommandArgument(0, "[MEDIATYPEID]")]
            [Description("The ganre's media type id")]
            public string? MediaTypeId { get; init; }

            [CommandArgument(0, "[DESCRIPTION]")]
            [Description("The description of the specified ganre")]
            public string? Description { get; init; }

            [CommandOption("-s|--show-select")]
            [DefaultValue(false)]
            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool ShowSelect { get; init; }

        }

        protected override ValidationResult Validate(CommandContext context, Settings settings)
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


        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            try
            {
                if (settings.ShowSelect)
                {
                    await HandleAdd();
                }
                else
                {
                    await HandleAddWithShowSelect(settings.Name,settings.MediaTypeId, settings.Description!);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }
            return 0;
        }

        private async Task HandleAddWithShowSelect(string name, string mediaTypeId, string? description)
        {
            Guid mediaTypeIdGuid = Guid.Parse(mediaTypeId);

            await _ganreService.Add(name, mediaTypeIdGuid, description);
            AnsiConsole.MarkupLine($"[green]Ganre with Id [[{mediaTypeIdGuid}]] was successfully added![/]");
        }

        private async Task HandleAdd()
        {
            var mediaTypes = await _mediaTypeService.GetAll();

            if (!mediaTypes.Any())
            {
                throw new Exception("No media types available");
            }

            var name = AnsiConsole.Prompt(new TextPrompt<string>("Enter a ganre name: ").Validate(x =>
            {
                var task = GanreValidationUtils.ValidateName(_ganreService, x);
                task.Wait();

                return task.Result.Successful;

            }));

            var description = AnsiConsole.Prompt(new TextPrompt<string>("Enter a description: ").AllowEmpty());

            var mediaType = AnsiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type you want to add a ganre to").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x => x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            await _ganreService.Add(name, mediaType.Id, description);
            AnsiConsole.MarkupLine($"[green]Ganre was successfully added![/]");
        }
    }
}
