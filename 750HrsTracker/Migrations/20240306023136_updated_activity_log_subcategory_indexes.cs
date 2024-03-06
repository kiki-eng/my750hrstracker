using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class updated_activity_log_subcategory_indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActivityLogSubCategories_Slug",
                table: "ActivityLogSubCategories");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogSubCategories_Slug_LogActivityId",
                table: "ActivityLogSubCategories",
                columns: new[] { "Slug", "LogActivityId" },
                unique: true,
                filter: "[Slug] IS NOT NULL AND [LogActivityId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActivityLogSubCategories_Slug_LogActivityId",
                table: "ActivityLogSubCategories");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogSubCategories_Slug",
                table: "ActivityLogSubCategories",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");
        }
    }
}
