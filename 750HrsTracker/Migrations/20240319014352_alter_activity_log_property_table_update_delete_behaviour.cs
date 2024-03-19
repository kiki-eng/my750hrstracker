using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_activity_log_property_table_update_delete_behaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogProperties_ActivityLogs_ActivityLogId",
                table: "ActivityLogProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogProperties_Properties_PropertyId",
                table: "ActivityLogProperties");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogProperties_ActivityLogs_ActivityLogId",
                table: "ActivityLogProperties",
                column: "ActivityLogId",
                principalTable: "ActivityLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogProperties_Properties_PropertyId",
                table: "ActivityLogProperties",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogProperties_ActivityLogs_ActivityLogId",
                table: "ActivityLogProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogProperties_Properties_PropertyId",
                table: "ActivityLogProperties");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogProperties_ActivityLogs_ActivityLogId",
                table: "ActivityLogProperties",
                column: "ActivityLogId",
                principalTable: "ActivityLogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogProperties_Properties_PropertyId",
                table: "ActivityLogProperties",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");
        }
    }
}
