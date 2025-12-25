using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using MediaControlApp.SharedSettings;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;


namespace MediaControlApp.Commands.Medias
{
    [Description("Update a media")]
    public class UpdateMediaCommand : AsyncCommand<UpdateMediaCommand.Settings>
    {
        private readonly GanreService _ganreService;
        private readonly MediaService _mediaService;
        private readonly AuthorService _authorService;

        public UpdateMediaCommand(GanreService ganreService, MediaService mediaService, AuthorService authorService)
        {
            _ganreService = ganreService;
            _mediaService = mediaService;
            _authorService = authorService;
        }

        public sealed class Settings : MediaSettings
        {
            [CommandArgument(0, "[MEDIAID]")]
            [Description("The media's id to delete.")]
            public string? MediaId { get; init; }

        }

        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (!settings.ShowSelect)
            {
                var mediaIdValidationResult = MediaValidationUtils.ValidateMediaId(settings.MediaId);

                if (!mediaIdValidationResult.Successful)
                    return mediaIdValidationResult;

                var validationTask = MediaValidationUtils.Validate(_mediaService, title: settings.Title, ganreId: settings.GanreId, authorId: settings.AuthorId, publishedDate: settings.PublishedDate, lastConsumedDate: settings.LastConsumedDateUtc, rating: settings.Rating);

                validationTask.Wait();

                ValidationResult validationResult = validationTask.Result;

                if (!validationResult.Successful)
                    return validationResult;
            }

            return base.Validate(context, settings);
        }


        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            
            if (settings.ShowSelect)
            {
                await HandleUpdate();
            }
            else
            {
                await HandleUpdateWithShowSelect(settings.MediaId, settings.Title, settings.Description, settings.GanreId, settings.AuthorId, settings.PublishedDate, settings.LastConsumedDateUtc, settings.Rating);
            }
       
            return 0;
        }

        private async Task HandleUpdateWithShowSelect(string mediaId, string title, string? description, string ganreId, string authorId, string publishedDate, string? lastConsumedDate, string? rating)
        {
            Guid mediaIdGuid = Guid.Parse(mediaId);
            Guid ganreIdGuid = Guid.Parse(ganreId);
            Guid authorIdGuid = Guid.Parse(authorId);
            DateTime publishedDateDate = DateTime.Parse(publishedDate);
            DateTime? lastConsumedDateDate = lastConsumedDate != null ? DateTime.Parse(lastConsumedDate) : null;
            Rating? ratingObj = rating != null ? new Rating(double.Parse(rating)) : null;

            await _mediaService.Update(id:mediaIdGuid, title:title, description:description, ganreId:ganreIdGuid, publishedDate:publishedDateDate, authorId:authorIdGuid, rating:ratingObj, lastConsumedDate:lastConsumedDateDate);

            AnsiConsole.MarkupLine($"[green]Media was successfully updated![/]");
        }

        private async Task HandleUpdate()
        {
            var ganres = await _ganreService.GetAll();
            var authors = await _authorService.GetAll();
            var medias = await _mediaService.GetAll();

            if (!medias.Any())
            {
                throw new Exception("No medias available");
            }

            if (!authors.Any())
            {
                throw new Exception("No authors available");
            }

            if (!ganres.Any())
            {
                throw new Exception("No ganres available");
            }



            var media = AnsiConsole.Prompt(new SelectionPrompt<Media>().Title("Please select the media you want to update").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(medias).UseConverter(x => x.Title));

            if (media == null)
            {
                throw new ArgumentNullException(nameof(media));
            }


            var newTitle = AnsiConsole.Prompt(new TextPrompt<string>("Enter a new title: ").Validate(x =>
            {
                var task = MediaValidationUtils.ValidateTitle(_mediaService, x);
                task.Wait();

                return !task.Result.Successful;

            }));

            var newDescription = AnsiConsole.Prompt(new TextPrompt<string>("Enter a new description: ").AllowEmpty());

            var ganre = AnsiConsole.Prompt(new SelectionPrompt<Ganre>().Title("Please select a ganre").PageSize(10).MoreChoicesText("Move up and down to reveal more ganres").AddChoices(ganres).UseConverter(x => x.Name));

            if (ganre == null)
            {
                throw new ArgumentNullException(nameof(ganre));
            }


            var author = AnsiConsole.Prompt(new SelectionPrompt<Author>().Title("Please select a ganre").PageSize(10).MoreChoicesText("Move up and down to reveal more authors").AddChoices(authors).UseConverter(x => x.Name));

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            var publishedDate = AnsiConsole.Prompt(new TextPrompt<string>("Enter the publication date: ").DefaultValue(DateTime.Now.AddYears(-2).ToShortDateString()).Validate(x =>
            {
                var publishedDateValidationResult = MediaValidationUtils.ValidatePublishedDate(x);
                return publishedDateValidationResult.Successful;
            }));

            var lastConsumedDate = AnsiConsole.Prompt(new TextPrompt<string?>("Enter the last consumed date: ").DefaultValue(null).Validate(x =>
            {
                var lastConsumedDateValidationResult = MediaValidationUtils.ValidateLastConsumedDate(x);
                return lastConsumedDateValidationResult.Successful;
            }));

            var rating = AnsiConsole.Prompt(new TextPrompt<double?>($"Enter the rating (between {Rating.LOW_RATING} and {Rating.HIGH_RATING}").DefaultValue(null).AllowEmpty().Validate(x =>
            {
                var res = MediaValidationUtils.ValidateRating(x.ToString());
                return res.Successful;
            }));

            await _mediaService.Update(media.Id, newTitle, newDescription, ganre.Id, DateTime.Parse(publishedDate), lastConsumedDate!=null?DateTime.Parse(lastConsumedDate):null,  author.Id, rating != null ? new Rating(rating.Value) : null);
            AnsiConsole.MarkupLine($"[green]Media was successfully updated![/]");
        }
    }
}
