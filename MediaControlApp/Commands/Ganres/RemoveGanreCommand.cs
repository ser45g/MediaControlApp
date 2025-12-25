using MediaControlApp.Application.Services;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.SharedSettings;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.Ganres
{
    [Description("Remove a ganre")]
    public class RemoveGanreCommand : AsyncCommand<RemoveGanreCommand.Settings>
    {

        private readonly GanreService _ganreService;

        public RemoveGanreCommand(GanreService ganreService)
        {
            _ganreService = ganreService;
        }

        public sealed class Settings : SelectableSettings
        {

            [CommandArgument(0, "[GANREID]")]
            [Description("The ganre's id to delete.")]
            public string? GanreId { get; init; }

        }


        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            try
            {
                if (settings.ShowSelect)
                {
                    await HandleRemove();
                }
                else
                {
                    await HandleRemoveWithShowSelect(settings.GanreId);
                }

            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }

            return 0;
        }

        private async Task HandleRemove()
        {
            var ganres = await _ganreService.GetAll();

            if (!ganres.Any())
            {
                throw new Exception("No ganres available");
            }
            var ganre = AnsiConsole.Prompt(new SelectionPrompt<Ganre>().Title("Please select the ganre you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(ganres).UseConverter(x => $"{x.Name}"));

            if (ganre == null)
            {
                throw new ArgumentNullException(nameof(ganre));
            }
            Guid selectedGanreId = ganre.Id;

            await _ganreService.Remove(selectedGanreId);
            AnsiConsole.MarkupLine($"[green]Ganre [[{ganre.Name}]] was successfully deleted![/]");
        }

        private async Task HandleRemoveWithShowSelect(string? ganreId)
        {

            var ganreIdValidationResult = GanreValidationUtils.ValidateGanreId(ganreId);

            if (ganreIdValidationResult.Successful)
            {
                Guid ganreIdGuid = Guid.Parse(ganreId!);

                await _ganreService.Remove(ganreIdGuid);
                AnsiConsole.MarkupLine($"[green]Ganre with Id [[{ganreId}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception(ganreIdValidationResult.Message);
            }
        }
    }
}
