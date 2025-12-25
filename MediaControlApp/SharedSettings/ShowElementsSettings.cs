using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.SharedSettings
{
    public class ShowElementsSettings : CommandSettings
    {
        [CommandOption("--limit <LIMIT>")]
        [Description("How many elements you want to see")]
        public int? Limit { get; init; }

        [CommandOption("--ascending")]
        [Description("Show media types in an ascending order")]
        public bool IsAscending { get; init; }
    }
}
