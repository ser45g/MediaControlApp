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
    public class RemoveGanreCommandTests
    {
        private readonly IGanreService _ganreService;
        private readonly IGanreValidationUtils _ganreValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public RemoveGanreCommandTests()
        {
            _ganreService = A.Fake<IGanreService>();
            _ganreValidationUtils = A.Fake<IGanreValidationUtils>();
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IGanreService>(s => _ganreService);

            _serviceCollection.AddTransient<IGanreValidationUtils>(s => _ganreValidationUtils);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }

        [Fact]
        public void RemoveGanreCommandTests_Execute_ReturnsInt()
        {
            string? guidString = "4765d892-4211-4e3c-a022-86249eae09cc";
            //Arrange
            A.CallTo(() => _ganreService.Remove(A<Guid>._)).Returns(true);

            A.CallTo(() => _ganreValidationUtils.ValidateGanreId(A<string?>._)).Returns(ValidationResult.Success());

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<RemoveGanreCommand>("author remove");
            });
            app.SetDefaultCommand<RemoveGanreCommand>();

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
            A.CallTo(() => _ganreService.Remove(A<Guid>._)).Returns(true);

            A.CallTo(() => _ganreValidationUtils.ValidateGanreId(A<string?>._)).Returns(ValidationResult.Success());

            var ganres = new Ganre[]
            {
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=Guid.NewGuid(), Description="Hello", Name="Kirk"},
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=Guid.NewGuid(), Description=null, Name="James hammet"},
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=Guid.NewGuid(), Description="", Name="Ozzy Ossborn"}
            };
            A.CallTo(() => _ganreService.GetAll()).Returns(ganres);

            var registrar = new DITypeRegistar(_serviceCollection);
            var console = new TestConsole();
            console.Profile.Capabilities.Interactive = true;
            console.Input.PushKey(ConsoleKey.Enter);

            var app = new CommandAppTester(registrar, console: console);

            app.Configure(builder =>
            {
                builder.AddCommand<RemoveGanreCommand>("ganre remove");
            });
            app.SetDefaultCommand<RemoveGanreCommand>();

            // Act
            var result = app.Run("-s");

            // Assert
            Assert.Equal(0, result.ExitCode);
            // Assert.Contains($"Media Type [{mediaTypes[0].Name}] was successfully deleted!", result.Output);

        }


    }
}
