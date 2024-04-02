using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ai_poker_coach.Migrations
{
    /// <inheritdoc />
    public partial class Decimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SmallBlind",
                table: "Hand",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<decimal>(
                name: "MyStack",
                table: "Hand",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<decimal>(
                name: "BigBlindAnte",
                table: "Hand",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<decimal>(
                name: "BigBlind",
                table: "Hand",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<decimal>(
                name: "Ante",
                table: "Hand",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<decimal>(
                name: "Bet",
                table: "Action",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SmallBlind",
                table: "Hand",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric"
            );

            migrationBuilder.AlterColumn<int>(
                name: "MyStack",
                table: "Hand",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric"
            );

            migrationBuilder.AlterColumn<int>(
                name: "BigBlindAnte",
                table: "Hand",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric"
            );

            migrationBuilder.AlterColumn<int>(
                name: "BigBlind",
                table: "Hand",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric"
            );

            migrationBuilder.AlterColumn<int>(
                name: "Ante",
                table: "Hand",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric"
            );

            migrationBuilder.AlterColumn<int>(
                name: "Bet",
                table: "Action",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric"
            );
        }
    }
}
