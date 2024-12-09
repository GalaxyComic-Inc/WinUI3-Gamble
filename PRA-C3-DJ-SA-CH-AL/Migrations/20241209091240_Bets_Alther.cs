using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRA_C3_DJ_SA_CH_AL.Migrations
{
    /// <inheritdoc />
    public partial class Bets_Alther : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerGuessName",
                table: "Bets",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerGuessName",
                table: "Bets");
        }
    }
}
