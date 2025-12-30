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

namespace MediaControlApp.Tests.App.Commands.Authors
{
    public class RemoveAuthorCommandTests
    {

        private readonly IAuthorService _authorService;
        private readonly IAuthorValidationUtils _authorValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public RemoveAuthorCommandTests()
        {
            _authorService = A.Fake<IAuthorService>();
            _authorValidationUtils = A.Fake<IAuthorValidationUtils>();
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IAuthorService>(s => _authorService);

            _serviceCollection.AddTransient<IAuthorValidationUtils>(s => _authorValidationUtils);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }

        [Fact]       
        public void RemoveAuthorCommandTests_Execute_ReturnsInt()
        {
            string? guidString = "4765d892-4211-4e3c-a022-86249eae09cc";
            //Arrange
            A.CallTo(() => _authorService.Remove(A<Guid>._)).Returns(true);

            A.CallTo(() => _authorValidationUtils.ValidateAuthorId(A<string?>._)).Returns(ValidationResult.Success());

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<RemoveAuthorCommand>("author remove");
            });
            app.SetDefaultCommand<RemoveAuthorCommand>();

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
            A.CallTo(() => _authorService.Remove(A<Guid>._)).Returns(true);

            A.CallTo(() => _authorValidationUtils.ValidateAuthorId(A<string?>._)).Returns(ValidationResult.Success());

            var authors = new Author[]
            {
                new Author(){Id=Guid.NewGuid(), Name="Kirk"},
                new Author(){Id=Guid.NewGuid(), Name="James hammet", Email="@@mail.com"},
                new Author(){Id=Guid.NewGuid(), Name="Ozzy Ossborn", CompanyName="Black Sabbath"}
            };
            A.CallTo(() => _authorService.GetAll()).Returns(authors);

            var registrar = new DITypeRegistar(_serviceCollection);
            var console = new TestConsole();
            console.Profile.Capabilities.Interactive = true;
            console.Input.PushKey(ConsoleKey.Enter);

            var app = new CommandAppTester(registrar, console: console);

            app.Configure(builder =>
            {
                builder.AddCommand<RemoveAuthorCommand>("author remove");
            });
            app.SetDefaultCommand<RemoveAuthorCommand>();

            // Act
            var result = app.Run("-s");

            // Assert
            Assert.Equal(0, result.ExitCode);
           // Assert.Contains($"Media Type [{mediaTypes[0].Name}] was successfully deleted!", result.Output);

        }


    }
}
