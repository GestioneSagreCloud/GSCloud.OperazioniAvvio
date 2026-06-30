using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperazioniAvvio.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feste",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInizio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFine = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feste", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Impostazioni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFesta = table.Column<int>(type: "int", nullable: false),
                    GestioneCoperti = table.Column<bool>(type: "bit", nullable: false),
                    GestioneMenu = table.Column<bool>(type: "bit", nullable: false),
                    GestioneCategorie = table.Column<bool>(type: "bit", nullable: false),
                    StampaCarta = table.Column<bool>(type: "bit", nullable: false),
                    StampaLogo = table.Column<bool>(type: "bit", nullable: false),
                    StampaRicevuta = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impostazioni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impostazioni_Feste_IdFesta",
                        column: x => x.IdFesta,
                        principalTable: "Feste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Intestazioni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFesta = table.Column<int>(type: "int", nullable: false),
                    Titolo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edizione = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Luogo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intestazioni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Intestazioni_Feste_IdFesta",
                        column: x => x.IdFesta,
                        principalTable: "Feste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Impostazioni_IdFesta",
                table: "Impostazioni",
                column: "IdFesta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Intestazioni_IdFesta",
                table: "Intestazioni",
                column: "IdFesta",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Impostazioni");

            migrationBuilder.DropTable(
                name: "Intestazioni");

            migrationBuilder.DropTable(
                name: "Feste");
        }
    }
}
