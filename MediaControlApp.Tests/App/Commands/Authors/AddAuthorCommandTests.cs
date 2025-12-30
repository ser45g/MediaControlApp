using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Helpers;
using MediaControlApp.Validators;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli.Testing;

namespace MediaControlApp.Tests.App.Commands.Authors
{
    public class AddAuthorCommandTests
    {

        private readonly IAuthorService _authorService;
        private readonly IAuthorValidationUtils _authorValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public AddAuthorCommandTests()
        {
            _authorService = A.Fake<IAuthorService>();
            _authorValidationUtils = A.Fake<IAuthorValidationUtils>();
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IAuthorService>(s => _authorService);

            _serviceCollection.AddTransient<IAuthorValidationUtils>(s => _authorValidationUtils);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }
        [Fact]
        public void AddAuthorCommand_Execute_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _authorService.Add(A<string>._)).Returns(true);

            A.CallTo(() => _authorValidationUtils.ValidateName(A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _authorValidationUtils.ValidateAuthorId(A<string?>._)).Returns(ValidationResult.Success());

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<AddAuthorCommand>("author add");
            });
            app.SetDefaultCommand<AddAuthorCommand>();

            // Act
            var result = app.Run("James Hatfield", "Metallica", "metallica@google.com");

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains($"Author was successfully added!", result.Output);

        }
    }
}
