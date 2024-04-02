using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ai_poker_coach.Migrations
{
    /// <inheritdoc />
    public partial class HandDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Action_Hand_HandId", table: "Action");

            migrationBuilder.DropForeignKey(name: "FK_Card_Hand_HandId", table: "Card");

            migrationBuilder.DropForeignKey(name: "FK_Evaluation_Hand_HandId", table: "Evaluation");

            migrationBuilder.DropForeignKey(name: "FK_Hand_AspNetUsers_ApplicationUserId", table: "Hand");

            migrationBuilder.DropForeignKey(name: "FK_Pot_Hand_HandId", table: "Pot");

            migrationBuilder.DropForeignKey(name: "FK_PotAction_Hand_HandId", table: "PotAction");

            migrationBuilder.DropPrimaryKey(name: "PK_Hand", table: "Hand");

            migrationBuilder.RenameTable(name: "Hand", newName: "Hands");

            migrationBuilder.RenameIndex(
                name: "IX_Hand_ApplicationUserId",
                table: "Hands",
                newName: "IX_Hands_ApplicationUserId"
            );

            migrationBuilder.AddPrimaryKey(name: "PK_Hands", table: "Hands", column: "HandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Hands_HandId",
                table: "Action",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Hands_HandId",
                table: "Card",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluation_Hands_HandId",
                table: "Evaluation",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Hands_AspNetUsers_ApplicationUserId",
                table: "Hands",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Pot_Hands_HandId",
                table: "Pot",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_PotAction_Hands_HandId",
                table: "PotAction",
                column: "HandId",
                principalTable: "Hands",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Action_Hands_HandId", table: "Action");

            migrationBuilder.DropForeignKey(name: "FK_Card_Hands_HandId", table: "Card");

            migrationBuilder.DropForeignKey(name: "FK_Evaluation_Hands_HandId", table: "Evaluation");

            migrationBuilder.DropForeignKey(name: "FK_Hands_AspNetUsers_ApplicationUserId", table: "Hands");

            migrationBuilder.DropForeignKey(name: "FK_Pot_Hands_HandId", table: "Pot");

            migrationBuilder.DropForeignKey(name: "FK_PotAction_Hands_HandId", table: "PotAction");

            migrationBuilder.DropPrimaryKey(name: "PK_Hands", table: "Hands");

            migrationBuilder.RenameTable(name: "Hands", newName: "Hand");

            migrationBuilder.RenameIndex(
                name: "IX_Hands_ApplicationUserId",
                table: "Hand",
                newName: "IX_Hand_ApplicationUserId"
            );

            migrationBuilder.AddPrimaryKey(name: "PK_Hand", table: "Hand", column: "HandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Hand_HandId",
                table: "Action",
                column: "HandId",
                principalTable: "Hand",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Hand_HandId",
                table: "Card",
                column: "HandId",
                principalTable: "Hand",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluation_Hand_HandId",
                table: "Evaluation",
                column: "HandId",
                principalTable: "Hand",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Hand_AspNetUsers_ApplicationUserId",
                table: "Hand",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Pot_Hand_HandId",
                table: "Pot",
                column: "HandId",
                principalTable: "Hand",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_PotAction_Hand_HandId",
                table: "PotAction",
                column: "HandId",
                principalTable: "Hand",
                principalColumn: "HandId",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
