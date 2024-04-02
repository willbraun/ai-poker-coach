using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ai_poker_coach.Migrations
{
    /// <inheritdoc />
    public partial class Evaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evaluation",
                columns: table => new
                {
                    EvaluationId = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    Value = table.Column<string>(type: "text", nullable: false),
                    HandId = table.Column<int>(type: "integer", nullable: false),
                    Step = table.Column<int>(type: "integer", nullable: false),
                    Player = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluation", x => x.EvaluationId);
                    table.ForeignKey(
                        name: "FK_Evaluation_Hand_HandId",
                        column: x => x.HandId,
                        principalTable: "Hand",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(name: "IX_Evaluation_HandId", table: "Evaluation", column: "HandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Evaluation");
        }
    }
}
