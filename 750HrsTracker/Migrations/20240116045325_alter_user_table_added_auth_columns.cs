using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_user_table_added_auth_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPasswordResetAt",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPasswordResetAt",
                table: "Users");
        }
    }
}
