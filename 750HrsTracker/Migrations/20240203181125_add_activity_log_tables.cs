using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class add_activity_log_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLogActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailablePropertyType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailablePropertyType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoursSpent = table.Column<int>(type: "int", nullable: false),
                    MinutesSpent = table.Column<int>(type: "int", nullable: false),
                    SecondsSpent = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityLogActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityLogCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_ActivityLogActivities_ActivityLogActivityId",
                        column: x => x.ActivityLogActivityId,
                        principalTable: "ActivityLogActivities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityLogs_ActivityLogCategories_ActivityLogCategoryId",
                        column: x => x.ActivityLogCategoryId,
                        principalTable: "ActivityLogCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Users_ActivityById",
                        column: x => x.ActivityById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemoteDirectoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogDocuments_ActivityLogs_ActivityLogId",
                        column: x => x.ActivityLogId,
                        principalTable: "ActivityLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogProperties",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogProperties", x => new { x.ActivityLogId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ActivityLogProperties_ActivityLogs_ActivityLogId",
                        column: x => x.ActivityLogId,
                        principalTable: "ActivityLogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityLogProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogDocuments_ActivityLogId",
                table: "ActivityLogDocuments",
                column: "ActivityLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogProperties_PropertyId",
                table: "ActivityLogProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_ActivityById",
                table: "ActivityLogs",
                column: "ActivityById");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_ActivityLogActivityId",
                table: "ActivityLogs",
                column: "ActivityLogActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_ActivityLogCategoryId",
                table: "ActivityLogs",
                column: "ActivityLogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CreatedById",
                table: "ActivityLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_TeamId",
                table: "ActivityLogs",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogDocuments");

            migrationBuilder.DropTable(
                name: "ActivityLogProperties");

            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "ActivityLogActivities");

            migrationBuilder.DropTable(
                name: "ActivityLogCategories");
        }
    }
}
