using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Helpers;
using MediaControlApp.Validators;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Tests.App.Commands.Authors
{
    public class ShowAuthorsCommandTests
    {
        private readonly IAuthorService _authorService;

        private readonly IServiceCollection _serviceCollection;

        public ShowAuthorsCommandTests()
        {
            _authorService = A.Fake<IAuthorService>();

            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IAuthorService>(s => _authorService);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }
        [Fact]
        public void ShowMediaTypesCommand_Execute_ReturnsInt()
        {
            //Arrange
            var authors = new Author[]
            {
                new Author(){Id=Guid.NewGuid(), Name="Kirk"},
                new Author(){Id=Guid.NewGuid(), Name="James hammet", Email="@@mail.com"},
                new Author(){Id=Guid.NewGuid(), Name="Ozzy Ossborn", CompanyName="Black Sabbath"}
            };
            A.CallTo(() => _authorService.GetAll()).Returns(authors);

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<ShowAuthorsCommand>("author show");
            });
            app.SetDefaultCommand<ShowAuthorsCommand>();

            // Act
            var result = app.Run();

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Equal(result.Output.Split('\n').Length, 10);

        }
    }
}
