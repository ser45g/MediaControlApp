using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Ganres;
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

namespace MediaControlApp.Tests.App.Commands.Ganres
{
    public class AddGanreCommandTests
    {
        private readonly IGanreService _ganreService;
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IGanreValidationUtils _ganreValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public AddGanreCommandTests()
        {
            _ganreService = A.Fake<IGanreService>();
            _mediaTypeService = A.Fake<IMediaTypeService>();
            _ganreValidationUtils = A.Fake<IGanreValidationUtils>();
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IGanreService>(s => _ganreService);
            _serviceCollection.AddScoped<IMediaTypeService>(s => _mediaTypeService);

            _serviceCollection.AddTransient<IGanreValidationUtils>(s => _ganreValidationUtils);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }
        [Fact]
        public void AddGanreCommand_Execute_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _ganreService.Add(A<string>._, A<Guid>._, A<string?>._)).Returns(true);

            A.CallTo(() => _ganreValidationUtils.ValidateName(A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _ganreValidationUtils.ValidateGanreId(A<string?>._)).Returns(ValidationResult.Success());
            A.CallTo(() => _ganreValidationUtils.ValidateMediaTypeId(A<string?>._)).Returns(ValidationResult.Success());
            

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            //string ganreIdGuidString = Guid.NewGuid().ToString();
            string mediaTypeIdGuidString = Guid.NewGuid().ToString();

            app.Configure(builder =>
            {
                builder.AddCommand<AddGanreCommand>("ganre add");
            });
            app.SetDefaultCommand<AddGanreCommand>();

            // Act
            var result = app.Run("Rock", "Hello", mediaTypeIdGuidString);

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains($"Ganre was successfully added!", result.Output);

        }

        [Fact]
        public void AddGanreCommandTests_Execute_Interactive_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _ganreService.Add(A<string>._, A<Guid>._, A<string?>._)).Returns(true);
            A.CallTo(() => _ganreValidationUtils.ValidateName(A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _ganreValidationUtils.ValidateGanreId(A<string?>._)).Returns(ValidationResult.Success());
            A.CallTo(() => _ganreValidationUtils.ValidateMediaTypeId(A<string?>._)).Returns(ValidationResult.Success());

          

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

            console.Input.PushTextWithEnter("Rock");
            console.Input.PushTextWithEnter("Heavy guitars everywhere...");
            console.Input.PushKey(ConsoleKey.DownArrow);
            console.Input.PushKey(ConsoleKey.Enter);

            var app = new CommandAppTester(registrar, console: console);

            app.Configure(builder =>
            {
                builder.AddCommand<AddGanreCommand>("ganre add");
            });
            app.SetDefaultCommand<AddGanreCommand>();

            // Act
            var result = app.Run("-s");

            // Assert
            Assert.Equal(0, result.ExitCode);

            Assert.Contains($"Ganre was successfully added!", result.Output);
        }
    }
    
}
