using MediaControlApp.SharedSettings;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.Ganres
{
     public class GanreSettings : SelectableSettings
    {
        [CommandArgument(0, "[GANRENAME]")]
        [Description("The ganre name to add. It must be unique")]
        public string? Name { get; init; }

        [CommandArgument(0, "[MEDIATYPEID]")]
        [Description("The ganre's media type id")]
        public string? MediaTypeId { get; init; }

        [CommandArgument(0, "[DESCRIPTION]")]
        [Description("The description of the specified ganre")]
        public string? Description { get; init; }

    }
}
