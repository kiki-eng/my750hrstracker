using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_log_document_and_user_team_role_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "ActivityLogDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_TeamId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "TeamId", "RoleId" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogDocuments_TeamId",
                table: "ActivityLogDocuments",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogDocuments_Teams_TeamId",
                table: "ActivityLogDocuments",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogDocuments_Teams_TeamId",
                table: "ActivityLogDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId_TeamId_RoleId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogDocuments_TeamId",
                table: "ActivityLogDocuments");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "ActivityLogDocuments");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId", "TeamId" });
        }
    }
}
