using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Helpers;
using MediaControlApp.Validators;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli.Testing;
using Spectre.Console.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Tests.App.Commands.MediaTypes
{
    public class UpdateMediaTypeCommandTests
    {
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IMediaTypeValidationUtils _mediaTypeValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public UpdateMediaTypeCommandTests()
        {
            _mediaTypeService = A.Fake<IMediaTypeService>();
            _mediaTypeValidationUtils = A.Fake<IMediaTypeValidationUtils>();
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IMediaTypeService>(s => _mediaTypeService);

            _serviceCollection.AddTransient<IMediaTypeValidationUtils>(s => _mediaTypeValidationUtils);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }
        [Fact]
        public void UpdateMediaTypeCommand_Execute_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _mediaTypeService.Update(A<Guid>._ ,A<string>._)).Returns(true);

            A.CallTo(() => _mediaTypeValidationUtils.ValidateName(A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _mediaTypeValidationUtils.ValidateMediaTypeId(A<string?>._)).Returns(ValidationResult.Success());

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            string guidString = Guid.NewGuid().ToString();

            app.Configure(builder =>
            {
                builder.AddCommand<UpdateMediaTypeCommand>("media-type update");
            });
            app.SetDefaultCommand<UpdateMediaTypeCommand>();

            // Act
            var result = app.Run(guidString, "MMusic");

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Equal($"Media Type with Id [{guidString}] was successfully updated!", result.Output);

        }


        [Fact]
        public void UpdateMediaTypeCommandTests_Execute_Interactive_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _mediaTypeService.Update(A<Guid>._, A<string>._)).Returns(true);
            A.CallTo(() => _mediaTypeValidationUtils.ValidateName(A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _mediaTypeValidationUtils.ValidateMediaTypeId(A<string?>._)).Returns(ValidationResult.Success());
            var mediaTypes = new MediaType[]
            {
                new MediaType(){Id=Guid.NewGuid(), Name="music"},
                new MediaType(){Id=Guid.NewGuid(), Name="books"},
                new MediaType(){Id=Guid.NewGuid(), Name="anime"}
            };
            A.CallTo(() => _mediaTypeService.GetAll()).Returns(mediaTypes);


            var registrar = new DITypeRegistar(_serviceCollection);
            var console = new TestConsole();
            console.Profile.Capabilities.Interactive = true;
            console.Input.PushKey(ConsoleKey.Enter);

            console.Input.PushTextWithEnter("NEW_MUSIC");

            var app = new CommandAppTester(registrar, console:console);

            app.Configure(builder =>
            {
                builder.AddCommand<UpdateMediaTypeCommand>("media-type update");
            });
            app.SetDefaultCommand<UpdateMediaTypeCommand>();

            // Act
            var result = app.Run("-s");

            // Assert
            Assert.Equal(0, result.ExitCode);

            Assert.Contains($"Media Type was successfully updated!", result.Output);

        }
    }
}
