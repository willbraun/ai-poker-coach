using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ai_poker_coach.Migrations
{
    /// <inheritdoc />
    public partial class StringHandId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_Hands_HandId",
                table: "Action");

            migrationBuilder.DropForeignKey(
                name: "FK_Card_Hands_HandId",
                table: "Card");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluation_Hands_HandId",
                table: "Evaluation");

            migrationBuilder.DropForeignKey(
                name: "FK_Pot_Hands_HandId",
                table: "Pot");

            migrationBuilder.DropForeignKey(
                name: "FK_PotAction_Hands_HandId",
                table: "PotAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hands",
                table: "Hands");

            migrationBuilder.DropColumn(
                name: "HandId",
                table: "Hands");

            migrationBuilder.AlterColumn<string>(
                name: "HandId",
                table: "PotAction",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "HandId",
                table: "Pot",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Hands",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "HandId",
                table: "Evaluation",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "HandId",
                table: "Card",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "HandId",
                table: "Action",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hands",
                table: "Hands",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Hands_HandId",
                table: "Action",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Hands_HandId",
                table: "Card",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluation_Hands_HandId",
                table: "Evaluation",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pot_Hands_HandId",
                table: "Pot",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PotAction_Hands_HandId",
                table: "PotAction",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_Hands_HandId",
                table: "Action");

            migrationBuilder.DropForeignKey(
                name: "FK_Card_Hands_HandId",
                table: "Card");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluation_Hands_HandId",
                table: "Evaluation");

            migrationBuilder.DropForeignKey(
                name: "FK_Pot_Hands_HandId",
                table: "Pot");

            migrationBuilder.DropForeignKey(
                name: "FK_PotAction_Hands_HandId",
                table: "PotAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hands",
                table: "Hands");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Hands");

            migrationBuilder.AlterColumn<int>(
                name: "HandId",
                table: "PotAction",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "HandId",
                table: "Pot",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "HandId",
                table: "Hands",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "HandId",
                table: "Evaluation",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "HandId",
                table: "Card",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "HandId",
                table: "Action",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hands",
                table: "Hands",
                column: "HandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Hands_HandId",
                table: "Action",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Hands_HandId",
                table: "Card",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluation_Hands_HandId",
                table: "Evaluation",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pot_Hands_HandId",
                table: "Pot",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PotAction_Hands_HandId",
                table: "PotAction",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
