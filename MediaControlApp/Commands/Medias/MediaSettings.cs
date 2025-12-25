using MediaControlApp.SharedSettings;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MediaControlApp.Commands.Medias
{
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

        [CommandArgument(0, "[LASTCONSUMEDDATE]")]
        [Description("The date the specified media was consumed")]
        public string? LastConsumedDateUtc { get; init; }

        [CommandArgument(0, "[RATING]")]
        [Description("The rating of a media")]
        public string? Rating { get; init; }

    }
}
