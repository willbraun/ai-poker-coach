using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ai_poker_coach.Migrations
{
    /// <inheritdoc />
    public partial class PotActions2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Winners", table: "Hand");

            migrationBuilder.DropColumn(name: "Pot", table: "Action");

            migrationBuilder.CreateTable(
                name: "Pot",
                columns: table => new
                {
                    PotId = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    Winner = table.Column<string>(type: "text", nullable: false),
                    HandId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pot", x => x.PotId);
                    table.ForeignKey(
                        name: "FK_Pot_Hand_HandId",
                        column: x => x.HandId,
                        principalTable: "Hand",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "PotAction",
                columns: table => new
                {
                    PotActionId = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    Player = table.Column<int>(type: "integer", nullable: false),
                    Bet = table.Column<decimal>(type: "numeric", nullable: false),
                    PotId = table.Column<int>(type: "integer", nullable: false),
                    HandId = table.Column<int>(type: "integer", nullable: false),
                    Step = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotAction", x => x.PotActionId);
                    table.ForeignKey(
                        name: "FK_PotAction_Hand_HandId",
                        column: x => x.HandId,
                        principalTable: "Hand",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_PotAction_Pot_PotId",
                        column: x => x.PotId,
                        principalTable: "Pot",
                        principalColumn: "PotId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(name: "IX_Pot_HandId", table: "Pot", column: "HandId");

            migrationBuilder.CreateIndex(name: "IX_PotAction_HandId", table: "PotAction", column: "HandId");

            migrationBuilder.CreateIndex(name: "IX_PotAction_PotId", table: "PotAction", column: "PotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PotAction");

            migrationBuilder.DropTable(name: "Pot");

            migrationBuilder.AddColumn<string>(
                name: "Winners",
                table: "Hand",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<decimal>(
                name: "Pot",
                table: "Action",
                type: "numeric",
                nullable: false,
                defaultValue: 0m
            );
        }
    }
}
