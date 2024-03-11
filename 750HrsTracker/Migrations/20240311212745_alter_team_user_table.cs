using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_team_user_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOwnerSpouse",
                table: "Team_User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOwnerSpouse",
                table: "Team_User");
        }
    }
}
