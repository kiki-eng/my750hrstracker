using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class updated_team_subscription_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamSubscriptions",
                table: "TeamSubscriptions");

            migrationBuilder.AlterColumn<string>(
                name: "SubscriptionTransactionId",
                table: "TeamSubscriptions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamSubscriptions",
                table: "TeamSubscriptions",
                columns: new[] { "SubscriptionId", "TeamId", "SubscriptionTransactionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamSubscriptions",
                table: "TeamSubscriptions");

            migrationBuilder.AlterColumn<string>(
                name: "SubscriptionTransactionId",
                table: "TeamSubscriptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamSubscriptions",
                table: "TeamSubscriptions",
                columns: new[] { "SubscriptionId", "TeamId" });
        }
    }
}
