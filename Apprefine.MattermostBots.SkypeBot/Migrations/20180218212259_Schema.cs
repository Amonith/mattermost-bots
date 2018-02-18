using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SkypeBot.Migrations
{
    public partial class Schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "skype");

            migrationBuilder.RenameTable(
                name: "UserInfos",
                newSchema: "skype");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                newSchema: "skype");

            migrationBuilder.RenameTable(
                name: "Groups",
                newSchema: "skype");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserInfos",
                schema: "skype");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                schema: "skype");

            migrationBuilder.RenameTable(
                name: "Groups",
                schema: "skype");
        }
    }
}
