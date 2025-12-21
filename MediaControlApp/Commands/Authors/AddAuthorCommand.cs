using MediaControlApp.Application.Services;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Commands.Add;
using MediaControlApp.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.MediaTypes
{
   

    [Description("Add an author.")]
    public sealed class AddAuthorCommand : AsyncCommand<AddAuthorCommand.Settings>
    {
        private readonly AuthorService _authorService;

        public AddAuthorCommand(AuthorService authorService)
        {
            _authorService = authorService;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<AUTHORNAME>")]
            
            [Description("The auhtor's name")]
            public string AuthorName { get; set; }

            [CommandArgument(1, "<COMPANYNAME>")]
            [Description("The media type to add. It must be unique")]
            public string? CompanyName { get; set; }

            [CommandArgument(2, "<EMAIL>")]
            [Description("The media type to add. It must be unique")]
            public string? Email { get; set; }
        }
        protected override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.AuthorName))
            {
                return ValidationResult.Error("Author name can't be empty");
            }
            return base.Validate(context, settings);
        }
  
        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {


            try
            {
                await _authorService.Add(settings.AuthorName, settings.CompanyName, settings.Email);
                AnsiConsole.MarkupLine($"[green]Author was successfully added![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }
            return 0;
        }
    }
}
