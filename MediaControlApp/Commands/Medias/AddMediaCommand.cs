using MediaControlApp.Application.Services;
using MediaControlApp.Commands.Ganres;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Domain.Models.Media.ValueObjects;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.Medias
{
    [Description("Add a media")]
    public class AddMediaCommand : AsyncCommand<AddMediaCommand.Settings>
    {
        private readonly GanreService _ganreService;
        private readonly MediaService _mediaService;
        private readonly AuthorService _authorService;

        public AddMediaCommand(GanreService ganreService, MediaService mediaService, AuthorService authorService)
        {
            _ganreService = ganreService;
            _mediaService = mediaService;
            _authorService = authorService;
        }

        public sealed class Settings : CommandSettings
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

            [CommandArgument(0, "[LASTCONSUMEDDATE]")]
            [Description("The date the specified media was consumed")]
            public string? LastConsumedDateUtc { get; init; }

            [CommandArgument(0, "[RATING]")]
            [Description("The rating of a media")]
            public string? Rating { get; init; }

            [CommandOption("-s|--show-select")]
            [DefaultValue(false)]
            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool ShowSelect { get; init; }

        }

        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (!settings.ShowSelect)
            {
                var validationTask = MediaValidationUtils.Validate(_mediaService, title:settings.Title, ganreId:settings.GanreId, authorId:settings.AuthorId, publishedDate: settings.PublishedDate, lastConsumedDate:settings.LastConsumedDateUtc, rating:settings.Rating);

                validationTask.Wait();

                ValidationResult validationResult = validationTask.Result;

                if (!validationResult.Successful)
                    return validationResult;
            }

            return base.Validate(context, settings);
        }


        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            try
            {
                if (settings.ShowSelect)
                {
                    await HandleAdd();
                }
                else
                {
                    await HandleAddWithShowSelect(settings.Title,settings.Description,settings.GanreId, settings.AuthorId,settings.PublishedDate, settings.LastConsumedDateUtc, settings.Rating);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
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

            AnsiConsole.MarkupLine($"[green]Media was successfully added![/]");
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

            var title = AnsiConsole.Prompt(new TextPrompt<string>("Enter a media title: ").Validate(x =>
            {
                var task = MediaValidationUtils.ValidateTitle(_mediaService, x);
                task.Wait();

                return task.Result.Successful;

            }));

            var description = AnsiConsole.Prompt(new TextPrompt<string>("Enter a description: ").AllowEmpty());

            var ganre = AnsiConsole.Prompt(new SelectionPrompt<Ganre>().Title("Please select a ganre").PageSize(10).MoreChoicesText("Move up and down to reveal more ganres").AddChoices(ganres).UseConverter(x => x.Name));

            if (ganre == null)
            {
                throw new ArgumentNullException(nameof(ganre));
            }

            var author = AnsiConsole.Prompt(new SelectionPrompt<Author>().Title("Please select an author").PageSize(10).MoreChoicesText("Move up and down to reveal more authors").AddChoices(authors).UseConverter(x => x.Name));

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            var publishedDate = AnsiConsole.Prompt(new TextPrompt<string>("Enter the publication date: ").DefaultValue(DateTime.Now.AddYears(-2).ToShortDateString()));

            var lastConsumedDate = AnsiConsole.Prompt(new TextPrompt<string?>("Enter the last consumed date: ").DefaultValue(null).AllowEmpty());

            var rating = AnsiConsole.Prompt(new TextPrompt<double?>($"Enter the rating (between {Rating.LOW_RATING} and {Rating.HIGH_RATING}").DefaultValue(null).AllowEmpty().Validate(x =>
            {
                var res = MediaValidationUtils.ValidateRating(x.ToString());
                return res.Successful;
            }));

            await _mediaService.Add(title, description,ganre.Id, DateTime.Parse(publishedDate), author.Id, rating!=null? new Rating(rating.Value):null );
            AnsiConsole.MarkupLine($"[green]Media was successfully added![/]");
        }
    }
}
