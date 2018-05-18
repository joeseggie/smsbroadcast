using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmsBroadcast.Core.Migrations
{
    public partial class ScheduleCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Schedule",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Code",
                table: "Schedule",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedule_Code",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Schedule");
        }
    }
}
