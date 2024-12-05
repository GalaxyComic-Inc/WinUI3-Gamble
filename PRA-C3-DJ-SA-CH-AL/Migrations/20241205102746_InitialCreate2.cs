using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRA_C3_DJ_SA_CH_AL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WinningGuess",
                table: "Bets");

            migrationBuilder.AddColumn<bool>(
                name: "Payed",
                table: "Bets",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payed",
                table: "Bets");

            migrationBuilder.AddColumn<string>(
                name: "WinningGuess",
                table: "Bets",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
