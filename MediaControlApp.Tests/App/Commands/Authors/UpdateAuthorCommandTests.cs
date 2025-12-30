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
    public class UpdateAuthorCommandTests
    {
        private readonly IAuthorService _authorService;
        private readonly IAuthorValidationUtils _authorValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public UpdateAuthorCommandTests()
        {
            _authorService = A.Fake<IAuthorService>();
            _authorValidationUtils = A.Fake<IAuthorValidationUtils>();
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IAuthorService>(s => _authorService);

            _serviceCollection.AddTransient<IAuthorValidationUtils>(s => _authorValidationUtils);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }
        [Fact]
        public void UpdateAuthorCommand_Execute_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _authorService.Update(A<Guid>._, A<string>._)).Returns(true);

            A.CallTo(() => _authorValidationUtils.ValidateName(A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _authorValidationUtils.ValidateAuthorId(A<string?>._)).Returns(ValidationResult.Success());

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            string guidString = Guid.NewGuid().ToString();

            app.Configure(builder =>
            {
                builder.AddCommand<UpdateAuthorCommand>("author update");
            });
            app.SetDefaultCommand<UpdateAuthorCommand>();

            // Act
            var result = app.Run(guidString, "Diddy Jhong", "Forrests");

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Equal($"Author with Id [{guidString}] was successfully updated!", result.Output);

        }


        [Fact]
        public void UpdateAuthorCommandTests_Execute_Interactive_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _authorService.Update(A<Guid>._, A<string>._)).Returns(true);
            A.CallTo(() => _authorValidationUtils.ValidateName(A<string?>._)).Returns(ValidationResult.Success());

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

            console.Input.PushTextWithEnter("Ussemble");
            console.Input.PushTextWithEnter("Kino");
            console.Input.PushTextWithEnter("a@gmail.com");

            var app = new CommandAppTester(registrar, console: console);

            app.Configure(builder =>
            {
                builder.AddCommand<UpdateAuthorCommand>("author update");
            });
            app.SetDefaultCommand<UpdateAuthorCommand>();

            // Act
            var result = app.Run("-s");

            // Assert
            Assert.Equal(0, result.ExitCode);

            Assert.Contains($"Author was successfully updated!", result.Output);
        }
    }
}
