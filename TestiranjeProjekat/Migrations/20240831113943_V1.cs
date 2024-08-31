using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestiranjeProjekat.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Igraci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KorisnickoIme = table.Column<string>(type: "text", nullable: false),
                    Lozinka = table.Column<string>(type: "text", nullable: false),
                    Ime = table.Column<string>(type: "text", nullable: false),
                    Prezime = table.Column<string>(type: "text", nullable: false),
                    VodjaTima = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Igraci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizatori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KorisnickoIme = table.Column<string>(type: "text", nullable: false),
                    Lozinka = table.Column<string>(type: "text", nullable: false),
                    Ime = table.Column<string>(type: "text", nullable: false),
                    Prezime = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizatori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Turniri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Naziv = table.Column<string>(type: "text", nullable: false),
                    DatumOdrzavanja = table.Column<string>(type: "text", nullable: false),
                    MestoOdrzavanja = table.Column<string>(type: "text", nullable: false),
                    MaxBrojTimova = table.Column<int>(type: "integer", nullable: false),
                    TrenutniBrojTimova = table.Column<int>(type: "integer", nullable: false),
                    Nagrada = table.Column<int>(type: "integer", nullable: false),
                    OrganizatorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turniri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turniri_Organizatori_OrganizatorId",
                        column: x => x.OrganizatorId,
                        principalTable: "Organizatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prijave",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NazivTima = table.Column<string>(type: "text", nullable: false),
                    PotrebanBrojSlusalica = table.Column<int>(type: "integer", nullable: false),
                    PotrebanBrojRacunara = table.Column<int>(type: "integer", nullable: false),
                    PotrebanBrojTastatura = table.Column<int>(type: "integer", nullable: false),
                    PotrebanBrojMiseva = table.Column<int>(type: "integer", nullable: false),
                    TurnirId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prijave", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prijave_Turniri_TurnirId",
                        column: x => x.TurnirId,
                        principalTable: "Turniri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrijavaIgracSpoj",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IgracId = table.Column<int>(type: "integer", nullable: false),
                    PrijavaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrijavaIgracSpoj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrijavaIgracSpoj_Igraci_IgracId",
                        column: x => x.IgracId,
                        principalTable: "Igraci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrijavaIgracSpoj_Prijave_PrijavaId",
                        column: x => x.PrijavaId,
                        principalTable: "Prijave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrijavaIgracSpoj_IgracId",
                table: "PrijavaIgracSpoj",
                column: "IgracId");

            migrationBuilder.CreateIndex(
                name: "IX_PrijavaIgracSpoj_PrijavaId",
                table: "PrijavaIgracSpoj",
                column: "PrijavaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prijave_TurnirId",
                table: "Prijave",
                column: "TurnirId");

            migrationBuilder.CreateIndex(
                name: "IX_Turniri_OrganizatorId",
                table: "Turniri",
                column: "OrganizatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrijavaIgracSpoj");

            migrationBuilder.DropTable(
                name: "Igraci");

            migrationBuilder.DropTable(
                name: "Prijave");

            migrationBuilder.DropTable(
                name: "Turniri");

            migrationBuilder.DropTable(
                name: "Organizatori");
        }
    }
}
