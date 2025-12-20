using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaControlApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class M2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_MediaTypes_MediaTypeId",
                table: "Author");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Author_GanreId",
                table: "Medias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Author",
                table: "Author");

            migrationBuilder.RenameTable(
                name: "Author",
                newName: "Ganres");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Medias",
                newName: "Rating_Value");

            migrationBuilder.RenameIndex(
                name: "IX_Author_MediaTypeId",
                table: "Ganres",
                newName: "IX_Ganres_MediaTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ganres",
                table: "Ganres",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ganres_MediaTypes_MediaTypeId",
                table: "Ganres",
                column: "MediaTypeId",
                principalTable: "MediaTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Ganres_GanreId",
                table: "Medias",
                column: "GanreId",
                principalTable: "Ganres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ganres_MediaTypes_MediaTypeId",
                table: "Ganres");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Ganres_GanreId",
                table: "Medias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ganres",
                table: "Ganres");

            migrationBuilder.RenameTable(
                name: "Ganres",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "Rating_Value",
                table: "Medias",
                newName: "Rating");

            migrationBuilder.RenameIndex(
                name: "IX_Ganres_MediaTypeId",
                table: "Author",
                newName: "IX_Author_MediaTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Author",
                table: "Author",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_MediaTypes_MediaTypeId",
                table: "Author",
                column: "MediaTypeId",
                principalTable: "MediaTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Author_GanreId",
                table: "Medias",
                column: "GanreId",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
