using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ai_poker_coach.Migrations.AppData
{
    /// <inheritdoc />
    public partial class Hand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GameStyle = table.Column<int>(type: "integer", nullable: false),
                    PlayerCount = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    SmallBlind = table.Column<int>(type: "integer", nullable: false),
                    BigBlind = table.Column<int>(type: "integer", nullable: false),
                    Ante = table.Column<int>(type: "integer", nullable: false),
                    BigBlindAnte = table.Column<int>(type: "integer", nullable: false),
                    MyStack = table.Column<int>(type: "integer", nullable: false),
                    PlayerNotes = table.Column<string>(type: "text", nullable: false),
                    Winners = table.Column<string>(type: "text", nullable: false),
                    Analysis = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hands", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hands");
        }
    }
}
