using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_properties_table_add_property_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTeamUser_Properties_PropertyId",
                table: "PropertyTeamUser");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTeamUser_Teams_TeamId",
                table: "PropertyTeamUser");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTeamUser_Users_UserId",
                table: "PropertyTeamUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyTeamUser",
                table: "PropertyTeamUser");

            migrationBuilder.RenameTable(
                name: "PropertyTeamUser",
                newName: "PropertyTeamUsers");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyTeamUser_UserId",
                table: "PropertyTeamUsers",
                newName: "IX_PropertyTeamUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyTeamUser_PropertyId",
                table: "PropertyTeamUsers",
                newName: "IX_PropertyTeamUsers_PropertyId");

            migrationBuilder.AddColumn<int>(
                name: "PropertyType",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyTeamUsers",
                table: "PropertyTeamUsers",
                columns: new[] { "TeamId", "UserId", "PropertyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTeamUsers_Properties_PropertyId",
                table: "PropertyTeamUsers",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTeamUsers_Teams_TeamId",
                table: "PropertyTeamUsers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTeamUsers_Users_UserId",
                table: "PropertyTeamUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTeamUsers_Properties_PropertyId",
                table: "PropertyTeamUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTeamUsers_Teams_TeamId",
                table: "PropertyTeamUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTeamUsers_Users_UserId",
                table: "PropertyTeamUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyTeamUsers",
                table: "PropertyTeamUsers");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "Properties");

            migrationBuilder.RenameTable(
                name: "PropertyTeamUsers",
                newName: "PropertyTeamUser");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyTeamUsers_UserId",
                table: "PropertyTeamUser",
                newName: "IX_PropertyTeamUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyTeamUsers_PropertyId",
                table: "PropertyTeamUser",
                newName: "IX_PropertyTeamUser_PropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyTeamUser",
                table: "PropertyTeamUser",
                columns: new[] { "TeamId", "UserId", "PropertyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTeamUser_Properties_PropertyId",
                table: "PropertyTeamUser",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTeamUser_Teams_TeamId",
                table: "PropertyTeamUser",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTeamUser_Users_UserId",
                table: "PropertyTeamUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
