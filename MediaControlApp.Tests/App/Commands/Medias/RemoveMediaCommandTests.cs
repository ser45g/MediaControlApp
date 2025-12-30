using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Ganres;
using MediaControlApp.Commands.Medias;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using MediaControlApp.Helpers;
using MediaControlApp.Validators;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli.Testing;
using Spectre.Console.Testing;

namespace MediaControlApp.Tests.App.Commands.Medias
{
    public class RemoveMediaCommandTests
    {
        private readonly IMediaService _mediaService;
        private readonly IMediaValidationUtils _mediaValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public RemoveMediaCommandTests()
        {
            _mediaService = A.Fake<IMediaService>();
            _mediaValidationUtils= A.Fake<IMediaValidationUtils>();

            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddTransient<IMediaValidationUtils>(s => _mediaValidationUtils);
            _serviceCollection.AddScoped<IMediaService>(s => _mediaService);
        }

        [Fact]
        public void RemoveMediaCommandTests_Execute_ReturnsInt()
        {
            string? guidString = "4765d892-4211-4e3c-a022-86249eae09cc";
            //Arrange
            A.CallTo(() => _mediaService.Remove(A<Guid>._)).Returns(true);
            A.CallTo(() => _mediaValidationUtils.ValidateMediaId(A<string?>._)).Returns(ValidationResult.Success());
            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<RemoveMediaCommand>("media remove");
            });
            app.SetDefaultCommand<RemoveMediaCommand>();

            // Act
            var result = app.Run(guidString);

            // Assert
            Assert.Equal(0, result.ExitCode);
            //Assert.Equal("", result.Output);
        }


        [Fact]
        public void RemoveAuthorCommandTests_Execute_Interactive_ReturnsInt()
        {

            //Arrange
            A.CallTo(() => _mediaService.Remove(A<Guid>._)).Returns(true);

            A.CallTo(() => _mediaValidationUtils.ValidateMediaId(A<string?>._)).Returns(ValidationResult.Success());

            var medias = new Media[]
             {
                new Media(){Title="Just a Title 1", Id=Guid.NewGuid(),GanreId=Guid.NewGuid(), PublishedDateUtc=DateTime.Now.AddYears(-3), Rating=new Rating(8),AuthorId=Guid.NewGuid(), Description="Hello"},
                new Media(){Title="Just a Title 2", Id=Guid.NewGuid(),GanreId=Guid.NewGuid(), PublishedDateUtc=DateTime.Now.AddYears(-3), Rating=new Rating(8),AuthorId=Guid.NewGuid(), Description=null},
                new Media(){Title="Just a Title 3", Id=Guid.NewGuid(),GanreId=Guid.NewGuid(), PublishedDateUtc=DateTime.Now.AddYears(-3), Rating=new Rating(8),AuthorId=Guid.NewGuid(), Description=""}
             };
            A.CallTo(() => _mediaService.GetAll()).Returns(medias);

            var registrar = new DITypeRegistar(_serviceCollection);
            var console = new TestConsole();
            console.Profile.Capabilities.Interactive = true;
            console.Input.PushKey(ConsoleKey.Enter);

            var app = new CommandAppTester(registrar, console: console);

            app.Configure(builder =>
            {
                builder.AddCommand<RemoveMediaCommand>("media remove");
            });
            app.SetDefaultCommand<RemoveMediaCommand>();

            // Act
            var result = app.Run("-s");

            // Assert
            Assert.Equal(0, result.ExitCode);
            // Assert.Contains($"Media Type [{mediaTypes[0].Name}] was successfully deleted!", result.Output);

        }
    }
}
