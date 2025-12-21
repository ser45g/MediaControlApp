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
            migrationBuilder.CreateIndex(
                name: "IX_MediaTypes_Name",
                table: "MediaTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medias_Title",
                table: "Medias",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ganres_Name",
                table: "Ganres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_Name",
                table: "Authors",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MediaTypes_Name",
                table: "MediaTypes");

            migrationBuilder.DropIndex(
                name: "IX_Medias_Title",
                table: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Ganres_Name",
                table: "Ganres");

            migrationBuilder.DropIndex(
                name: "IX_Authors_Name",
                table: "Authors");
        }
    }
}
