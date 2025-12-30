using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Ganres;
using MediaControlApp.Commands.Medias;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using MediaControlApp.Helpers;
using MediaControlApp.Validators;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Tests.App.Commands.Medias
{
    public class ShowMediasCommandTests
    {
        private readonly IMediaService _mediaService;

        private readonly IServiceCollection _serviceCollection;

        public ShowMediasCommandTests()
        {
            _mediaService = A.Fake<IMediaService>();

            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IMediaService>(s => _mediaService);
        }
        [Fact]
        public void ShowMediaTypesCommand_Execute_ReturnsInt()
        {
            //Arrange
            var medias = new Media[]
            {
                new Media(){Title="Just a Title 1", Id=Guid.NewGuid(),GanreId=Guid.NewGuid(), PublishedDateUtc=DateTime.Now.AddYears(-3), Rating=new Rating(8),AuthorId=Guid.NewGuid(), Description="Hello"},
                new Media(){Title="Just a Title 2", Id=Guid.NewGuid(),GanreId=Guid.NewGuid(), PublishedDateUtc=DateTime.Now.AddYears(-3), Rating=new Rating(8),AuthorId=Guid.NewGuid(), Description=null},
                new Media(){Title="Just a Title 3", Id=Guid.NewGuid(),GanreId=Guid.NewGuid(), PublishedDateUtc=DateTime.Now.AddYears(-3), Rating=new Rating(8),AuthorId=Guid.NewGuid(), Description=""}
            };
            A.CallTo(() => _mediaService.GetAll()).Returns(medias);

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<ShowMediasCommand>("media show");
            });
            app.SetDefaultCommand<ShowMediasCommand>();

            // Act
            var result = app.Run();

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Equal(result.Output.Split('\n').Length, 10);

        }
    }
}
