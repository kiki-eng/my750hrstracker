using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_subscriptions_table_add_features_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Features",
                table: "Subscriptions");
        }
    }
}
