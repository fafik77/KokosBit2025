using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace kokos.Api.Migrations.UserEventDb
{
    /// <inheritdoc />
    public partial class Eventy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wydarzenia",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nazwa = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Opis = table.Column<string>(type: "text", nullable: true),
                    OrganizatorId = table.Column<int>(type: "integer", nullable: false),
                    Typ = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    Godzina = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Wysokosc = table.Column<double>(type: "double precision", nullable: false),
                    Szerokosc = table.Column<double>(type: "double precision", nullable: false),
                    Zakonczone = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wydarzenia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wydarzenia_Uzytkownicy_OrganizatorId",
                        column: x => x.OrganizatorId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipantsConfirmed",
                columns: table => new
                {
                    EventInfoId = table.Column<long>(type: "bigint", nullable: false),
                    UczestnicyPotwierdzeniId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipantsConfirmed", x => new { x.EventInfoId, x.UczestnicyPotwierdzeniId });
                    table.ForeignKey(
                        name: "FK_EventParticipantsConfirmed_Uzytkownicy_UczestnicyPotwierdze~",
                        column: x => x.UczestnicyPotwierdzeniId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipantsConfirmed_Wydarzenia_EventInfoId",
                        column: x => x.EventInfoId,
                        principalTable: "Wydarzenia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipantsWilling",
                columns: table => new
                {
                    EventInfo1Id = table.Column<long>(type: "bigint", nullable: false),
                    UczestnicyChetniId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipantsWilling", x => new { x.EventInfo1Id, x.UczestnicyChetniId });
                    table.ForeignKey(
                        name: "FK_EventParticipantsWilling_Uzytkownicy_UczestnicyChetniId",
                        column: x => x.UczestnicyChetniId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipantsWilling_Wydarzenia_EventInfo1Id",
                        column: x => x.EventInfo1Id,
                        principalTable: "Wydarzenia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipantsConfirmed_UczestnicyPotwierdzeniId",
                table: "EventParticipantsConfirmed",
                column: "UczestnicyPotwierdzeniId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipantsWilling_UczestnicyChetniId",
                table: "EventParticipantsWilling",
                column: "UczestnicyChetniId");

            migrationBuilder.CreateIndex(
                name: "IX_Wydarzenia_OrganizatorId",
                table: "Wydarzenia",
                column: "OrganizatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventParticipantsConfirmed");

            migrationBuilder.DropTable(
                name: "EventParticipantsWilling");

            migrationBuilder.DropTable(
                name: "Wydarzenia");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");
        }
    }
}
