using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestiranjeProjekat.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "potrebanBrojTastatura",
                table: "Prijave",
                newName: "PotrebanBrojTastatura");

            migrationBuilder.RenameColumn(
                name: "potrebanBrojSlusalica",
                table: "Prijave",
                newName: "PotrebanBrojSlusalica");

            migrationBuilder.RenameColumn(
                name: "potrebanBrojRacunara",
                table: "Prijave",
                newName: "PotrebanBrojRacunara");

            migrationBuilder.RenameColumn(
                name: "potrebanBrojMiseva",
                table: "Prijave",
                newName: "PotrebanBrojMiseva");

            migrationBuilder.AddColumn<int>(
                name: "TrenutniBrojTimova",
                table: "Turniri",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrenutniBrojTimova",
                table: "Turniri");

            migrationBuilder.RenameColumn(
                name: "PotrebanBrojTastatura",
                table: "Prijave",
                newName: "potrebanBrojTastatura");

            migrationBuilder.RenameColumn(
                name: "PotrebanBrojSlusalica",
                table: "Prijave",
                newName: "potrebanBrojSlusalica");

            migrationBuilder.RenameColumn(
                name: "PotrebanBrojRacunara",
                table: "Prijave",
                newName: "potrebanBrojRacunara");

            migrationBuilder.RenameColumn(
                name: "PotrebanBrojMiseva",
                table: "Prijave",
                newName: "potrebanBrojMiseva");
        }
    }
}
