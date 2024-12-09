using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_user_role_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId_TeamId_RoleId",
                table: "UserRoles");

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
                columns: new[] { "UserId", "TeamId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_TeamId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "TeamId", "RoleId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId_TeamId_RoleId",
                table: "UserRoles");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
        }
    }
}
