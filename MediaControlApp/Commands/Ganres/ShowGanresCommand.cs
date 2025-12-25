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
    [Description("Show available ganres")]
    public class ShowGanresCommand : AsyncCommand<ShowGanresCommand.Settings>
    {
        private readonly GanreService _ganreService;

        public ShowGanresCommand(GanreService ganreService)
        {
            _ganreService = ganreService;
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            var table = new Table().RoundedBorder();

            table.AddColumn("[red]Id[/]");
            table.AddColumn("[green]Name[/]");
            table.AddColumn("[green]Description[/]");
            table.AddColumn("[green]Media Type Id[/]");
            table.AddColumn("[green]Media Type Name[/]");
            table.ShowRowSeparators();

            IEnumerable<Ganre> ganres = Enumerable.Empty<Ganre>();

            try
            {
                ganres = await _ganreService.GetAll();

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


            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }


            AnsiConsole.MarkupLine("[red]Ganres[/]");

            if (!ganres.Any())
            {
                table.AddEmptyRow();
            }
            foreach (var el in ganres)
            {
                table.AddRow(el.Id.ToString(), el.Name, el.Description ?? " - ",el.MediaTypeId.ToString(), el.MediaType?.Name ?? " - ");
            }

            AnsiConsole.Write(table);
            return 0;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandOption("--limit <LIMIT>")]
            [Description("How many elements you want to see")]
            public int? Limit { get; init; }

            [CommandOption("--ascending")]
            [Description("Show authors by Name in an ascending order")]
            public bool IsAscending { get; init; }
        }


    }
}
