using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Commands.Statistics;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Helpers;
using MediaControlApp.Infrastructure.DataAccess.MediaStore.Utility;
using MediaControlApp.Validators;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Tests.App.Commands.Statistics
{
    public class ShowChartOfElementsCommandTests
    {
        private readonly IUtilityDataRepo _utilityDataRepo;

        private readonly IServiceCollection _serviceCollection;

        public ShowChartOfElementsCommandTests()
        {
            _utilityDataRepo = A.Fake<IUtilityDataRepo>();

            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IUtilityDataRepo>(s => _utilityDataRepo);

        }
        [Fact]
        public void ShowChartOfElementsCommand_Execute_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _utilityDataRepo.GetAmountsOfElements()).Returns(new AmountsOfElements(5,10,7,4));

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<ShowChartOfElementsCommand>("statistics show");
            });
            app.SetDefaultCommand<ShowChartOfElementsCommand>();

            // Act
            var result = app.Run("--bar-chart");

            // Assert
            //Assert.Equal(0, result.ExitCode);
            //Assert.Equal(result.Output.Split('\n').Length, 10);

        }
    }
}
