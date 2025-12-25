using MediaControlApp.Application.Services;
using MediaControlApp.SharedSettings;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.Medias
{
    [Description("Show available medias")]
    public class ShowMediasCommand : AsyncCommand<ShowElementsSettings>
    {

        private readonly MediaService _mediaService;

        public ShowMediasCommand(MediaService mediaService)
        {
            _mediaService = mediaService;
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, ShowElementsSettings settings, CancellationToken cancellationToken)
        {
            var table = new Table().RoundedBorder();

            table.AddColumn("[red]Id[/]");
            table.AddColumn("[green]Title[/]");
            table.AddColumn("[green]Description[/]");
            table.AddColumn("[green]Ganre Id[/]");
            table.AddColumn("[green]Ganre Name[/]");
            table.AddColumn("[green]Author Id[/]");
            table.AddColumn("[green]Author Name[/]");
            table.AddColumn("[green]Published[/]");
            table.AddColumn("[green]Last consumed[/]");

            table.ShowRowSeparators();

            var medias = await _mediaService.GetAll();
            if (settings.Limit != null)
            {
                medias = medias.Take(settings.Limit.Value);
            }

            if (settings.IsAscending)
            {
                medias = medias.OrderBy(x => x.Title);
            }
            else
            {
                medias = medias.OrderByDescending(x => x.Title);
            }


            AnsiConsole.MarkupLine("[red]Medias[/]");
            if (!medias.Any())
            {
                table.AddEmptyRow();
            }
            foreach (var el in medias)
            {
                table.AddRow(el.Id.ToString(), el.Title, el.Description ?? " - ", el.GanreId.ToString(), el.Ganre?.Name ?? " - ", el.AuthorId.ToString(), el.Author?.Name ?? " - ", el.PublisedDateUtc.ToShortDateString(), el.LastConsumedDateUtc?.ToShortDateString() ?? " - ");
            }

            AnsiConsole.Write(table);
            return 0;
        }

    }
}
