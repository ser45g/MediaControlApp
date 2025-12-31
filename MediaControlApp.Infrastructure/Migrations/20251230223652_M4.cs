using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaControlApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class M4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublisedDateUtc",
                table: "Medias",
                newName: "PublishedDateUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublishedDateUtc",
                table: "Medias",
                newName: "PublisedDateUtc");
        }
    }
}
