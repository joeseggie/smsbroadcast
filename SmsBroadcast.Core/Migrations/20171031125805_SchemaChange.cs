using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SmsBroadcast.Core.Migrations
{
    public partial class SchemaChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    From = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ScheduleDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false),
                    Subject = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    To = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedule");
        }
    }
}