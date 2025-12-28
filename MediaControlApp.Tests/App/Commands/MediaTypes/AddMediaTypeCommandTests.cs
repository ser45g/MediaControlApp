using MediaControlApp.Application.Services;
using MediaControlApp.Commands.MediaTypes;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli.Testing;


namespace MediaControlApp.Tests.App.Commands.MediaTypes
{
    public static class AddMediaTypeCommandTests
    {
        [Fact]
        public static void AddMediaTypeCommand_Execute_ReturnsInt()
        {
            //Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<MediaTypeService>();
            var app = new CommandAppTester();
            app.SetDefaultCommand<AddMediaTypeCommand>();

            // Act
            var result = app.Run("MUSIC");

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("Hello", result.Output);
            Assert.Contains("World", result.Output);

        }
    }
}
