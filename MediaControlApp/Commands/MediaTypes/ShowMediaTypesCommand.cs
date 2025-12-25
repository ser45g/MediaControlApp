using MediaControlApp.Application.Services;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.MediaTypes
{
   

    [Description("Show available media types.")]
    public sealed class ShowMediaTypesCommand : AsyncCommand<ShowMediaTypesCommand.Settings>
    {

        private readonly MediaTypeService _mediaTypeService;

        public ShowMediaTypesCommand(MediaTypeService mediaTypeService)
        {
            _mediaTypeService = mediaTypeService;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandOption("--limit <LIMIT>")]
            [Description("How many elements you want to see")]
            public int? Limit { get; set; }

            [CommandOption("--ascending")]
            [Description("Show media types in an ascending order")]
            public bool IsAscending { get; set; }
        }
    

        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            var table = new Table().RoundedBorder();
           
            table.AddColumn("[red]Id[/]");
            table.AddColumn("[green]Name[/]");
            table.ShowRowSeparators();

            IEnumerable<MediaType> mediaTypes = Enumerable.Empty<MediaType>();

            try
            {
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
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }
        
            AnsiConsole.MarkupLine("[red]Media Types[/]");
            if (!mediaTypes.Any())
            {
                table.AddEmptyRow();
            }
            foreach (var el in mediaTypes)
            {
                table.AddRow(el.Id.ToString(), el.Name);
            }

            AnsiConsole.Write(table);
            return 0;
        }
    }
}
