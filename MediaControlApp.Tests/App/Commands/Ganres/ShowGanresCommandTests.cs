using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Ganres;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Helpers;
using MediaControlApp.Validators;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli.Testing;

namespace MediaControlApp.Tests.App.Commands.Ganres
{
    public class ShowGanresCommandTests
    {
        private readonly IGanreService _ganreService;

        private readonly IServiceCollection _serviceCollection;

        public ShowGanresCommandTests()
        {
            _ganreService = A.Fake<IGanreService>();

            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IGanreService>(s => _ganreService);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }
        [Fact]
        public void ShowMediaTypesCommand_Execute_ReturnsInt()
        {
            //Arrange
            var ganres = new Ganre[]
            {
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=Guid.NewGuid(), Description="Hello", Name="Kirk"},
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=Guid.NewGuid(), Description=null, Name="James hammet"},
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=Guid.NewGuid(), Description="", Name="Ozzy Ossborn"}
            };
            A.CallTo(() => _ganreService.GetAll()).Returns(ganres);

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<ShowGanresCommand>("ganres show");
            });
            app.SetDefaultCommand<ShowGanresCommand>();

            // Act
            var result = app.Run();

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Equal(result.Output.Split('\n').Length, 10);

        }
    }
}
