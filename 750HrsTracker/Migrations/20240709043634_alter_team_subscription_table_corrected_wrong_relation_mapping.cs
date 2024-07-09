using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_team_subscription_table_corrected_wrong_relation_mapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamSubscriptions_Teams_SubscriptionId",
                table: "TeamSubscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_TeamSubscriptions_TeamId",
                table: "TeamSubscriptions",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamSubscriptions_Teams_TeamId",
                table: "TeamSubscriptions",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamSubscriptions_Teams_TeamId",
                table: "TeamSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_TeamSubscriptions_TeamId",
                table: "TeamSubscriptions");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamSubscriptions_Teams_SubscriptionId",
                table: "TeamSubscriptions",
                column: "SubscriptionId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
