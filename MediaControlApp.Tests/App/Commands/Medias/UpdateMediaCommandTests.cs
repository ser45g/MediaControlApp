using FakeItEasy;
using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Medias;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using MediaControlApp.Helpers;
using MediaControlApp.Validators;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli.Testing;
using Spectre.Console.Testing;

namespace MediaControlApp.Tests.App.Commands.Medias
{
    public class UpdateMediaCommandTests
    {
        private readonly IGanreService _ganreService;
        private readonly IAuthorService _authorService;
        private readonly IMediaService _mediaService;

        private readonly IMediaValidationUtils _mediaValidationUtils;
        private readonly IServiceCollection _serviceCollection;

        public UpdateMediaCommandTests()
        {
            _ganreService = A.Fake<IGanreService>();
            _mediaService = A.Fake<IMediaService>();
            _authorService = A.Fake<IAuthorService>();

            _mediaValidationUtils = A.Fake<IMediaValidationUtils>();
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddScoped<IGanreService>(s => _ganreService);
            _serviceCollection.AddScoped<IMediaService>(s => _mediaService);
            _serviceCollection.AddScoped<IAuthorService>(s => _authorService);

            _serviceCollection.AddTransient<IMediaValidationUtils>(s => _mediaValidationUtils);

            _serviceCollection.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();
        }
        [Fact]
        public void UpdateMediaCommand_Execute_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _mediaService.Update(A<Guid>._,A<string>._, A<string>._, A<Guid>._, A<DateTime>._, A<DateTime>._, A<Guid>._, A<Rating>._)).Returns(true);

            A.CallTo(() => _mediaValidationUtils.Validate(A<string?>._, A<string?>._, A<string?>._, A<string?>._, A<string?>._, A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _mediaValidationUtils.ValidateMediaId(A<string?>._)).Returns(ValidationResult.Success());

            var registrar = new DITypeRegistar(_serviceCollection);

            var app = new CommandAppTester(registrar);

            app.Configure(builder =>
            {
                builder.AddCommand<UpdateMediaCommand>("media update");
            });
            app.SetDefaultCommand<UpdateMediaCommand>();

            // Act
            var result = app.Run(Guid.NewGuid().ToString(), "AoT", Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), ".....", new DateTime(2009, 10, 9).ToShortDateString(), (8.2).ToString());

            // Assert
            Assert.Equal(0, result.ExitCode);
            //Assert.Contains($"Ganre was successfully added!", result.Output);

        }

        [Fact]
        public void UpdateMediaCommandTests_Execute_Interactive_ReturnsInt()
        {
            //Arrange
            A.CallTo(() => _mediaService.Update(A<Guid>._, A<string>._, A<string>._, A<Guid>._, A<DateTime>._, A<DateTime>._, A<Guid>._, A<Rating>._)).Returns(true);

            A.CallTo(() => _mediaValidationUtils.Validate(A<string?>._, A<string?>._, A<string?>._, A<string?>._, A<string?>._, A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _mediaValidationUtils.ValidateTitle(A<string?>._)).Returns(ValidationResult.Success());
            A.CallTo(() => _mediaValidationUtils.ValidateRating(A<string?>._)).Returns(ValidationResult.Success());
            A.CallTo(() => _mediaValidationUtils.ValidatePublishedDate(A<string?>._)).Returns(ValidationResult.Success());
            A.CallTo(() => _mediaValidationUtils.ValidateLastConsumedDate(A<string?>._)).Returns(ValidationResult.Success());

            A.CallTo(() => _mediaValidationUtils.ValidateMediaId(A<string?>._)).Returns(ValidationResult.Success());

            var medias = new Media[]
          {
                new Media(){Title="Just a Title 1", Id=Guid.NewGuid(),GanreId=Guid.NewGuid(), PublishedDateUtc=DateTime.Now.AddYears(-3), Rating=new Rating(8),AuthorId=Guid.NewGuid(), Description="Hello"},
                new Media(){Title="Just a Title 2", Id=Guid.NewGuid(),GanreId=Guid.NewGuid(), PublishedDateUtc=DateTime.Now.AddYears(-3), Rating=new Rating(8),AuthorId=Guid.NewGuid(), Description=null},
                new Media(){Title="Just a Title 3", Id=Guid.NewGuid(),GanreId=Guid.NewGuid(), PublishedDateUtc=DateTime.Now.AddYears(-3), Rating=new Rating(8),AuthorId=Guid.NewGuid(), Description=""}
          };
            A.CallTo(() => _mediaService.GetAll()).Returns(medias);

            var ganres = new Ganre[]
            {
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=Guid.NewGuid(), Description="Hello", Name="Kirk"},
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=Guid.NewGuid(), Description=null, Name="James hammet"},
                new Ganre(){Id=Guid.NewGuid(),MediaTypeId=Guid.NewGuid(), Description="", Name="Ozzy Ossborn"}
            };
            A.CallTo(() => _ganreService.GetAll()).Returns(ganres);

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
            console.Input.PushTextWithEnter("NewTitle");
            console.Input.PushKey(ConsoleKey.Enter);
            console.Input.PushKey(ConsoleKey.Enter);

            console.Input.PushTextWithEnter("...description...");
            console.Input.PushTextWithEnter(DateTime.Now.AddYears(-4).ToString());
            console.Input.PushTextWithEnter((8.2).ToString());
            console.Input.PushTextWithEnter(DateTime.Now.AddYears(-1).ToString());


            var app = new CommandAppTester(registrar, console: console);

            app.Configure(builder =>
            {
                builder.AddCommand<AddMediaCommand>("media add");
            });
            app.SetDefaultCommand<AddMediaCommand>();

            // Act
            var result = app.Run("-s");

            // Assert
            Assert.Equal(0, result.ExitCode);

            //Assert.Contains($"Media was successfully added!", result.Output);
        }
    }
}
