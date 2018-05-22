using Microsoft.EntityFrameworkCore.Migrations;

namespace SmsBroadcast.Core.Migrations
{
    public partial class RemoveUniqueScheduleCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedule_Code",
                table: "Schedule");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Code",
                table: "Schedule",
                column: "Code",
                unique: true);
        }
    }
}