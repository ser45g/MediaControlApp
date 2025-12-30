using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Validators;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using MediaControlApp.Helpers;
using Spectre.Console.Cli.Testing;


namespace MediaControlApp.Tests.App.Commands.MediaTypes
{
    public  class AddMediaTypeCommandTests
    {

        private readonly IMediaTypeService _mediaTypeService;
        private readonly IMediaTypeValidationUtils _mediaTypeValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public AddMediaTypeCommandTests()
        {
            _mediaTypeService = A.Fake<IMediaTypeService>();

            _mediaTypeValidationUtils = A.Fake<IMediaTypeValidationUtils>();
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IMediaTypeService>(s=>_mediaTypeService);

            _serviceCollection.AddTransient<IMediaTypeValidationUtils>(s=>_mediaTypeValidationUtils);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }
        [Fact]
        public void AddMediaTypeCommand_Execute_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _mediaTypeService.Add(A<string>._)).Returns(true);

            A.CallTo(() => _mediaTypeValidationUtils.ValidateName(A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _mediaTypeValidationUtils.ValidateMediaTypeId(A<string?>._)).Returns(ValidationResult.Success());

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<AddMediaTypeCommand>("media-type add");
            });
            app.SetDefaultCommand<AddMediaTypeCommand>();

            // Act
            var result = app.Run("MUSIC");

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Equal($"Media Type was successfully added!", result.Output);

        }
    }
}
