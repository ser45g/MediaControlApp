using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Ganres;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using MediaControlApp.SharedSettings;
using MediaControlApp.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.Medias
{
    [Description("Add a media")]
    public class AddMediaCommand : AsyncCommand<AddMediaCommand.MediaSettings>
    {
        private readonly IGanreService _ganreService;
        private readonly IMediaService _mediaService;
        private readonly IAuthorService _authorService;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IMediaValidationUtils _mediaValidationUtils;


        public AddMediaCommand(IGanreService ganreService, IMediaService mediaService, IAuthorService authorService, IAnsiConsole ansiConsole, IMediaValidationUtils mediaValidationUtils)
        {
            _ganreService = ganreService;
            _mediaService = mediaService;
            _authorService = authorService;
            _ansiConsole = ansiConsole;
            _mediaValidationUtils = mediaValidationUtils;
        }

        public class MediaSettings : SelectableSettings
        {
            [CommandArgument(0, "[MEDIATITLE]")]
            [Description("The media's title. It must be unique")]
            public string? Title { get; init; }

            [CommandArgument(0, "[GANREID]")]
            [Description("The ganre's id")]
            public string? GanreId { get; init; }

            [CommandArgument(0, "[AUTHORID]")]
            [Description("The author's id")]
            public string? AuthorId { get; init; }

            [CommandArgument(0, "[DESCRIPTION]")]
            [Description("The description")]
            public string? Description { get; init; }

            [CommandArgument(0, "[PUBLISHEDDATE]")]
            [Description("The publicashion date of the specified ganre")]
            public string? PublishedDate { get; init; }
            
            [CommandArgument(0, "[RATING]")]
            [Description("The rating of a media")]
            public string? Rating { get; init; }

            [CommandArgument(0, "[LASTCONSUMEDDATE]")]
            [Description("The date the specified media was consumed")]
            public string? LastConsumedDateUtc { get; init; }

           

        }

        protected override ValidationResult Validate(CommandContext context, MediaSettings settings)
        {
            if (!settings.ShowSelect)
            {
                var validationTask =_mediaValidationUtils.Validate(title:settings.Title, ganreId:settings.GanreId, authorId:settings.AuthorId, publishedDate: settings.PublishedDate, lastConsumedDate:settings.LastConsumedDateUtc, rating:settings.Rating);

                validationTask.Wait();

                ValidationResult validationResult = validationTask.Result;

                if (!validationResult.Successful)
                    return validationResult;
            }

            return base.Validate(context, settings);
        }


        protected async override Task<int> ExecuteAsync(CommandContext context, MediaSettings settings, CancellationToken cancellationToken)
        {
            
            if (settings.ShowSelect)
            {
                await HandleAdd();
            }
            else
            {
                await HandleAddWithShowSelect(settings.Title,settings.Description,settings.GanreId, settings.AuthorId,settings.PublishedDate, settings.LastConsumedDateUtc, settings.Rating);
            }
            
            return 0;
        }

        private async Task HandleAddWithShowSelect(string title, string? description, string ganreId, string authorId, string publishedDate, string? lastConsumedDate, string? rating )
        {
            Guid ganreIdGuid = Guid.Parse(ganreId);
            Guid authorIdGuid = Guid.Parse(authorId);
            DateTime publishedDateDate = DateTime.Parse(publishedDate);
            DateTime? lastConsumedDateDate = lastConsumedDate!=null? DateTime.Parse(lastConsumedDate):null;
            Rating? ratingObj = rating != null ? new Rating(double.Parse(rating)) : null;

            await _mediaService.Add(title,description,ganreIdGuid,publishedDateDate,authorIdGuid, ratingObj);

            _ansiConsole.MarkupLine($"[green]Media was successfully added![/]");
        }

        private async Task HandleAdd()
        {
            var ganres = await _ganreService.GetAll();

            var authors = await _authorService.GetAll();

            if (!authors.Any())
            {
                throw new Exception("No authors available");
            }

            if (!ganres.Any())
            {
                throw new Exception("No ganres available");
            }

            var title = _ansiConsole.Prompt(new TextPrompt<string>("Enter a media title: ").Validate(x =>
            {
                var task = _mediaValidationUtils.ValidateTitle(x);
                task.Wait();

                return task.Result.Successful;

            }));


            var ganre = _ansiConsole.Prompt(new SelectionPrompt<Ganre>().Title("Please select a ganre").PageSize(10).MoreChoicesText("Move up and down to reveal more ganres").AddChoices(ganres).UseConverter(x => x.Name));

            if (ganre == null)
            {
                throw new ArgumentNullException(nameof(ganre));
            }

            var author = _ansiConsole.Prompt(new SelectionPrompt<Author>().Title("Please select an author").PageSize(10).MoreChoicesText("Move up and down to reveal more authors").AddChoices(authors).UseConverter(x => x.Name));

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            var description = _ansiConsole.Prompt(new TextPrompt<string?>("Enter a description: ").DefaultValue(null).AllowEmpty());

            var publishedDate = _ansiConsole.Prompt(new TextPrompt<string>("Enter the publication date: ").DefaultValue(DateTime.Now.AddYears(-2).ToShortDateString()).Validate(x =>
            {
                var res = _mediaValidationUtils.ValidatePublishedDate(x);
                return res.Successful;
            }));
            
            var rating = _ansiConsole.Prompt(new TextPrompt<string?>($"Enter the rating (between {Rating.LOW_RATING} and {Rating.HIGH_RATING}").DefaultValue(null).AllowEmpty().Validate(x =>
            {
                var res = _mediaValidationUtils.ValidateRating(x);
                return res.Successful;
            }));

            var lastConsumedDate = _ansiConsole.Prompt(new TextPrompt<string?>("Enter the last consumed date: ").DefaultValue(null).AllowEmpty().Validate(x =>
            {
                var res = _mediaValidationUtils.ValidateLastConsumedDate(x);
                return res.Successful;
            }));

        

            await _mediaService.Add(title, description,ganre.Id, DateTime.Parse(publishedDate), author.Id, rating!=null? new Rating(double.Parse(rating)):null );
            _ansiConsole.MarkupLine($"[green]Media was successfully added![/]");
        }
    }
}
