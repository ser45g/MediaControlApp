using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Commands.Add;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Infrastructure.DataAccess.MediaStore;
using MediaControlApp.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace MediaControlApp.Commands.MediaTypes
{
   

    [Description("Show available media types.")]
    public sealed class ShowMediaTypesCommand : AsyncCommand<ShowMediaTypesCommand.Settings>
    {

        private readonly IMediaTypeRepo _mediaTypeRepo;

        public ShowMediaTypesCommand(IMediaTypeRepo mediaTypeRepo)
        {
            _mediaTypeRepo = mediaTypeRepo;
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            var table = new Table().RoundedBorder();
           
         
            table.AddColumn("[red]№[/]");
            table.AddColumn("[green]Name[/]");
            table.ShowRowSeparators();

            IEnumerable<MediaType> mediaTypes = Enumerable.Empty<MediaType>();

            try
            {
                mediaTypes = await _mediaTypeRepo.GetAll();
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
                return await Task.FromResult(-1);
            }
         
            int count = 1;
            AnsiConsole.MarkupLine("[red]Media Types[/]");
            foreach (var el in mediaTypes)
            {
                table.AddRow((count++).ToString(), el.Name);
            }

            AnsiConsole.Write(table);
            return 0;
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

        
    }
}
