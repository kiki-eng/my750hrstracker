using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_log_table_add_task_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "ActivityLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_TaskId",
                table: "ActivityLogs",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_ActivityLogSubCategories_TaskId",
                table: "ActivityLogs",
                column: "TaskId",
                principalTable: "ActivityLogSubCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_ActivityLogSubCategories_TaskId",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_TaskId",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "ActivityLogs");
        }
    }
}
