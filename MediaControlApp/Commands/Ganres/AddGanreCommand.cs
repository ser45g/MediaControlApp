using MediaControlApp.Application.Services;
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

        public AddGanreCommand(GanreService ganreService)
        {
            _ganreService = ganreService;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<GANRENAME>")]
            [Description("The ganre name to add. It must be unique")]
            public required string Name { get; set; }

            [CommandArgument(0, "[DESCRIPTION]")]
            [Description("The description of the specified ganre")]
            public string? Description { get; set; }

            [CommandArgument(0, "<MEDIATYPEID>")]
            [Description("The ganre's media type id")]
            public Guid MediaTypeId { get; set; }


        }



        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Name))
            {
                return ValidationResult.Error("Name can't be empty");
            }
            if (settings.MediaTypeId == null)
            {
                bool isValidGuid = Guid.TryParse(settings.MediaTypeId, out Guid res);
                if (!isValidGuid)
                    return ValidationResult.Error("Invalid format for Media Type Id");

            }
            var tf = async () =>
            {
                var res = await _mediaTypeService.GetByName(settings.MediaTypeName.ToUpper());
                return res == null;
            };
            var task2 = tf();

            task2.Wait();

            if (!task2.Result)
            {
                return ValidationResult.Error("Media Name must be unique");
            }
            return base.Validate(context, settings);
        }

        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            try
            {
                await _mediaTypeService.Add(settings.MediaTypeName!.ToUpper());
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
