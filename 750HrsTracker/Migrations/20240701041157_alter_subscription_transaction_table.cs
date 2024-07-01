using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_subscription_transaction_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionTransactions_TeamSubscriptions_TeamSubscriptionSubscriptionId_TeamSubscriptionTeamId",
                table: "SubscriptionTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionTransactions_TeamSubscriptionSubscriptionId_TeamSubscriptionTeamId",
                table: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "TeamSubscriptionSubscriptionId",
                table: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "TeamSubscriptionTeamId",
                table: "SubscriptionTransactions");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                table: "SubscriptionTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "SubscriptionTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionTransactions_SubscriptionId",
                table: "SubscriptionTransactions",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionTransactions_TeamId",
                table: "SubscriptionTransactions",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionTransactions_Subscriptions_SubscriptionId",
                table: "SubscriptionTransactions",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionTransactions_Teams_TeamId",
                table: "SubscriptionTransactions",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionTransactions_Subscriptions_SubscriptionId",
                table: "SubscriptionTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionTransactions_Teams_TeamId",
                table: "SubscriptionTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionTransactions_SubscriptionId",
                table: "SubscriptionTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionTransactions_TeamId",
                table: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "SubscriptionTransactions");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamSubscriptionSubscriptionId",
                table: "SubscriptionTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeamSubscriptionTeamId",
                table: "SubscriptionTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionTransactions_TeamSubscriptionSubscriptionId_TeamSubscriptionTeamId",
                table: "SubscriptionTransactions",
                columns: new[] { "TeamSubscriptionSubscriptionId", "TeamSubscriptionTeamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionTransactions_TeamSubscriptions_TeamSubscriptionSubscriptionId_TeamSubscriptionTeamId",
                table: "SubscriptionTransactions",
                columns: new[] { "TeamSubscriptionSubscriptionId", "TeamSubscriptionTeamId" },
                principalTable: "TeamSubscriptions",
                principalColumns: new[] { "SubscriptionId", "TeamId" });
        }
    }
}
