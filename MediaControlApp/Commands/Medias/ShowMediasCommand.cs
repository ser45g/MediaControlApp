using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Ganres;
using MediaControlApp.Domain.Models.Media;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.Medias
{
    [Description("Show available medias")]
    public class ShowMediasCommand : AsyncCommand<ShowMediasCommand.Settings>
    {

        private readonly MediaService _mediaService;

        public ShowMediasCommand(MediaService mediaService)
        {
            _mediaService = mediaService;
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
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

            IEnumerable<Media> medias = Enumerable.Empty<Media>();

            try
            {
                medias = await _mediaService.GetAll();
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
            }

            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }

            AnsiConsole.MarkupLine("[red]Medias[/]");
            if (!medias.Any())
            {
                table.AddEmptyRow();
            }
            foreach (var el in medias)
            {
                table.AddRow(el.Id.ToString(), el.Title, el.Description ?? " - ", el.GanreId.ToString(), el.Ganre?.Name ?? " - ", el.AuthorId.ToString(), el.Author?.Name?? " - ", el.PublisedDateUtc.ToShortDateString(), el.LastConsumedDateUtc?.ToShortDateString()??" - ");
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
