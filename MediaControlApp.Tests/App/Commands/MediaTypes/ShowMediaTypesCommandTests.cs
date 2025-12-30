using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Helpers;
using MediaControlApp.Validators;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli.Testing;

namespace MediaControlApp.Tests.App.Commands.MediaTypes
{
    public class ShowMediaTypesCommandTests
    {

        private readonly IMediaTypeService _mediaTypeService;

        private readonly IServiceCollection _serviceCollection;

        public ShowMediaTypesCommandTests()
        {
            _mediaTypeService = A.Fake<IMediaTypeService>();

            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IMediaTypeService>(s => _mediaTypeService);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }
        [Fact]
        public void ShowMediaTypesCommand_Execute_ReturnsInt()
        {
            //Arrange
            var mediaTypes = new MediaType[]
            {
                new MediaType(){Id=Guid.NewGuid(), Name="music"},
                new MediaType(){Id=Guid.NewGuid(), Name="books"},
                new MediaType(){Id=Guid.NewGuid(), Name="anime"}
            };
            A.CallTo(() => _mediaTypeService.GetAll()).Returns(mediaTypes);

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<ShowMediaTypesCommand>("media-type show");
            });
            app.SetDefaultCommand<ShowMediaTypesCommand>();

            // Act
            var result = app.Run();

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Equal(result.Output.Split('\n').Length, 10);

        }
    }
}
