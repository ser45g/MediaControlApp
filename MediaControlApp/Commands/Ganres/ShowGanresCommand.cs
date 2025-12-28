using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.SharedSettings;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.Ganres
{
    [Description("Show available ganres")]
    public class ShowGanresCommand : AsyncCommand<ShowElementsSettings>
    {
        private readonly GanreService _ganreService;
        private readonly IAnsiConsole _ansiConsole;


        public ShowGanresCommand(GanreService ganreService, IAnsiConsole ansiConsole)
        {
            _ganreService = ganreService;
            _ansiConsole = ansiConsole;
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, ShowElementsSettings settings, CancellationToken cancellationToken)
        {
            var table = new Table().RoundedBorder();

            table.AddColumn("[red]Id[/]");
            table.AddColumn("[green]Name[/]");
            table.AddColumn("[green]Description[/]");
            table.AddColumn("[green]Media Type Id[/]");
            table.AddColumn("[green]Media Type Name[/]");
            table.ShowRowSeparators();

            var ganres = await _ganreService.GetAll();

            if (settings.Limit != null)
            {
                ganres = ganres.Take(settings.Limit.Value);
            }

            if (settings.IsAscending)
            {
                ganres = ganres.OrderBy(x => x.Name);
            }
            else
            {
                ganres = ganres.OrderByDescending(x => x.Name);
            }

            _ansiConsole.MarkupLine("[red]Ganres[/]");

            if (!ganres.Any())
            {
                table.AddEmptyRow();
            }
            foreach (var el in ganres)
            {
                table.AddRow(el.Id.ToString(), el.Name, el.Description ?? " - ", el.MediaTypeId.ToString(), el.MediaType?.Name ?? " - ");
            }

            _ansiConsole.Write(table);
            return 0;
        }


    }
}
