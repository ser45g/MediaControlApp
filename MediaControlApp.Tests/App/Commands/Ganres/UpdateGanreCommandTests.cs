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
    public class UpdateGanreCommandTests
    {
        private readonly IGanreService _ganreService;
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IGanreValidationUtils _ganreValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public UpdateGanreCommandTests()
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
        public void UpdateGanreCommand_Execute_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _ganreService.Update(A<Guid>._, A<string>._, A<Guid>._, A<string?>._)).Returns(true);


            A.CallTo(() => _ganreValidationUtils.ValidateName(A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _ganreValidationUtils.ValidateGanreId(A<string?>._)).Returns(ValidationResult.Success());
            A.CallTo(() => _ganreValidationUtils.ValidateMediaTypeId(A<string?>._)).Returns(ValidationResult.Success());

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            string ganreIdGuidString = Guid.NewGuid().ToString();
            string mediaTypeIdGuidString = Guid.NewGuid().ToString();

            app.Configure(builder =>
            {
                builder.AddCommand<UpdateGanreCommand>("ganre update");
            });
            app.SetDefaultCommand<UpdateGanreCommand>();

            // Act
            var result = app.Run(ganreIdGuidString, "Diddy Jhong","Forrests", mediaTypeIdGuidString);

            // Assert
            Assert.Equal(0, result.ExitCode);
            //Assert.Equal($"Author with Id [{guidString}] was successfully updated!", result.Output);

        }


        [Fact]
        public void UpdateGanreCommandTests_Execute_Interactive_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _ganreService.Update(A<Guid>._, A<string>._, A<Guid>._, A<string?>._)).Returns(true);

            A.CallTo(() => _ganreValidationUtils.ValidateName(A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _ganreValidationUtils.ValidateGanreId(A<string?>._)).Returns(ValidationResult.Success());
            A.CallTo(() => _ganreValidationUtils.ValidateMediaTypeId(A<string?>._)).Returns(ValidationResult.Success());

            var mediaTypes = new MediaType[]
            {
                new MediaType(){ Id=Guid.NewGuid(), Name="music" },
                new MediaType(){ Id=Guid.NewGuid(), Name="books" },
                new MediaType(){ Id=Guid.NewGuid(), Name="anime" }
            };
            A.CallTo(() => _mediaTypeService.GetAll()).Returns(mediaTypes);
            
            var ganres = new Ganre[]
            {
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=mediaTypes[0].Id, Description="Hello", Name="Kirk"},
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=mediaTypes[2].Id, Description=null, Name="James hammet"},
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=mediaTypes[0].Id, Description="", Name="Ozzy Ossborn"}
            };
            A.CallTo(() => _ganreService.GetAll()).Returns(ganres);


            var registrar = new DITypeRegistar(_serviceCollection);
            var console = new TestConsole();
            console.Profile.Capabilities.Interactive = true;
            console.Input.PushKey(ConsoleKey.Enter);

            console.Input.PushTextWithEnter("Rock");
            console.Input.PushTextWithEnter("....description....");
            console.Input.PushKey(ConsoleKey.Enter);

            var app = new CommandAppTester(registrar, console: console);

            app.Configure(builder =>
            {
                builder.AddCommand<UpdateGanreCommand>("ganre update");
            });
            app.SetDefaultCommand<UpdateGanreCommand>();

            // Act
            var result = app.Run("-s");

            // Assert
            Assert.Equal(0, result.ExitCode);

            Assert.Contains($"Ganre was successfully updated!", result.Output);
        }
    }
}
