using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_log_tables_and_add_profile_pic_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_ActivityLogActivities_ActivityLogActivityId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_ActivityLogCategories_ActivityLogCategoryId",
                table: "ActivityLogs");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfilePictureId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserProfilePictures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemoteDirectoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfilePictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfilePictures_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfilePictures_UserId",
                table: "UserProfilePictures",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_ActivityLogActivities_ActivityLogActivityId",
                table: "ActivityLogs",
                column: "ActivityLogActivityId",
                principalTable: "ActivityLogActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_ActivityLogCategories_ActivityLogCategoryId",
                table: "ActivityLogs",
                column: "ActivityLogCategoryId",
                principalTable: "ActivityLogCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_ActivityLogActivities_ActivityLogActivityId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_ActivityLogCategories_ActivityLogCategoryId",
                table: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "UserProfilePictures");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_ActivityLogActivities_ActivityLogActivityId",
                table: "ActivityLogs",
                column: "ActivityLogActivityId",
                principalTable: "ActivityLogActivities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_ActivityLogCategories_ActivityLogCategoryId",
                table: "ActivityLogs",
                column: "ActivityLogCategoryId",
                principalTable: "ActivityLogCategories",
                principalColumn: "Id");
        }
    }
}
