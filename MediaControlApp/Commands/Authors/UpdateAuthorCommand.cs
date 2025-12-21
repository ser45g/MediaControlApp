using MediaControlApp.Application.Services;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.MediaTypes
{
    public class UpdateAuthorCommand : AsyncCommand<UpdateAuthorCommand.Settings>
    {
        private readonly AuthorService _authorService;

        public UpdateAuthorCommand(AuthorService authorService)
        {
            _authorService = authorService;
        }

        public sealed class Settings : CommandSettings
        {
            /// <summary>
            /// Gets or sets the MediaTypeId
            /// </summary>
            [CommandArgument(0, "[AUTHORID]")]
            [Description("The media type's id to delete if.")]
            public string? AuthorId { get; set; }

            /// <summary>
            /// Gets or sets the MediaTypeName
            /// </summary>
            [CommandArgument(0, "[AUTHORNAME]")]
            [Description("The media type name to add. It must be unique")]
            public string? AuthorName { get; set; }

            [CommandArgument(0, "[COMPANYNAME]")]
            [Description("The media type name to add. It must be unique")]
            public string? CompanyName { get; set; }

            [CommandArgument(0, "[EMAIL]")]
            [Description("The media type name to add. It must be unique")]
            public string? Email { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether ShowSelect
            /// </summary>
            [CommandOption("-s|--show-select")]
            [DefaultValue(false)]
            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool ShowSelect { get; set; }
        }

        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {

            try
            {
                if (settings.ShowSelect)
                {
                    await HandleUpdateWithShowSelect(settings.AuthorId, settings.AuthorName, settings.CompanyName, settings.Email);
                }
                else
                {
                    await HandleUpdate();
                }

            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }

            return 0;
        }


        private async Task HandleUpdateWithShowSelect(string? authorId, string? authorName, string? companyName, string? email)
        {
            if (authorId != null && authorName != null)
            {
                Guid authorIdGuid = Guid.Parse(authorId);

                await _authorService.Update(authorIdGuid, authorName, companyName, email);
                AnsiConsole.MarkupLine($"[green]Author with Id [[{authorIdGuid}]] was successfully deleted![/]");
            }
            else
            {
                throw new Exception("Media type id and Media type name must be provided");
            }
        }

        /// <summary>
        /// The HandleRemove
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task HandleUpdate()
        {
            var authors = await _authorService.GetAll();

            var author = AnsiConsole.Prompt(new SelectionPrompt<Author>().Title("Please select the author you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(authors).UseConverter(x => x.Name));

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            var newName = AnsiConsole.Prompt(new TextPrompt<string>("Enter a new name: ").DefaultValue(author.Name).Validate(x =>
            {
                if (string.IsNullOrWhiteSpace(x))
                    return false;

                return true;
            }));

            var newCompanyName = AnsiConsole.Prompt(new TextPrompt<string?>("Enter a new company name: ").DefaultValue(author.CompanyName));

            var newEmail = AnsiConsole.Prompt(new TextPrompt<string?>("Enter a new email: ").DefaultValue(author.Email));

            await _authorService.Update(author.Id, newName, companyName:newCompanyName, email:newEmail);
            AnsiConsole.MarkupLine($"[green]Author was successfully updated![/]");
        }


    }
}
