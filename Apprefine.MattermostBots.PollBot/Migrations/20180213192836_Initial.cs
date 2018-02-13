using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Apprefine.MattermostBots.PollBot.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PollAnswers",
                columns: table => new
                {
                    PollId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Answer = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollAnswers", x => new { x.PollId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AvailableAnswers = table.Column<string>(maxLength: 4000, nullable: true),
                    ChannelId = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PollAnswers");

            migrationBuilder.DropTable(
                name: "Polls");
        }
    }
}
