using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;


namespace MediaControlApp.Commands.MediaTypes
{
   

    [Description("Show available authors.")]
    public sealed class ShowAuthorsCommand : AsyncCommand<ShowAuthorsCommand.Settings>
    {

        private readonly AuthorService _authorService;

        public ShowAuthorsCommand(AuthorService authorService)
        {
            _authorService = authorService;
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            var table = new Table().RoundedBorder(); 
         
            table.AddColumn("[red]Id[/]");
            table.AddColumn("[green]Name[/]");
            table.AddColumn("[green]Company Name[/]");
            table.AddColumn("[green]Email[/]");
            table.ShowRowSeparators();

            IEnumerable<Author> authors = Enumerable.Empty<Author>();

            try
            {
                authors = await _authorService.GetAll();
                if (settings.Limit != null)
                {
                    authors = authors.Take(settings.Limit.Value);
                }

                if (settings.IsAscending) { 
                    authors = authors.OrderBy(x => x.Name);
                }
                else
                {
                    authors = authors.OrderByDescending(x => x.Name);
                }
            }

            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }
         
            AnsiConsole.MarkupLine("[red]Authors[/]");

            if (!authors.Any())
            {
                table.AddEmptyRow();
            }

            foreach (var el in authors)
            {
                table.AddRow(el.Id.ToString(), el.Name, el.CompanyName ?? " - ", el.Email ?? " - ");
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
            [Description("Show authors by Name in an ascending order")]
            public bool IsAscending { get; set; }
        }
    }
}
