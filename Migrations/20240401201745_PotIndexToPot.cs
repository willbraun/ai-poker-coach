using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ai_poker_coach.Migrations
{
    /// <inheritdoc />
    public partial class PotIndexToPot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PotIndex",
                table: "Pot",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PotIndex",
                table: "Pot");
        }
    }
}
