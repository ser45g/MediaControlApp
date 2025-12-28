using MediaControlApp.Application.Services;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.SharedSettings;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.MediaTypes
{
   

    [Description("Show available media types.")]
    public sealed class ShowMediaTypesCommand : AsyncCommand<ShowElementsSettings>
    {

        private readonly IMediaTypeService _mediaTypeService;
        private readonly IAnsiConsole _ansiConsole;


        public ShowMediaTypesCommand(IMediaTypeService mediaTypeService, IAnsiConsole ansiConsole)
        {
            _mediaTypeService = mediaTypeService;
            _ansiConsole = ansiConsole;
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, ShowElementsSettings settings, CancellationToken cancellationToken)
        {
            var table = new Table().RoundedBorder();
           
            table.AddColumn("[red]Id[/]");
            table.AddColumn("[green]Name[/]");
            table.ShowRowSeparators();

            IEnumerable<MediaType> mediaTypes = Enumerable.Empty<MediaType>();

            mediaTypes = await _mediaTypeService.GetAll();
            if (settings.Limit != null)
            {
                mediaTypes = mediaTypes.Take(settings.Limit.Value);
            }

            if (settings.IsAscending) { 
                mediaTypes = mediaTypes.OrderBy(x => x.Name);
            }
            else
            {
                mediaTypes = mediaTypes.OrderByDescending(x => x.Name);
            }  
           
            _ansiConsole.MarkupLine("[red]Media Types[/]");
            if (!mediaTypes.Any())
            {
                table.AddEmptyRow();
            }
            foreach (var el in mediaTypes)
            {
                table.AddRow(el.Id.ToString(), el.Name);
            }

            _ansiConsole.Write(table);
            return 0;
        }
    }
}
