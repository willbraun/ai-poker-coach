using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ai_poker_coach.Migrations.AppData
{
    /// <inheritdoc />
    public partial class HandSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Hands");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Hands",
                newName: "HandId");

            migrationBuilder.AlterColumn<string>(
                name: "Winners",
                table: "Hands",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerNotes",
                table: "Hands",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hands",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Analysis",
                table: "Hands",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    ActionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HandId = table.Column<int>(type: "integer", nullable: false),
                    Step = table.Column<int>(type: "integer", nullable: false),
                    Player = table.Column<int>(type: "integer", nullable: false),
                    Decision = table.Column<int>(type: "integer", nullable: false),
                    Bet = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.ActionId);
                    table.ForeignKey(
                        name: "FK_Actions_Hands_HandId",
                        column: x => x.HandId,
                        principalTable: "Hands",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HandId = table.Column<int>(type: "integer", nullable: false),
                    Step = table.Column<int>(type: "integer", nullable: false),
                    Player = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Suit = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_Cards_Hands_HandId",
                        column: x => x.HandId,
                        principalTable: "Hands",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actions_HandId",
                table: "Actions",
                column: "HandId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_HandId",
                table: "Cards",
                column: "HandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.RenameColumn(
                name: "HandId",
                table: "Hands",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Winners",
                table: "Hands",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlayerNotes",
                table: "Hands",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hands",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Analysis",
                table: "Hands",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Hands",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
