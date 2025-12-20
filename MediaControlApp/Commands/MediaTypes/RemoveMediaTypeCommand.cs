using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Domain.Models.Media;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.MediaTypes
{
    public class RemoveMediaTypeCommand : AsyncCommand<RemoveMediaTypeCommand.Settings>
    {
        private readonly IMediaTypeRepo _mediaTypeRepo;
        public sealed class Settings : CommandSettings
        {
            
        }

        public RemoveMediaTypeCommand(IMediaTypeRepo mediaTypeRepo)
        {
            _mediaTypeRepo = mediaTypeRepo;
        }


        protected async override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {

            var mediaTypes = await _mediaTypeRepo.GetAll();
          
            var mediaType = AnsiConsole.Prompt(new SelectionPrompt<MediaType>().Title("Please select the media type you want to delete").PageSize(10).MoreChoicesText("Move up and down to reveal more media types").AddChoices(mediaTypes).UseConverter(x=>x.Name));

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }
            Guid selectedMediaTypeId = mediaType.Id;
            try
            {
                //await _mediaTypeRepo.Remove(selectedMediaTypeId);
                AnsiConsole.MarkupLine($"[green]Media Type [[{mediaType.Name}]] was successfully deleted![/]");
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
