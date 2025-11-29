using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kokos.Api.Migrations.UserEventDb
{
    /// <inheritdoc />
    public partial class LoginUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Preferencje",
                table: "Uzytkownicy",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Uzytkownicy_Login",
                table: "Uzytkownicy",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Uzytkownicy_Login",
                table: "Uzytkownicy");

            migrationBuilder.DropColumn(
                name: "Preferencje",
                table: "Uzytkownicy");
        }
    }
}
