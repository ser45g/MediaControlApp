using MediaControlApp.Application.Services;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using MediaControlApp.SharedSettings;
using MediaControlApp.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;


namespace MediaControlApp.Commands.Medias
{
    [Description("Update a media")]
    public class UpdateMediaCommand : AsyncCommand<UpdateMediaCommand.Settings>
    {
        private readonly IGanreService _ganreService;
        private readonly IMediaService _mediaService;
        private readonly IAuthorService _authorService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IMediaValidationUtils _mediaValidationUtils;

        public UpdateMediaCommand(IGanreService ganreService, IMediaService mediaService, IAuthorService authorService, IAnsiConsole ansiConsole, IMediaValidationUtils mediaValidationUtils)
        {
            _ganreService = ganreService;
            _mediaService = mediaService;
            _authorService = authorService;
            _ansiConsole = ansiConsole;
            _mediaValidationUtils = mediaValidationUtils;
        }

        public sealed class Settings : SelectableSettings
        {
            [CommandArgument(0, "[MEDIAID]")]
            [Description("The media's id to delete.")]
            public string? MediaId { get; init; }

            [CommandArgument(1, "[MEDIATITLE]")]
            [Description("The media's title. It must be unique")]
            public string? Title { get; init; }

            [CommandArgument(2, "[GANREID]")]
            [Description("The ganre's id")]
            public string? GanreId { get; init; }

            [CommandArgument(3, "[AUTHORID]")]
            [Description("The author's id")]
            public string? AuthorId { get; init; }

            [CommandArgument(4, "[DESCRIPTION]")]
            [Description("The description")]
            public string? Description { get; init; }

            [CommandArgument(5, "[PUBLISHEDDATE]")]
            [Description("The publicashion date of the specified ganre")]
            public string? PublishedDate { get; init; }

            [CommandArgument(6, "[LASTCONSUMEDDATE]")]
            [Description("The date the specified media was consumed")]
            public string? LastConsumedDateUtc { get; init; }

            [CommandArgument(7, "[RATING]")]
            [Description("The rating of a media")]
            public string? Rating { get; init; }

        }

        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (!settings.ShowSelect)
            {
                var mediaIdValidationResult = _mediaValidationUtils.ValidateMediaId(settings.MediaId);

                if (!mediaIdValidationResult.Successful)
                    return mediaIdValidationResult;

                var validationTask = _mediaValidationUtils.Validate(title: settings.Title, ganreId: settings.GanreId, authorId: settings.AuthorId, publishedDate: settings.PublishedDate, lastConsumedDate: settings.LastConsumedDateUtc, rating: settings.Rating);

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
                await HandleUpdate(cancellationToken);
            }
            else
            {
                await HandleUpdateWithShowSelect(settings.MediaId, settings.Title, settings.Description, settings.GanreId, settings.AuthorId, settings.PublishedDate, settings.LastConsumedDateUtc, settings.Rating, cancellationToken);
            }
       
            return 0;
        }

        private async Task HandleUpdateWithShowSelect(string mediaId, string title, string? description, string ganreId, string authorId, string publishedDate, string? lastConsumedDate, string? rating, CancellationToken cancellationToken = default)
        {
            Guid mediaIdGuid = Guid.Parse(mediaId);
            Guid ganreIdGuid = Guid.Parse(ganreId);
            Guid authorIdGuid = Guid.Parse(authorId);
            DateTime publishedDateDate = DateTime.Parse(publishedDate);
            DateTime? lastConsumedDateDate = lastConsumedDate != null ? DateTime.Parse(lastConsumedDate) : null;
            Rating? ratingObj = rating != null ? new Rating(double.Parse(rating)) : null;

            await _mediaService.Update(id:mediaIdGuid, title:title, description:description, ganreId:ganreIdGuid, publishedDate:publishedDateDate, authorId:authorIdGuid, rating:ratingObj, lastConsumedDate:lastConsumedDateDate, cancellationToken:cancellationToken);

            _ansiConsole.MarkupLine($"[green]Media was successfully updated![/]");
        }

        private async Task HandleUpdate(CancellationToken cancellationToken = default)
        {
            var ganres = await _ganreService.GetAll(cancellationToken);
            var authors = await _authorService.GetAll(cancellationToken);
            var medias = await _mediaService.GetAll(cancellationToken);

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

            var media = _ansiConsole.Prompt(new SelectionPrompt<Media>().Title("Please select the media you want to update").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(medias).UseConverter(x => x.Title));

            if (media == null)
            {
                throw new ArgumentNullException(nameof(media));
            }


            var newTitle = _ansiConsole.Prompt(new TextPrompt<string>("Enter a new title: ").Validate(x =>
            {
                var task = _mediaValidationUtils.ValidateTitle(x);
                task.Wait();

                return !task.Result.Successful;

            }));

            var newDescription = _ansiConsole.Prompt(new TextPrompt<string>("Enter a new description: ").AllowEmpty());

            var ganre = _ansiConsole.Prompt(new SelectionPrompt<Ganre>().Title("Please select a ganre").PageSize(10).MoreChoicesText("Move up and down to reveal more ganres").AddChoices(ganres).UseConverter(x => x.Name));

            if (ganre == null)
            {
                throw new ArgumentNullException(nameof(ganre));
            }


            var author = _ansiConsole.Prompt(new SelectionPrompt<Author>().Title("Please select a ganre").PageSize(10).MoreChoicesText("Move up and down to reveal more authors").AddChoices(authors).UseConverter(x => x.Name));

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            var publishedDate = _ansiConsole.Prompt(new TextPrompt<string>("Enter the publication date: ").DefaultValue(DateTime.Now.AddYears(-2).ToShortDateString()).Validate(x =>
            {
                var publishedDateValidationResult = _mediaValidationUtils.ValidatePublishedDate(x);
                return publishedDateValidationResult.Successful;
            }));

            var lastConsumedDate = _ansiConsole.Prompt(new TextPrompt<string?>("Enter the last consumed date: ").DefaultValue(null).Validate(x =>
            {
                var lastConsumedDateValidationResult = _mediaValidationUtils.ValidateLastConsumedDate(x);
                return lastConsumedDateValidationResult.Successful;
            }));

            var rating = _ansiConsole.Prompt(new TextPrompt<double?>($"Enter the rating (between {Rating.LOW_RATING} and {Rating.HIGH_RATING}").DefaultValue(null).AllowEmpty().Validate(x =>
            {
                var res = _mediaValidationUtils.ValidateRating(x.ToString());
                return res.Successful;
            }));

            await _mediaService.Update(media.Id, newTitle, newDescription, ganre.Id, DateTime.Parse(publishedDate), lastConsumedDate!=null?DateTime.Parse(lastConsumedDate):null,  author.Id, rating != null ? new Rating(rating.Value) : null, cancellationToken:cancellationToken);

            _ansiConsole.MarkupLine($"[green]Media was successfully updated![/]");
        }
    }
}
