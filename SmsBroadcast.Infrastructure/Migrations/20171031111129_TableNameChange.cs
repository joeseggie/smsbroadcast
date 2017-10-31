using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmsBroadcast.Infrastructure.Migrations
{
    public partial class TableNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ScScheduledBroadcasts",
                table: "ScScheduledBroadcasts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RunOnceBroadcasts",
                table: "RunOnceBroadcasts");

            migrationBuilder.RenameTable(
                name: "ScScheduledBroadcasts",
                newName: "ScheduledBroadcast");

            migrationBuilder.RenameTable(
                name: "RunOnceBroadcasts",
                newName: "RunOnceBroadcast");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScheduledBroadcast",
                table: "ScheduledBroadcast",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RunOnceBroadcast",
                table: "RunOnceBroadcast",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ScheduledBroadcast",
                table: "ScheduledBroadcast");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RunOnceBroadcast",
                table: "RunOnceBroadcast");

            migrationBuilder.RenameTable(
                name: "ScheduledBroadcast",
                newName: "ScScheduledBroadcasts");

            migrationBuilder.RenameTable(
                name: "RunOnceBroadcast",
                newName: "RunOnceBroadcasts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScScheduledBroadcasts",
                table: "ScScheduledBroadcasts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RunOnceBroadcasts",
                table: "RunOnceBroadcasts",
                column: "Id");
        }
    }
}
