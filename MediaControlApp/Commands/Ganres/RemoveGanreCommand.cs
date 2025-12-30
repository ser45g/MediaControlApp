using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.SharedSettings;
using MediaControlApp.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.Ganres
{
    [Description("Remove a ganre")]
    public class RemoveGanreCommand : AsyncCommand<RemoveGanreCommand.Settings>
    {

        private readonly IGanreService _ganreService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IGanreValidationUtils _ganreValidationUtils;


        public RemoveGanreCommand(IGanreService ganreService, IAnsiConsole ansiConsole, IGanreValidationUtils ganreValidationUtils)
        {
            _ganreService = ganreService;
            _ansiConsole = ansiConsole;
            _ganreValidationUtils = ganreValidationUtils;
        }

        public sealed class Settings : SelectableSettings
        {

            [CommandArgument(0, "[GANREID]")]
            [Description("The ganre's id to delete.")]
            public string? GanreId { get; init; }

        }

        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {

            if (settings.ShowSelect)
            {
                await HandleRemove();
            }
            else
            {
                await HandleRemoveWithShowSelect(settings.GanreId);
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
            var ganre = _ansiConsole.Prompt(new SelectionPrompt<Ganre>().Title("Please select the ganre you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(ganres).UseConverter(x => $"{x.Name}"));

            if (ganre == null)
            {
                throw new ArgumentNullException(nameof(ganre));
            }
            Guid selectedGanreId = ganre.Id;

            await _ganreService.Remove(selectedGanreId);
            _ansiConsole.MarkupLine($"[green]Ganre [[{ganre.Name}]] was successfully deleted![/]");
        }

        private async Task HandleRemoveWithShowSelect(string? ganreId)
        {

            var ganreIdValidationResult = _ganreValidationUtils.ValidateGanreId(ganreId);

            if (ganreIdValidationResult.Successful)
            {
                Guid ganreIdGuid = Guid.Parse(ganreId!);

                await _ganreService.Remove(ganreIdGuid);
                _ansiConsole.MarkupLine($"[green]Ganre with Id [[{ganreId}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception(ganreIdValidationResult.Message);
            }
        }
    }
}
