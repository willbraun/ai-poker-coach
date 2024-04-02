using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ai_poker_coach.Migrations
{
    /// <inheritdoc />
    public partial class ActionPot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Pot",
                table: "Action",
                type: "numeric",
                nullable: false,
                defaultValue: 0m
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Pot", table: "Action");
        }
    }
}
