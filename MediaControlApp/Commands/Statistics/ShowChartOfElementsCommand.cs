using MediaControlApp.Infrastructure.DataAccess.MediaStore.Utility;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace MediaControlApp.Commands.Statistics
{
    [Description("Show a chart of elements.")]
    public sealed class ShowChartOfElementsCommand : AsyncCommand<ShowChartOfElementsCommand.Settings>
    {

        private readonly IUtilityDataRepo _utilityDataRepo;
        private readonly IAnsiConsole _ansiConsole;

        public ShowChartOfElementsCommand(IUtilityDataRepo utilityDataRepo, IAnsiConsole ansiConsole)
        {
            _utilityDataRepo = utilityDataRepo;
            _ansiConsole = ansiConsole;
        }
        public class Settings : CommandSettings
        {

            [CommandOption("--bar-chart")]
            [Description("Show data using a bar chart")]
            public bool IsBarChart { get; init; }
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            var stats = await _utilityDataRepo.GetAmountsOfElements();

            if (settings.IsBarChart) {
                var barChart = new BarChart()
                    .AddItem("Medias", stats.MediasAmount, Color.Green)
                    .AddItem("Media Types", stats.MediaTypesAmount, Color.Blue)
                    .AddItem("Ganres", stats.GanresAmount, Color.Yellow)
                    .AddItem("Authors", stats.AuthorsAmount, Color.Red);
                
                _ansiConsole.Write(barChart);
            }
            else
            {
                var breakdownChart = new BreakdownChart()
                    .AddItem("Medias", stats.MediasAmount, Color.Green)
                    .AddItem("Media Types", stats.MediaTypesAmount, Color.Blue)
                    .AddItem("Ganres", stats.GanresAmount, Color.Yellow)
                    .AddItem("Authors", stats.AuthorsAmount, Color.Red);

                _ansiConsole.Write(breakdownChart);
            }

            return 0;
        }
    }
}
