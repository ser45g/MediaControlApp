using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.SharedSettings
{
    public class SelectableSettings : CommandSettings
    {
        [CommandOption("-s|--show-select")]
        [DefaultValue(false)]
        [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
        public bool ShowSelect { get; init; }
    }
}
