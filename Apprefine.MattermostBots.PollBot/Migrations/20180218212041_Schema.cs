using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Apprefine.MattermostBots.PollBot.Migrations
{
    public partial class Schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "poll");

            migrationBuilder.RenameTable(
                name: "Polls",
                newSchema: "poll");

            migrationBuilder.RenameTable(
                name: "PollAnswers",
                newSchema: "poll");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Polls",
                schema: "poll");

            migrationBuilder.RenameTable(
                name: "PollAnswers",
                schema: "poll");
        }
    }
}
