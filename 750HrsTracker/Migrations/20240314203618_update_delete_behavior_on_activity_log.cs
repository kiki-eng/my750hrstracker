using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class update_delete_behavior_on_activity_log : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogDocuments_ActivityLogs_ActivityLogId",
                table: "ActivityLogDocuments");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogDocuments_ActivityLogs_ActivityLogId",
                table: "ActivityLogDocuments",
                column: "ActivityLogId",
                principalTable: "ActivityLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogDocuments_ActivityLogs_ActivityLogId",
                table: "ActivityLogDocuments");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogDocuments_ActivityLogs_ActivityLogId",
                table: "ActivityLogDocuments",
                column: "ActivityLogId",
                principalTable: "ActivityLogs",
                principalColumn: "Id");
        }
    }
}
