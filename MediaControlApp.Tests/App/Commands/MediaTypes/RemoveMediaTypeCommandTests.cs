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
    public class RemoveMediaTypeCommandTests
    {

        private readonly IMediaTypeService _mediaTypeService;
        private readonly IMediaTypeValidationUtils _mediaTypeValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public RemoveMediaTypeCommandTests()
        {
            _mediaTypeService = A.Fake<IMediaTypeService>();
            _mediaTypeValidationUtils = A.Fake<IMediaTypeValidationUtils>();
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IMediaTypeService>(s => _mediaTypeService);

            _serviceCollection.AddTransient<IMediaTypeValidationUtils>(s => _mediaTypeValidationUtils);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }
        [Fact]
        public void RemoveMediaTypeCommandTests_Execute_ReturnsInt()
        {
            string? guidString = "4765d892-4211-4e3c-a022-86249eae09cc";
            //Arrange
            A.CallTo(() => _mediaTypeService.Remove(A<Guid>._)).Returns(true);
     
            A.CallTo(() => _mediaTypeValidationUtils.ValidateMediaTypeId(A<string?>._)).Returns(ValidationResult.Success());

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<RemoveMediaTypeCommand>("media-type remove");
            });
            app.SetDefaultCommand<RemoveMediaTypeCommand>();
          
            // Act
            var result = app.Run(guidString);

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains($"Media Type with Id [{guidString}] was successfully deleted!", result.Output);

        }


        [Fact]
        public void RemoveMediaTypeCommandTests_Execute_Interactive_ReturnsInt()
        {
            
            //Arrange
            A.CallTo(() => _mediaTypeService.Remove(A<Guid>._)).Returns(true);

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

            var app = new CommandAppTester(registrar, console:console);

            app.Configure(builder =>
            {
                builder.AddCommand<RemoveMediaTypeCommand>("media-type remove");
            });
            app.SetDefaultCommand<RemoveMediaTypeCommand>();

            // Act
            var result = app.Run("-s");

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains($"Media Type [{mediaTypes[0].Name}] was successfully deleted!", result.Output);

        }


    }
}
