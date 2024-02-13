using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class add_log_activity_sub_category_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "ActivityLogCategories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityLogCategoryId",
                table: "ActivityLogActivities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "ActivityLogActivities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityLogSubCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LogActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogSubCategories_ActivityLogActivities_LogActivityId",
                        column: x => x.LogActivityId,
                        principalTable: "ActivityLogActivities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogCategories_Slug",
                table: "ActivityLogCategories",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogActivities_ActivityLogCategoryId",
                table: "ActivityLogActivities",
                column: "ActivityLogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogActivities_Slug",
                table: "ActivityLogActivities",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogSubCategories_LogActivityId",
                table: "ActivityLogSubCategories",
                column: "LogActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogSubCategories_Slug",
                table: "ActivityLogSubCategories",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogActivities_ActivityLogCategories_ActivityLogCategoryId",
                table: "ActivityLogActivities",
                column: "ActivityLogCategoryId",
                principalTable: "ActivityLogCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogActivities_ActivityLogCategories_ActivityLogCategoryId",
                table: "ActivityLogActivities");

            migrationBuilder.DropTable(
                name: "ActivityLogSubCategories");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogCategories_Slug",
                table: "ActivityLogCategories");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogActivities_ActivityLogCategoryId",
                table: "ActivityLogActivities");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogActivities_Slug",
                table: "ActivityLogActivities");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "ActivityLogCategories");

            migrationBuilder.DropColumn(
                name: "ActivityLogCategoryId",
                table: "ActivityLogActivities");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "ActivityLogActivities");
        }
    }
}
