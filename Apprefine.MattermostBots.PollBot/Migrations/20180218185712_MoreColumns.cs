using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Apprefine.MattermostBots.PollBot.Migrations
{
    public partial class MoreColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Polls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "PollAnswers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "PollAnswers");
        }
    }
}
